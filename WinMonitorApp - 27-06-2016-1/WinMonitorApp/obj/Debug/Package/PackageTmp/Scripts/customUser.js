    //function to be called on history ready page
        //function on history ready page 
            function historyReadyFunction()
            {
                //code to take value from Parent   
                var UserHistoryCompanyId = $("#UserCompanyIdReturn", window.parent.document).val();

                $.ajax({
                    type: "post",
                    url: "/User/jsonHistoryUserView",
                    // The key needs to match your method's input parameter (case-sensitive).
                    data: { mHistoryCompanyId: UserHistoryCompanyId },
                    dataType: "json",
                    async: false,
                    cache: false,
                    success: function (data) {
                        var i;
                        var newRow;
                        for (i = 0; i < data.length; i++) {
                            newRow = "<tr style='font-size:medium; font-weight:500; color:black;'><td>" + data[i].ComponentName + "</td><td>" + data[i].IncidentName + "</td><td>" + data[i].IncidentDetails + "</td><td>" + moment(data[i].GetDateTime).format('DD-MM-YYYY HH:mm:ss').toString() + "</td></tr>";
                            $("#HistoryTable").append(newRow);
                        }
                    },
                    error: function () {
                        alert("error");
                    }
                });

                $("#HistoryTable").DataTable(
                            {
                                "lengthMenu": [[10, 20, 50], [10, 20, 50]]
                            });
            }

    //Maintenance calender functions
        //function of Maintenance calendar 
            function readyCalender() {
                $.getScript('../../Scripts/fullcalendar.min.js', function () {
                    var companyId = $("#UserCompanyIdReturn", window.parent.document).val();
                    $('#calendar').fullCalendar({
                        header: {
                            left: 'prev,next today',
                            center: 'title',
                            right: 'month,agendaWeek,agendaDay'
                        },
                        aspectRatio: 1.80,
                        slotEventOverlap: false,
                        editable: false,
                        eventStartEditable: false,
                        eventBorderColor: "FFFFFF",
                        eventDurationEditable: false,

                        events: '../getExistingEventsInUser?pstrCompanyId=' + companyId,
                        eventTextColor: "#000000",
                        eventClick: function (calEvent) {
                            modalClickEventDetails(calEvent.id);
                        },
                        eventRender: function (event, element, view) {

                            element.find(".fc-event-time").text('(' + moment(event.start).format('MMM-DD HH:mm') + ')-(' + moment(event.end).format('MMM-DD HH:mm') + ')');
                        }
                    });
                });
            }

        //function to inactivate events that are before present time
            function InactivateEvents() {
                $.ajax({
                    url: 'InactivateEventsInUser',
                    async: false,
                    type: 'get',
                    cache: false
                });
            }

        //function to open click event modal
            function modalClickEventDetails(id) {
                displayEventDetails(id);
                $(function () {
                    function reposition() {
                        var modal = $(this),
                            dialog = modal.find('.modal-dialog');
                        modal.css('display', 'block');

                        // Dividing by two centers the modal exactly, but dividing by three 
                        // or four works better for larger screens.
                        dialog.css("margin-top", Math.max(0, ($(window).height() - dialog.height()) / 3));
                    }
                    // Reposition when a modal is shown
                    $('.modal').on('show.bs.modal', reposition);
                    // Reposition when the window is resized
                    $(window).on('resize', function () {
                        $('.modal:visible').each(reposition);
                    });
                });
                parent.$("#eventClickEventModalInUser").modal('show');
            }

        //to show event details
            function displayEventDetails(id) {
                $.ajax({
                    url: '/User/jsonEventClickInUser',
                    type: 'POST',
                    data: { jsonpstrgetEventId: id },
                    datatype: "json",
                    async: false,
                    cache: false,
                    success: function (data) {
                        var parentElement = $("#eventClickEventModalInUser", window.parent.document);
                        $(parentElement).contents().find("#EventTitleShowInUser").val(data[0].Title);
                        $(parentElement).contents().find("#EventDetailsShowInUser").val(data[0].Details);
                        $(parentElement).contents().find("#datetimepickerStartTimeShowInUser").val(moment(data[0].Start).format('YYYY-MM-DD HH:mm'));
                        $(parentElement).contents().find("#datetimepickerEndTimeShowInUser").val(moment(data[0].EndTime).format('YYYY-MM-DD HH:mm'));

                        //conditions for Maintenance 
                        if (data[0].Maintenance == "Maintenance") {
                            $(parentElement).contents().find("#selectTypeMaintenanceShowInUser").val("Maintenance");
                            $(parentElement).contents().find("#selectTypeMaintenanceShowInUser").css("color", "green");
                        }
                        else {
                            $(parentElement).contents().find("#selectTypeMaintenanceShowInUser").val("Update");
                            $(parentElement).contents().find("#selectTypeMaintenanceShowInUser").css("color", "blue");
                        }

                        //Conditions for status
                        if (data[0].Status == "Active") {
                            $(parentElement).contents().find("#StatusShowInUser").val("Active");
                            $(parentElement).contents().find("#StatusShowInUser").css("color", "green");
                        }
                        else {
                            $(parentElement).contents().find("#StatusShowInUser").val("Inactive");
                            $(parentElement).contents().find("#StatusShowInUser").css("color", "orange");
                        }
                    },
                    error: function () {
                        alert('there was an error while displaying events!');
                    },
                });
            }

        //function to show calender view of maintenance
            function showCalenderViewOfMaintenance() {
                document.getElementById("calenderViewSelect").style.backgroundColor = "#cccccc";
                document.getElementById("listViewSelect").style.backgroundColor = "#f5f5f5";
                $("#calendar").show();
                $("#listView").hide();
                if (!document.getElementById('calendar').hasChildNodes()) {
                    readyCalender();
                }

            }

        //function to show list view of maintenance
            function showListViewOfMaintenance() {
                document.getElementById("calenderViewSelect").style.backgroundColor = "#f5f5f5";
                document.getElementById("listViewSelect").style.backgroundColor = "#cccccc";
                var companyId = $("#UserCompanyIdReturn", window.parent.document).val();
                $("#calendar").hide();
                $("#listView").show();
                $("#listViewBody").html("");

                $.ajax({
                    url: '/User/getExistingEventsInUser',
                    type: 'post',
                    data: { pstrCompanyId: companyId },
                    datatype: 'json',
                    async: false,
                    cache: false,
                    success: function (data) {

                        var newRow, i;
                        for (i = 0; i < data.length; i++) {
                            $.ajax({
                                url: '/User/jsonEventClickInUser',
                                type: 'post',
                                data: { jsonpstrgetEventId: data[i].id },
                                datatype: 'json',
                                async: false,
                                cache: false,
                                success: function (data) {
                                    newRow = '<tr><td>' + data[0].Title + '</td><td>' + data[0].Details + '</td><td>' + moment(data[0].Start).format("DD-MM-YYYY hh:mm") + '</td><td>' + moment(data[0].EndTime).format("DD-MM-YYYY hh:mm") + '</td></tr>';
                                    $("#listViewBody").append(newRow);
                                }
                            });

                        }
                    },
                    error: function () {
                        //alert("error in fetching events for list View");
                    }
                })

            }

            //to add datatable class to table
            $("#listViewTable").ready(function () {
                $("#listViewTable").DataTable(
                    {
                        "lengthMenu": [[10, 20, 50], [10, 20, 50]]
                    });
            });


    //StatusUserView functions
            //function to be called on status page ready event
            function onStatusPageReady()
            {
                //code to take value from Parent
                var UserCompanyId = $("#UserCompanyIdReturn", window.parent.document).val();

                $.ajax({
                    url: '/User/getCompanyDetails',
                    type: 'Post',
                    data: { pIntCompanyId: UserCompanyId },
                    datatype: 'Json',
                    cache: false,
                    async: false,
                    success: function (data) {
                        var DataOfCompany = data;
                        $.ajax({
                            type: "POST",
                            url: "/User/jsonStatusUserView",
                            data: { CompanyId: UserCompanyId },
                            dataType: "json",
                            async: false,
                            cache: false,
                            success: function (data) {
                                var i;
                                var newRow;
                                for (i = 0; i < data.length; i++) {
                                    var storecolor = null;
                                    var storevalue = null;
                                    var storecolorStatus = null;

                                    //If else block 1
                                    if (data[i].DBCenterName == DataOfCompany.DBPrimaryCenter) {
                                        storecolor = 'color:green;';
                                        storevalue = 'Primary: ' + data[i].DBCenterName;
                                    }
                                    else {
                                        if (data[i].DBCenterName == DataOfCompany.DBSecondaryCenter) {
                                            storecolor = 'color:orange';
                                            storevalue = 'Secondary: ' + data[i].DBCenterName;

                                        }
                                    }

                                    //If else block 2
                                    if (data[i].DBStatus == "Operational"){
                                        storecolorStatus = 'color:green;';
                                        newRow = "<tr id='" + data[i].DBCSId + "'><td>" + data[i].DBComponentName + "</td><td style=" + storecolorStatus + ">" + data[i].DBStatus + "</td><td style=" + storecolor + ">" + storevalue + "</td></tr>";
                                    }
                                    else {
                                        if (data[i].DBStatus == "Performance Degradation") {
                                            storecolorStatus = 'color:orange';
                                            newRow = "<tr id='" + data[i].DBCSId + "'><td>" + data[i].DBComponentName + "</td><td style=" + storecolorStatus + "><span class='clickable' onclick='showIncidentDetailsModal(this)'>" + data[i].DBStatus + "</span></td><td style=" + storecolor + ">" + storevalue + "</td></tr>";
                                        }
                                        else {
                                            storecolorStatus = 'color:red';
                                            newRow = "<tr id='" + data[i].DBCSId + "'><td>" + data[i].DBComponentName + "</td><td style=" + storecolorStatus + "><span class='clickable' onclick='showIncidentDetailsModal(this)'>" + data[i].DBStatus + "</span></td><td style=" + storecolor + ">" + storevalue + "</td></tr>";
                                        }
                                    }


                            
                                    $("#statTable").append(newRow);
                                }
                            },
                            error: function () {
                                alert("error in showing ");
                            }
                        });

                    },
                    error: function () { alert("error in fetching company details");}
                });

        

                $("#statTable").DataTable(
                            {
                                "lengthMenu": [[10, 20, 50], [10, 20, 50]]
                            });

            }

            //function to submit the details for subscription
            function submitDetails() {

                if (($('#SubscribeName').val()) && ($('#SubscribeEmail').val())) {
                    var atRateIndex = $('#SubscribeEmail').val().indexOf("@@");
                    var dotIndex = $('#SubscribeEmail').val().lastIndexOf(".");
                    if ((dotIndex - atRateIndex) >= 2) {
                        $.ajax({
                            url: "/User/jsonAddSubscriptions",
                            type: 'post',
                            data: { pstrSubscriptionName: $('#SubscribeName').val(), pstrSubscriptionEmail: $('#SubscribeEmail').val(), pstrCompanyId: $("#UserCompanyIdReturn", window.parent.document).val() },
                            dataType: 'text',
                            async: false,
                            cache: false,
                            success: function (data) {
                                if (data == "Unsucessful: Subscriptions limit: 5 for company reached") {
                                    $('#ShowMessage').val(data);
                                    $('#ShowMessage').css("color", "red");
                                }
                                else {
                                    $('#ShowMessage').val(data);
                                    $('#ShowMessage').css("color", "green");
                                }
                            },
                            error: function () {
                                alert("Error in Subscription");
                            }
                        });
                    }
                    else {
                        alert("Incorrect Email Format");
                    }
                }
                else {
                    alert("Name and Email not filled");
                }
            }

            //For Opening Modal to show incident details
            function showIncidentDetailsModal(component) {
                var componentId = $(component).closest('tr').attr('id');
                fillIncidentDetailShowModal(componentId);

                $(function () {
                    function reposition() {
                        var modal = $(this),
                            dialog = modal.find('.modal-dialog');
                        modal.css('display', 'block');

                        // Dividing by two centers the modal exactly, but dividing by three 
                        // or four works better for larger screens.
                        dialog.css("margin-top", Math.max(0, ($(window).height() - dialog.height()) / 3));
                    }
                    // Reposition when a modal is shown
                    $('.modal').on('show.bs.modal', reposition);
                    // Reposition when the window is resized
                    $(window).on('resize', function () {
                        $('.modal:visible').each(reposition);
                    });
                });
                $("#showIncidentDetailsOnStatusModalInUser").modal('show');
            }

            //function to fill details in incident show modal
            function fillIncidentDetailShowModal(componentId) {
                $.ajax({
                    url: '/User/getIncidentDetailsOfComponent',
                    data: { pstrComponentId: componentId },
                    type: 'post',
                    datatype: 'json',
                    cache: false,
                    async: false,
                    success: function (data) {
                        $("#IncidentNameShowInUser").val(data.DBIncidentName);
                        $("#IncidentDetailsShowInUser").val(data.DBIncidentDetails);
                        $("#StartTimeShowInUser").val(moment(data.DBDateTimeStart).format('DD MMM HH:mm'));
                        if (data.DBDateTimeEnd != null) {
                            $("#EndTimeShowInUser").val(moment(data.DBDateTimeEnd).format('DD MMM HH:mm'));
                        }
                        else {
                            var expectedEndTime;
                            $.ajax({
                                url: '/User/getExpectedEndTime',
                                type: 'post',
                                datatype: 'Json',
                                data: { pstrComponentId: componentId },
                                async: false,
                                cache: false,
                                success: function (data) {
                                    expectedEndpresentOrNot = data.expectedEndpresentOrNot;
                                    if (data.expectedEndDate == null) {
                                        expectedEndTime = "Going On";
                                    }
                                    else {

                                        expectedEndTime = moment(data.expectedEndDate).format('MMM DD HH:mm:ss') + " UTC (Expected)";
                                    }
                                },
                                error: function () {
                                    alert("error in getting expected end date successfully");
                                }
                            });

                            $("#EndTimeShowInUser").val(expectedEndTime);


                        }
                    },
                    error: function () { alert("error in populating data in details modal"); }

                });
            }