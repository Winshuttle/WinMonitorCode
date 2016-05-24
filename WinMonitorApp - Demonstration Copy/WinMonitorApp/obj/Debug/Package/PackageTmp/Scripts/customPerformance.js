//Defining three coloured table division bars

function functionbtnHistory() {
    var noOfDays = 15;
    document.getElementById("spanForNoOfDays").innerHTML = noOfDays ;
    $("#hiddenFieldForHistoryPage").val(noOfDays);
    $("#totalDaysToShow").val(noOfDays);
    
    $("#nextHistoryIcon").hide();
    var currentTime = new Date();
    document.getElementById("presentTimeForHistory").innerHTML = (moment(currentTime).format('MMM DD HH:mm')+" UTC");
    $("#divForRecent").hide();
    $("#divForWeekHistory").show();
    var startOffset = -7;
    var endOffset = 0;
    $("#startOffset").val(startOffset);
    $("#endOffset").val(endOffset);
    getHistory(startOffset, endOffset);
}

function functionbtnRecent() {
    $(".rowForComponents").remove();
    var currentTime = new Date();
    document.getElementById("presentTimeForRecent").innerHTML=(moment(currentTime).format('MMM DD HH:mm')+" UTC");
    $("#divForRecent").show();
    $("#divForWeekHistory").hide();
    var companyId = $("#UserCompanyIdReturn", window.parent.document).val();
    var startOffset = -1;
    var endOffset = 0;
    var recentOrNot = true;

    $.ajax({
        url: '../getComponents',
        cache: false,
        async: false,
        data: { pstrCompanyId: companyId },
        success: function (data) {
            var componentId;
            var i;
            for (i = 0; i < data.length; i++) {
                componentId = data[i].DBCSId;
                
                $('#statusTableForRecent tr:last').after('<tr class="rowForComponents"><td>' + data[i].DBComponentName + '</td><td colspan="4"><div id="divForComponentOnRecent' + componentId + '" style="width:100%; height:10px"></div></td></tr>');
                $.ajax({
                    url: '../getEventTimeStored',
                    data: { pstrComponentId: componentId, pstrEventStartOffset: startOffset, pstrEventEndOffset: endOffset, pstrRecentOrNot: recentOrNot },
                    async: false,
                    cache: false,
                    success: function (data) {
                        var j;
                        var color;
                        var eachUnit = 100 / parseInt((endOffset - startOffset) * 24 * 60 * 60);
                        for (j = 0; j < data.length; j++) {
                            if (data[j].mStatus == "Operational")
                                color = 'green';
                            else if (data[j].mStatus == "Performance Degradation")
                                color = 'yellow';
                            else
                                color = 'red';

                            if (data[j].mLogId != -1) {
                                $("#divForComponentOnRecent" + componentId).append('<div class="clickable" id="' + data[j].mLogId + '" style="background-color:' + color + '; width:' + eachUnit * parseInt(data[j].mDiff) + '%; height:10px;float:left;" onclick ="getDetailsOfIncident(this)" ></div>');
                            }
                            else

                                $("#divForComponentOnRecent" + componentId).append('<div style="background-color:' + color + '; width:' + eachUnit * parseInt(data[j].mDiff) + '%; height:10px;float:left;"></div>');
                        }

                    },
                    error: function () { alert("error"); }

                });
            }
        },
        error: function () { alert("error in fetching events"); }
    });

    
}

function getDetailsOfIncident(component) {
    $("#divForDetails").val("");
    if (component.id != -1) {
        $.ajax({
            url: '../getDetailsOfIncident',
            data: { pstrLogId: component.id },
            cache: false,
            async: false,
            datatype: 'Json',
            success: function (data) {
                document.getElementById("divForStatusDetail").innerHTML = data.DBStatus;
                document.getElementById("spanForStartTime").innerHTML = moment(data.DBDateTimeStart).format('MMM DD HH:mm:ss') + " UTC";
                document.getElementById("spanForEndTime").innerHTML = moment(data.DBDateTimeEnd).format('MMM DD HH:mm:ss') + " UTC";
                document.getElementById("divForIncidentName").innerHTML = data.DBIncidentName;
                document.getElementById("divForIncidentDetails").innerHTML = data.DBIncidentDetails;
            },
            error: function () { alert("error in showing details"); }
        });
        $("#divForDetails").toggle('slide', { direction: 'right' }, 500);
    }
}

