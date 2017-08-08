var GuerillaTrader;
if (!GuerillaTrader) GuerillaTrader = {
    Log: {},
    Console: {},
    Util: {},
    TradingAccount: {},
    Trade: {},
    Screenshot: {},
    MonteCarloSimulation: {},
    Market: {
        Markets: [
            { Name: "E-Mini NASDAQ 100", Symbol: "ES", TickSize: .25, TickValue: 5, InitialMargin: 4620 },
            { Name: "E-Mini S&P 500", Symbol: "NQ", TickSize: .25, TickValue: 12.50, InitialMargin: 4290 },
            { Name: "E-Mini Dow", Symbol: "YM", TickSize: 1, TickValue: 5, InitialMargin: 3905 },
            { Name: "Gold", Symbol: "GC", TickSize: .10, TickValue: 10, InitialMargin: 4345 },
            { Name: "Oil", Symbol: "CL", TickSize: .01, TickValue: 10, InitialMargin: 2750 }
        ]
    },
    TradeExitReasons: [
        { Value: 0, Display: "None", ExitPriceField: "EntryPrice" },
        { Value: 1, Display: "Target Hit", ExitPriceField: "ProfitTakerPrice" },
        { Value: 2, Display: "Stop Loss Hit", ExitPriceField: "StopLossPrice" },
        { Value: 3, Display: "Reversal Signal", ExitPriceField: "EntryPrice" },
        { Value: 4, Display: "End of Day", ExitPriceField: "EntryPrice" }
    ]
};

//[EnumDisplay("None")]
//None,
//    [EnumDisplay("Target Hit")]
//TargetHit,
//    [EnumDisplay("Stop Loss Hit")]
//StopLossHit,
//    [EnumDisplay("Reversal Signal")]
//ReversalSignal,
//    [EnumDisplay("End of Day")]
//EndOfDay

(function ($) {

    //Notification handler
    abp.event.on('abp.notifications.received', function (userNotification) {
        abp.notifications.showUiNotifyForUserNotification(userNotification);
    });

    //serializeFormToObject plugin for jQuery
    $.fn.serializeFormToObject = function () {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        return obj;
    };

    //Configure blockUI
    if ($.blockUI) {
        $.blockUI.defaults.baseZ = 2000;
    }
})(jQuery);

$(function () {
    $("body").on("click", ".expandScreenshot", GuerillaTrader.Util.expandScreenshotClick);

    $("#marketsModal").on("shown.bs.modal", function (e) {
        $("#marketsGrid").data("kendoGrid").refresh();
    });
});

$(document).ready(function () {
    var consoleHub = $.connection.consoleHub; //get a reference to the hub

    consoleHub.client.writeLine = function (line) { //register for incoming messages
        GuerillaTrader.Console.writeLine(line);
    };
});

GuerillaTrader.Console.clear = function () {
    $("#consoleWell").html("");
}

GuerillaTrader.Console.writeLine = function (line) {
    $("#consoleWell").prepend("<div>" + line + "</div>");
}

GuerillaTrader.Util.expandScreenshotClick = function () {
    var expandScreenshotModal = $("#expandScreenshotModal");
    expandScreenshotModal.find("img").attr("src", $(this).attr("src"));
    expandScreenshotModal.modal("show");
}

GuerillaTrader.Util.initForm = function (id, submitFunc) {
    var _$form = $('#' + id);

    _$form.validate({
        ignore: ":hidden:Not(.includeHidden), .ignoreValidation"
    });

    _$form.find('button[type=submit]')
        .click(function (e) {
            e.preventDefault();

            if (!_$form.valid()) {
                return;
            }

            var input = _$form.serializeFormToObject();

            _$form.find("select[multiple=multiple]").each(function () {
                var listBox = $(this);
                var id = listBox.attr("id");

                if (listBox.val() == null) {
                    input[id] = [];
                }
                else {
                    input[id] = listBox.val();
                }
            });

            abp.ui.setBusy('#' + id);

            submitFunc(input);
        });
}

GuerillaTrader.Util.showModalForm = function (id, clearForm) {
    var _$modal = $('#' + id);
    var _$form = _$modal.find("form");

    _$modal.modal("show");

    if (_$form.size() > 0) {
        setTimeout(function () {
            if (typeof (clearForm) == "undefined" || clearForm) {
                _$form.find('input').val("");
            }

            _$form.find('input:first').focus();
        }, 500);
    }
}

GuerillaTrader.Util.hideModalForm = function (id) {
    var _$modal = $('#' + id);
    abp.ui.clearBusy('#' + id);
    _$modal.modal("hide");
}

GuerillaTrader.Util.hideEditField = function (container, name) {
    var label = container.find("label[for=" + name + "]");
    if (label.size() > 0) label.closest(".editor-label").hide();

    var field = container.find("[name=" + name + "]");
    if (field.size() > 0) field.closest(".editor-field").hide();
}

