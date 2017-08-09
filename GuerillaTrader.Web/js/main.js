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

            $("#addLogEntryModal").on("hidden.bs.modal", function (e) {
                $("#addLogEntryModalWrapper").html("");
            });

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
                var isEntryScreenshot = fieldName == "EntryScreenshotDbId";
                var tradeType = isEntryScreenshot ? $("#TradeType").data("kendoComboBox").value() : 0;

                $.ajax({
                    type: "POST",
                    url: abp.appPath + 'Screenshots/SaveBase64',
                    data: JSON.stringify({
                        base64: this.result,
                        tradeType: tradeType
                    }),
                    success: function (r) {
                        $("#pasteTarget" + fieldName).hide();
                        $("#img" + fieldName).attr("src", abp.appPath + 'Screenshots/Screenshot/' + r.result.id).show();
                        $("#" + fieldName).val(r.result.id);

                        if (isEntryScreenshot) {
                            if (r.result.entryPrice > 0) {
                                $("#EntryPrice").data("kendoNumericTextBox").value(r.result.entryPrice)
                            }
                            if (r.result.stopLossPrice > 0) {
                                $("#StopLossPrice").data("kendoNumericTextBox").value(r.result.stopLossPrice)
                            }
                            if (r.result.profitTakerPrice > 0) {
                                $("#ProfitTakerPrice").data("kendoNumericTextBox").value(r.result.profitTakerPrice)
                            }
                        }
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

            $("#tradeModal").on("hidden.bs.modal", function (e) {
                $("#tradeModalWrapper").html("");
            });

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

GuerillaTrader.Screenshot.recognizeText = function (id, tradeType) {
    $.ajax({
        type: "GET",
        url: abp.appPath + 'Screenshots/RecognizeText?id=' + id + '&tradeType=' + tradeType,
        success: function (r) {
            console.log(r);
        },
        contentType: "application/json"
    });
}