function showPreviousHistory()
{
    var totalDays = parseInt($("#totalDaysToShow").val());
    var noOfDays = parseInt($("#hiddenFieldForHistoryPage").val());
    $("#hiddenFieldForHistoryPage").val();
    $("#hiddenFieldForHistoryPage").val(noOfDays - 7);
    if (($("#hiddenFieldForHistoryPage").val()) >= 7) {
        $("#nextHistoryIcon").show();
    }
    if (($("#hiddenFieldForHistoryPage").val() - 7) >= 7) {
        $("#previousHistoryIcon").show();
    }
    else
    {
        $("#previousHistoryIcon").hide();
    }

    var startOffset = parseInt($("#startOffset").val());
    $("#startOffset").val(startOffset - 7);
    var endOffset = parseInt($("#endOffset").val());
    $("#endOffset").val(endOffset - 7);
    getHistory(parseInt($("#startOffset").val()), parseInt($("#endOffset").val()));
}

function showNextHistory()
{
    var totalDays = parseInt($("#totalDaysToShow").val());
    var noOfDays = parseInt($("#hiddenFieldForHistoryPage").val());
    $("#hiddenFieldForHistoryPage").val();
    $("#hiddenFieldForHistoryPage").val(noOfDays + 7);
    if (($("#hiddenFieldForHistoryPage").val()) <= totalDays) {
        $("#previousHistoryIcon").show();
    }
    if (($("#hiddenFieldForHistoryPage").val() + 7) <= totalDays) {
        $("#nextHistoryIcon").show();
    }
    else {
        $("#nextHistoryIcon").hide();
    }
    var startOffset = parseInt($("#startOffset").val());
    $("#startOffset").val(startOffset + 7);
    var endOffset = parseInt($("#endOffset").val());
    $("#endOffset").val(endOffset + 7);
    getHistory(parseInt($("#startOffset").val()), parseInt($("#endOffset").val()));
}

function getHistory(startOffset, endOffset)
{
    $(".rowForComponents").remove();
    var randomVar = startOffset;
    var companyId = $("#UserCompanyIdReturn", window.parent.document).val();
    var currentDate = new Date();
    var variableDate = new Date();
    var array = ["seventhDaySpan", "sixthDaySpan", "fifthDaySpan", "fourthDaySpan", "thirdDaySpan", "secondDaySpan", "firstDaySpan"];
    var intVar = 0;
    for (intVar = 0 ; intVar < 7; intVar++)
    {
        document.getElementById(array[intVar]).innerHTML = "";
        variableDate.setDate(currentDate.getDate() + randomVar);
        document.getElementById(array[intVar]).innerHTML = (moment(variableDate).format('MMM DD'));
        randomVar++;
    }
    
    var recentOrNot = false;
    $.ajax({
        url: '../getComponents',
        cache: false,
        async: false,
        data: { pstrCompanyId: companyId },
        success: function (data) {
            var componentId;
            var i;
            for (i = 0; i < data.length; i++) {
                componentId = data[i].DBCSId;

                $('#statusTableForWeekHistory tr:last').after('<tr class="rowForComponents"><td>' + data[i].DBComponentName + '</td><td colspan="7"><div id="divForComponentOnHistory' + componentId + '" style="width:100%; height:10px"></div></td></tr>');
                $.ajax({
                    url: '../getEventTimeStored',
                    data: { pstrComponentId: componentId, pstrEventStartOffset: startOffset, pstrEventEndOffset: endOffset, pstrRecentOrNot: recentOrNot },
                    async: false,
                    cache: false,
                    success: function (data) {
                        var j;
                        var color;
                        var eachUnit = 100 / parseInt((endOffset - startOffset) * 24 * 60 * 60);
                        for (j = 0; j < data.length; j++) {
                            if (data[j].mStatus == "Operational")
                                color = 'green';
                            else if (data[j].mStatus == "Performance Degradation")
                                color = 'yellow';
                            else
                                color = 'red';

                            if (data[j].mLogId != -1) {
                                $("#divForComponentOnHistory" + componentId).append('<div class="clickable" id="' + data[j].mLogId + '" style="background-color:' + color + '; width:' + eachUnit * parseInt(data[j].mDiff) + '%; height:10px;float:left;" onclick ="getDetailsOfIncident(this)" ></div>');
                            }
                            else

                                $("#divForComponentOnHistory" + componentId).append('<div style="background-color:' + color + '; width:' + eachUnit * parseInt(data[j].mDiff) + '%; height:10px;float:left;"></div>');
                        }

                    },
                    error: function () { alert("error"); }

                });
            }
        },
        error: function () { alert("error in fetching events"); }
    });
}