GuerillaTrader.Log.showAddLogEntryModal = function () {
    $.ajax({
        type: "GET",
        url: abp.appPath + 'MarketLog/AddLogEntryModal',
        success: function (r) {
            $("#addLogEntryModalWrapper").html(r);
            GuerillaTrader.Util.initForm("addLogEntryForm", GuerillaTrader.Log.addLogEntry);
            GuerillaTrader.Util.showModalForm("addLogEntryModal", false);

            document.getElementById("pasteTargetScreenshotDbId").
                addEventListener("paste", function (e) {
                    GuerillaTrader.Util.handlePaste("ScreenshotDbId", e);
                });
        },
        contentType: "application/json"
    });
}

GuerillaTrader.Log.addLogEntry = function (input) {
    input.TradingAccountId = $("#activeTradingAccount").data("id");
    if (input.Screenshot == "{ id = Screenshot, class = includeHidden }") input.Screenshot = '';

    abp.services.app.marketLogEntry.add(input).done(function () {
        GuerillaTrader.Util.hideModalForm("addLogEntryModal");
        GuerillaTrader.Log.refresh();
    });
}

GuerillaTrader.Log.purge = function () {
    abp.services.app.marketLogEntry.purge().done(function () {
        GuerillaTrader.Log.refresh();
    });
}

GuerillaTrader.Log.refresh = function (input) {
    $("#logListView").data("kendoListView").dataSource.read();
}

GuerillaTrader.Util.handlePaste = function (fieldName, e) {
    for (var i = 0; i < e.clipboardData.items.length; i++) {
        var item = e.clipboardData.items[i];
        if (item.type.indexOf("image") > -1) {
            var f = item.getAsFile();
            var reader = new FileReader();

            reader.onloadend = function () {
                $.ajax({
                    type: "POST",
                    url: abp.appPath + 'Screenshots/SaveBase64',
                    data: JSON.stringify({
                        base64: this.result
                    }),
                    success: function (r) {
                        $("#pasteTarget" + fieldName).hide();
                        $("#img" + fieldName).attr("src", abp.appPath + 'Screenshots/Screenshot/' + r.result.id).show();
                        $("#" + fieldName).val(r.result.id);
                    },
                    contentType: "application/json"
                });

            };

            reader.readAsDataURL(f);
        } else {
            console.log("Discarding image paste data");
        }
    }
}

GuerillaTrader.TradingAccount.grid_edit = function (e) {
    GuerillaTrader.Util.hideEditField(e.container, "IsNew");
    GuerillaTrader.Util.hideEditField(e.container, "Id");
}

GuerillaTrader.TradingAccount.setActive = function (id, name) {
    $("#activeTradingAccount").html(name).data("id", id);
    abp.services.app.tradingAccount.setActive(id).done(function () {
        var tradingAccountsGrid = $("#tradingAccountsGrid");
        if (tradingAccountsGrid.size() > 0) {
            tradingAccountsGrid.data("kendoGrid").dataSource.read();
        }

        var logListView = $("#logListView");
        if (logListView.size() > 0) {
            logListView.data("kendoListView").dataSource.read();
        }
    });
}

GuerillaTrader.Trade.grid_edit = function (e) {
    GuerillaTrader.Util.hideEditField(e.container, "IsNew");
    GuerillaTrader.Util.hideEditField(e.container, "Id");
    GuerillaTrader.Util.hideEditField(e.container, "RefNumber");
    GuerillaTrader.Util.hideEditField(e.container, "Market");
    GuerillaTrader.Util.hideEditField(e.container, "ProfitLoss");
    GuerillaTrader.Util.hideEditField(e.container, "TradingAccount");
    GuerillaTrader.Util.hideEditField(e.container, "ProfitLossPerContract");
}

GuerillaTrader.Trade.showTradeModal = function (id) {
    $.ajax({
        type: "GET",
        url: abp.appPath + 'Trades/TradeModal?id=' + id,
        success: function (r) {
            $("#tradeModalWrapper").html(r);
            GuerillaTrader.Util.initForm("tradeForm", GuerillaTrader.Trade.saveTrade);
            GuerillaTrader.Util.showModalForm("tradeModal", false);

            if ($("#pasteTargetEntryScreenshotDbId").size() > 0) {
                document.getElementById("pasteTargetEntryScreenshotDbId").
                    addEventListener("paste", function (e) {
                        GuerillaTrader.Util.handlePaste("EntryScreenshotDbId", e);
                    });
            }

            if ($("#pasteTargetExitScreenshotDbId").size() > 0) {
                document.getElementById("pasteTargetExitScreenshotDbId").
                    addEventListener("paste", function (e) {
                        GuerillaTrader.Util.handlePaste("ExitScreenshotDbId", e);
                    });
            }
        },
        contentType: "application/json"
    });
}

GuerillaTrader.Trade.exitReasonSelect = function (e) {
    var exitReasonInt = parseInt(e.dataItem.Value);
    var exitReason = _.find(GuerillaTrader.TradeExitReasons, { 'Value': exitReasonInt });

    $("#ExitPrice").data("kendoNumericTextBox").value($("#" + exitReason.ExitPriceField).data("kendoNumericTextBox").value());
}

GuerillaTrader.Trade.entryDateChange = function () {
    $("#ExitDate").data("kendoDateTimePicker").value(this.value());
}

