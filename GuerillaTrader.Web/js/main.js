var GuerillaTrader;
if (!GuerillaTrader) GuerillaTrader = {
    Log: {},
    Console: {},
    Util: {},
    TradingAccount: {},
    Trade: {},
    Screenshot: {},
    Stocks: {},
    MonteCarloSimulation: {},
    Countdown: {},
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
    ],
    PasteTradeTypes: {
        AddTradeFromPaste: 0,
        OpenCoveredStockPositions: 1,
        UpdateCoveredStockPositions: 2,
        OpenBullPutSpreadPositions: 3,
        UpdateBullPutSpreadPositions: 4
    }
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

    $('#marketTabs a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var tosMarketsScatter = $("#tosMarketsScatter");
        if (tosMarketsScatter.is(":visible")) {
            tosMarketsScatter.data("kendoChart").redraw();
        }

        var tosMarketsColumn = $("#tosMarketsColumn");
        if (tosMarketsColumn.is(":visible")) {
            tosMarketsColumn.data("kendoChart").redraw();
        }

        var qtMarketsScatter = $("#qtMarketsScatter");
        if (qtMarketsScatter.is(":visible")) {
            qtMarketsScatter.data("kendoChart").redraw();
        }

        var qtMarketsColumn = $("#qtMarketsColumn");
        if (qtMarketsColumn.is(":visible")) {
            qtMarketsColumn.data("kendoChart").redraw();
        }
    });

    $('#stockTabs a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var stocksGrid = $("#stocksGrid");
        if (stocksGrid.is(":visible")) {
            stocksGrid.data("kendoGrid").refresh();
        }

        var sectorsGrid = $("#sectorsGrid");
        if (sectorsGrid.is(":visible")) {
            sectorsGrid.data("kendoGrid").refresh();
        }
    });

    GuerillaTrader.Util.initForm("addTradeFromPasteForm", GuerillaTrader.Trade.addTradeFromPaste);
    //GuerillaTrader.Util.initForm("openCoveredStockPositionsForm", GuerillaTrader.Trade.openCoveredStockPositions);
    //GuerillaTrader.Util.initForm("updateCoveredStockPositionsForm", GuerillaTrader.Trade.updateCoveredStockPositions);
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