GuerillaTrader.Trade.entryPriceChange = function () {
    var value = this.value();
    $("#StopLossPrice").data("kendoNumericTextBox").value(value);
    $("#ProfitTakerPrice").data("kendoNumericTextBox").value(value);
}

GuerillaTrader.Trade.marketChange = function () {
    var id = this.value();

    abp.services.app.market.get(id).done(function (market) {
        $("#Timeframe").data("kendoNumericTextBox").value(market.mtt);

        var currencyControls = ["EntryPrice", "StopLossPrice", "ProfitTakerPrice", "ExitPrice"];

        _.forEach(currencyControls, function (id) {
            $("#" + id).data("kendoNumericTextBox").step(market.tickSize);
        });
    });
}

GuerillaTrader.Trade.purge = function () {
    abp.ui.setBusy('#tradesGrid');
    abp.services.app.trade.purge().done(function () {
        GuerillaTrader.Trade.refresh();
        abp.ui.clearBusy('#tradesGrid');
    });
}

GuerillaTrader.Trade.saveTrade = function (input) {
    if (input.EntryScreenshot == "{ id = EntryScreenshot, class = includeHidden }") input.EntryScreenshot = '';
    if (input.ExitScreenshot == "{ id = ExitScreenshot, class = includeHidden }") input.ExitScreenshot = '';

    abp.services.app.trade.save(input).done(function (reconcileTradingAccount) {
        GuerillaTrader.Util.hideModalForm("tradeModal");
        GuerillaTrader.Trade.refresh();
        GuerillaTrader.Log.refresh();

        if (reconcileTradingAccount) {
            abp.services.app.tradingAccount.reconcile().done(function () {
                GuerillaTrader.TradingAccount.refreshDetails();
            });
        }
    });
    
    GuerillaTrader.Util.hideModalForm("tradeModal");
}

GuerillaTrader.TradingAccount.purge = function () {
    abp.ui.setBusy('#tradesGrid');
    abp.services.app.tradingAccount.purge().done(function () {
        GuerillaTrader.TradingAccount.refreshDetails();
        GuerillaTrader.Trade.refresh();
        abp.ui.clearBusy('#tradesGrid');
    });
}

GuerillaTrader.TradingAccount.refreshDetails = function (input) {
    $("#tradingAccountDetailsListView").data("kendoListView").dataSource.read();
}

GuerillaTrader.Trade.refresh = function (input) {
    $("#tradesGrid").data("kendoGrid").dataSource.read();
}

GuerillaTrader.MonteCarloSimulation.showMonteCarloSimulationModal = function (id) {
    $.ajax({
        type: "GET",
        url: abp.appPath + 'MonteCarloSimulations/MonteCarloSimulationModal?id=' + id,
        success: function (r) {
            $("#monteCarloSimulationModalWrapper").html(r);
            GuerillaTrader.Util.initForm("monteCarloSimulationForm", GuerillaTrader.MonteCarloSimulation.saveMonteCarloSimulation);
            GuerillaTrader.Util.showModalForm("monteCarloSimulationModal", false);
        },
        contentType: "application/json"
    });
}

GuerillaTrader.MonteCarloSimulation.tradingAccountChange = function () {
    var id = this.value();

    //abp.services.app.market.get(id).done(function (market) {
    //    $("#Timeframe").data("kendoNumericTextBox").value(market.mtt);

    //    var currencyControls = ["EntryPrice", "StopLossPrice", "ProfitTakerPrice", "ExitPrice"];

    //    _.forEach(currencyControls, function (id) {
    //        $("#" + id).data("kendoNumericTextBox").step(market.tickSize);
    //    });
    //});
}

GuerillaTrader.MonteCarloSimulation.purge = function () {
    abp.ui.setBusy('#monteCarloSimulationsGrid');
    abp.services.app.monteCarloSimulation.purge().done(function () {
        GuerillaTrader.MonteCarloSimulation.refresh();
        abp.ui.clearBusy('#monteCarloSimulationsGrid');
    });
}

GuerillaTrader.MonteCarloSimulation.saveMonteCarloSimulation = function (input) {
    abp.services.app.monteCarloSimulation.save(input).done(function (reconcileTradingAccount) {
        GuerillaTrader.Util.hideModalForm("monteCarloSimulationModal");
        GuerillaTrader.MonteCarloSimulation.refresh();

        $("#consoleModal").modal("show");

        //GuerillaTrader.Log.refresh();

        //if (reconcileTradingAccount) {
        //    abp.services.app.tradingAccount.reconcile().done(function () {
        //        GuerillaTrader.TradingAccount.refreshDetails();
        //    });
        //}
    });

    GuerillaTrader.Util.hideModalForm("monteCarloSimulationModal");
}

GuerillaTrader.MonteCarloSimulation.refresh = function (input) {
    $("#monteCarloSimulationsGrid").data("kendoGrid").dataSource.read();
}

GuerillaTrader.Screenshot.test = function () {
    $.ajax({
        type: "POST",
        url: abp.appPath + 'Screenshots/Test',
        data: JSON.stringify({
            base64: "data:image/png;base64,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"
        }),
        success: function (r) {
            
        },
        contentType: "application/json"
    });
}