GuerillaTrader.Util.initForm = function (id, submitFunc, showBusy) {
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

            if (typeof (showBusy) != "undefined" && showBusy)
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

GuerillaTrader.Util.hideModalForm = function (id, showBusy) {
    var _$modal = $('#' + id);
    if (typeof (showBusy) != "undefined" && showBusy)
        abp.ui.clearBusy(_$modal.find("form"));
    _$modal.modal("hide");
}

GuerillaTrader.Util.hideEditField = function (container, name) {
    var label = container.find("label[for=" + name + "]");
    if (label.size() > 0) label.closest(".editor-label").hide();

    var field = container.find("[name=" + name + "]");
    if (field.size() > 0) field.closest(".editor-field").hide();
}

GuerillaTrader.Util.getScoreColor = function (val) {
    var color = '';
    if (val <= 0.00) color = '#0000FF'
    else if (val <= 1.00) color = '#0008F7'
    else if (val <= 2.00) color = '#000FF0'
    else if (val <= 3.00) color = '#0017E8'
    else if (val <= 4.00) color = '#001FE0'
    else if (val <= 5.00) color = '#0027D8'
    else if (val <= 6.00) color = '#002ED1'
    else if (val <= 7.00) color = '#0036C9'
    else if (val <= 8.00) color = '#003EC1'
    else if (val <= 9.00) color = '#0046B9'
    else if (val <= 10.00) color = '#004DB2'
    else if (val <= 11.00) color = '#0055AA'
    else if (val <= 12.00) color = '#005DA2'
    else if (val <= 13.00) color = '#00649B'
    else if (val <= 14.00) color = '#006C93'
    else if (val <= 15.00) color = '#00748B'
    else if (val <= 16.00) color = '#007C83'
    else if (val <= 17.00) color = '#00837C'
    else if (val <= 18.00) color = '#008B74'
    else if (val <= 19.00) color = '#00936C'
    else if (val <= 20.00) color = '#009B64'
    else if (val <= 21.00) color = '#00A25D'
    else if (val <= 22.00) color = '#00AA55'
    else if (val <= 23.00) color = '#00B24D'
    else if (val <= 24.00) color = '#00B946'
    else if (val <= 25.00) color = '#00C13E'
    else if (val <= 26.00) color = '#00C936'
    else if (val <= 27.00) color = '#00D12E'
    else if (val <= 28.00) color = '#00D827'
    else if (val <= 29.00) color = '#00E01F'
    else if (val <= 30.00) color = '#00E817'
    else if (val <= 31.00) color = '#00F00F'
    else if (val <= 32.00) color = '#00F708'
    else if (val <= 33.00) color = '#00FF00'
    else if (val <= 34.00) color = '#08FF00'
    else if (val <= 35.00) color = '#0FFF00'
    else if (val <= 36.00) color = '#17FF00'
    else if (val <= 37.00) color = '#1FFF00'
    else if (val <= 38.00) color = '#27FF00'
    else if (val <= 39.00) color = '#2EFF00'
    else if (val <= 40.00) color = '#36FF00'
    else if (val <= 41.00) color = '#3EFF00'
    else if (val <= 42.00) color = '#46FF00'
    else if (val <= 43.00) color = '#4DFF00'
    else if (val <= 44.00) color = '#55FF00'
    else if (val <= 45.00) color = '#5DFF00'
    else if (val <= 46.00) color = '#64FF00'
    else if (val <= 47.00) color = '#6CFF00'
    else if (val <= 48.00) color = '#74FF00'
    else if (val <= 49.00) color = '#7CFF00'
    else if (val <= 50.00) color = '#83FF00'
    else if (val <= 51.00) color = '#8BFF00'
    else if (val <= 52.00) color = '#93FF00'
    else if (val <= 53.00) color = '#9BFF00'
    else if (val <= 54.00) color = '#A2FF00'
    else if (val <= 55.00) color = '#AAFF00'
    else if (val <= 56.00) color = '#B2FF00'
    else if (val <= 57.00) color = '#B9FF00'
    else if (val <= 58.00) color = '#C1FF00'
    else if (val <= 59.00) color = '#C9FF00'
    else if (val <= 60.00) color = '#D1FF00'
    else if (val <= 61.00) color = '#D8FF00'
    else if (val <= 62.00) color = '#E0FF00'
    else if (val <= 63.00) color = '#E8FF00'
    else if (val <= 64.00) color = '#F0FF00'
    else if (val <= 65.00) color = '#F7FF00'
    else if (val <= 66.00) color = '#FFFF00'
    else if (val <= 67.00) color = '#FFF800'
    else if (val <= 68.00) color = '#FFF000'
    else if (val <= 69.00) color = '#FFE900'
    else if (val <= 70.00) color = '#FFE100'
    else if (val <= 71.00) color = '#FFDA00'
    else if (val <= 72.00) color = '#FFD200'
    else if (val <= 73.00) color = '#FFCB00'
    else if (val <= 74.00) color = '#FFC300'
    else if (val <= 75.00) color = '#FFBC00'
    else if (val <= 76.00) color = '#FFB400'
    else if (val <= 77.00) color = '#FFAC00'
    else if (val <= 78.00) color = '#FFA500'
    else if (val <= 79.00) color = '#FF9E00'
    else if (val <= 80.00) color = '#FF9600'
    else if (val <= 81.00) color = '#FF8E00'
    else if (val <= 82.00) color = '#FF8700'
    else if (val <= 83.00) color = '#FF8000'
    else if (val <= 84.00) color = '#FF7800'
    else if (val <= 85.00) color = '#FF7100'
    else if (val <= 86.00) color = '#FF6900'
    else if (val <= 87.00) color = '#FF6200'
    else if (val <= 88.00) color = '#FF5A00'
    else if (val <= 89.00) color = '#FF5300'
    else if (val <= 90.00) color = '#FF4B00'
    else if (val <= 91.00) color = '#FF4400'
    else if (val <= 92.00) color = '#FF3C00'
    else if (val <= 93.00) color = '#FF3400'
    else if (val <= 94.00) color = '#FF2D00'
    else if (val <= 95.00) color = '#FF2600'
    else if (val <= 96.00) color = '#FF1E00'
    else if (val <= 97.00) color = '#FF1700'
    else if (val <= 98.00) color = '#FF0F00'
    else if (val <= 99.00) color = '#FF0800'
    else if (val <= 100.00) color = '#FF0000'
    return color;
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
    if (input.ScreenshotDbId == "{ id = ScreenshotDbId, class = includeHidden }") input.ScreenshotDbId = '';

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

        GuerillaTrader.Trade.refresh();
        GuerillaTrader.TradingAccount.refreshDetails();
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

GuerillaTrader.Trade.showAddTradeFromPasteModal = function (id) {
    GuerillaTrader.Trade.openPasteModal(GuerillaTrader.PasteTradeTypes.AddTradeFromPaste);
}

GuerillaTrader.Trade.showOpenCoveredStockPositionsModal = function () {
    GuerillaTrader.Trade.openPasteModal(GuerillaTrader.PasteTradeTypes.OpenCoveredStockPositions);
}

GuerillaTrader.Trade.showUpdateCoveredStockPositionsModal = function () {
    GuerillaTrader.Trade.openPasteModal(GuerillaTrader.PasteTradeTypes.UpdateCoveredStockPositions);
}

GuerillaTrader.Trade.showOpenBullPutSpreadPositionsModal = function () {
    GuerillaTrader.Trade.openPasteModal(GuerillaTrader.PasteTradeTypes.OpenBullPutSpreadPositions);
}

GuerillaTrader.Trade.showUpdateBullPutSpreadPositionsModal = function () {
    GuerillaTrader.Trade.openPasteModal(GuerillaTrader.PasteTradeTypes.UpdateBullPutSpreadPositions);
}

GuerillaTrader.Trade.openPasteModal = function (pasteTradeType) {
    $("#TradingAccountId").data("kendoComboBox").value($("#activeTradingAccount").data("id"));
    $("#addTradeFromPasteModal").data("tradeType", pasteTradeType);
    GuerillaTrader.Util.showModalForm("addTradeFromPasteModal", false);
}

GuerillaTrader.Trade.addTradeFromPaste = function (input) {
    abp.ui.clearBusy('#addTradeFromPasteModal');

    switch ($("#addTradeFromPasteModal").data("tradeType")) {
        case GuerillaTrader.PasteTradeTypes.AddTradeFromPaste:
            abp.services.app.trade.addTradeFromPaste(input).done(function () {
                GuerillaTrader.Trade.refresh();
                GuerillaTrader.Log.refresh();
                $("#addTradeFromPasteModal #Trades").val("");

                abp.services.app.tradingAccount.reconcile(input.TradingAccountId, input.Date).done(function () {
                    GuerillaTrader.TradingAccount.refreshDetails();
                });
            });
            break;
        case GuerillaTrader.PasteTradeTypes.OpenCoveredStockPositions:
            abp.services.app.trade.openCoveredStockPositions(input).done(function () {
                GuerillaTrader.Trade.refresh();
                GuerillaTrader.Log.refresh();
                $("#addTradeFromPasteModal #Trades").val("");

                abp.services.app.tradingAccount.reconcile(input.TradingAccountId, input.Date).done(function () {
                    GuerillaTrader.TradingAccount.refreshDetails();
                });
            });
            break;
        case GuerillaTrader.PasteTradeTypes.UpdateCoveredStockPositions:
            abp.services.app.trade.updateCoveredStockPositions(input).done(function () {
                GuerillaTrader.Trade.refresh();
                GuerillaTrader.Log.refresh();
                $("#addTradeFromPasteModal #Trades").val("");

                abp.services.app.tradingAccount.reconcile(input.TradingAccountId, input.Date).done(function () {
                    GuerillaTrader.TradingAccount.refreshDetails();
                });
            });
            break;
        case GuerillaTrader.PasteTradeTypes.OpenBullPutSpreadPositions:
            abp.services.app.trade.openBullPutSpreadPositions(input).done(function () {
                GuerillaTrader.Trade.refresh();
                GuerillaTrader.Log.refresh();
                $("#addTradeFromPasteModal #Trades").val("");

                abp.services.app.tradingAccount.reconcile(input.TradingAccountId, input.Date).done(function () {
                    GuerillaTrader.TradingAccount.refreshDetails();
                });
            });
            break;
        case GuerillaTrader.PasteTradeTypes.UpdateBullPutSpreadPositions:
            abp.services.app.trade.updateBullPutSpreadPositions(input).done(function () {
                GuerillaTrader.Trade.refresh();
                GuerillaTrader.Log.refresh();
                $("#addTradeFromPasteModal #Trades").val("");

                abp.services.app.tradingAccount.reconcile(input.TradingAccountId, input.Date).done(function () {
                    GuerillaTrader.TradingAccount.refreshDetails();
                });
            });
            break;
    }   
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

GuerillaTrader.Countdown.startClick = function () {
    var countdown = $("#countdown");
    var countdownStart = $("#CountdownStart").data("kendoDateTimePicker");
    var countdownDuration = $("#CountdownDuration").data("kendoNumericTextBox");

    var durationStart = moment(countdownStart.value());
    var durationEnd = moment(countdownStart.value());
    durationEnd.add(countdownDuration.value(), "minutes");
    var diff = durationEnd - durationStart;
    var duration = moment.duration(diff);

    countdown.html(moment(duration.asMilliseconds()).format('mm:ss'));

    setInterval(function () {
        if (duration.asSeconds() == 0) {
            duration = moment.duration(diff);
        }
        else {
            duration = moment.duration(duration.asSeconds() - 1, 'seconds');
        }

        countdown.html(moment(duration.asMilliseconds()).format('mm:ss'));
    }, 1000);

    $("#setCountDownModal").modal("hide");
}

GuerillaTrader.Util.grid_read = function (options) {
    return { realRequest: options };
}

GuerillaTrader.Stocks.showGenerateStockReportsModal = function () {
    $.ajax({
        type: "GET",
        url: abp.appPath + 'Stocks/GenerateStockReportsModal?id=',
        success: function (r) {
            $("#generateStockReportsModalWrapper").html(r);
            GuerillaTrader.Util.initForm("generateStockReportsForm", GuerillaTrader.Stocks.saveGenerateStockReports, false);
            GuerillaTrader.Util.showModalForm("generateStockReportsModal", false);
            
        },
        contentType: "application/json"
    });
}

GuerillaTrader.Stocks.saveGenerateStockReports = function (input) {
    GuerillaTrader.Util.hideModalForm("generateStockReportsModal", false);
    $("#consoleModal").modal("show");

    abp.services.app.stock.deleteReports().done(function () {
        abp.services.app.stock.generateReports(input).done(function () {
            GuerillaTrader.Util.hideModalForm("generateStockReportsModal", false);
            GuerillaTrader.Stocks.refresh();
            GuerillaTrader.Stocks.updateSectorProperties()
        });
    });
}

GuerillaTrader.Stocks.refresh = function (input) {
    $("#stocksGrid").data("kendoGrid").dataSource.read();
}

GuerillaTrader.Stocks.updateSectorProperties = function () {
    abp.ui.setBusy('#sectorsGrid');
    abp.services.app.stock.updateSectorProperties().done(function () {
        $("#sectorsGrid").data("kendoGrid").dataSource.read();
        abp.ui.clearBusy('#sectorsGrid');
    });
}

GuerillaTrader.Stocks.updatePriceAndDates = function () {
    $("#consoleModal").modal("show");
    abp.services.app.stock.updatePriceAndDates().done(function () {
        GuerillaTrader.Stocks.refresh();
    });
}

function tradesGridActionsMenu_Select(e) {
    var funcId = $(e.item).attr("funcId");
    if (typeof (funcId) != "undefined") {
        switch (parseInt(funcId)) {
            case 0:
                GuerillaTrader.Trade.showTradeModal(0)
                break;
            case 1:
                GuerillaTrader.Trade.showAddTradeFromPasteModal(0)
                break;
            case 2:
                GuerillaTrader.Trade.showOpenCoveredStockPositionsModal(0)
                break;
            case 3:
                GuerillaTrader.Trade.purge()
                break;
            case 4:
                GuerillaTrader.Trade.showUpdateCoveredStockPositionsModal(0)
                break;
            case 5:
                GuerillaTrader.Trade.showOpenBullPutSpreadPositionsModal(0)
                break;
            case 6:
                GuerillaTrader.Trade.showUpdateBullPutSpreadPositionsModal(0)
                break;
        }
    }
}
