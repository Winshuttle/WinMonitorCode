function readyCalender() {

    $.getScript('../Scripts/fullcalendar.min.js', function () {
        var companyId = document.getElementById("companyIdStoringTextField").value;
        if (companyId == null) {
            alert("Select Company First");
        }
        else
        {
            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();

            $('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                aspectRatio: 1.80,
                slotEventOverlap: false,
                editable: true,
                eventStartEditable: true,
                eventBorderColor : "FFFFFF",
                eventDurationEditable: true,
                events: '/Admin/getExistingEvents?pstrCompanyId=' + companyId,
                eventTextColor:"#000000",
                dayClick: function () {
                    modalAddDayClickEvent();
                },
                eventClick: function (calEvent, jsEvent, view) {
                    $("#HiddenEventId").val(calEvent.id);
                    modalClickEventDetails();

                },
                eventResize: function (event, delta, revertFunc) {
                    updateCalenderInDB(event.id, event.start, event.end);
                    $("#masterPageContentPlace").load("/Admin/EditMaintenanceCalendar/?companyId=" + companyId);
                },

                eventDrop: function (event, delta, revertFunc) {
                    updateCalenderInDB(event.id, event.start, event.end);
                    $("#masterPageContentPlace").load("/Admin/EditMaintenanceCalendar/?companyId=" + companyId);
                },
                eventRender: function (event, element, view) {

                    element.find(".fc-event-time").text('(' + moment(event.start).format('MMM-DD HH:mm') + ')-(' + moment(event.end).format('MMM-DD HH:mm') + ')');
                }
            });
        }
        
    });
}
//function to update calender in DB
    function updateCalenderInDB(eventId, startTime, endTime)
    {
        $.ajax({
            url: '/Admin/UpdateCalenderInDB',
            data: { pstrEventId: eventId, pstrNewEventStart: moment(startTime).format('YYYY-MM-DDTHH:mm:ssZ'), pstrNewEventEnd: moment(endTime).format('YYYY-MM-DDTHH:mm:ssZ') },
            type: 'get',
            datatype: 'Json',
            cache: false,
            async: false,
            success: function () {
                //alert("success");
            },
            error: function () { alert("error in updating calender"); }
        });
    }

//function to inactivate events that have been passed
    function InactivateEvents()
    {
        $.ajax({
            url: '/Admin/InactivateEvents',
            async: false,
            cache: false
        });
    }


// -------------------------------------------------------------------------------------------1--------------------------------------------------------------------------------------------
//function to open add event modal
function modalAddDayClickEvent() {
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
    $("#addDayClickEventModal").modal('show');
}

function addEvent() {

    var Title = $("#EventTitle").val();
    var Details = $("#EventDetails").val();
    var StartTime = $("#datetimepickerStartTime").val();
    var EndTime = $("#datetimepickerEndTime").val();
    var EventFor = $("#selectEventFor option:selected").val();
    var Maintenance = $("#selectTypeMaintenance option:selected").val();
    var companyId = document.getElementById("companyIdStoringTextField").value;

    $.ajax({
        url: '/Admin/jsonAddDayClickEvents',
        type: 'POST',
        data: { jsonpstrTitle: Title, jsonpstrDetails: Details, jsonpstrStartTime: StartTime, jsonpstrEndTime: EndTime, jsonpstrEventFor: EventFor, jsonpstrMaintenance: Maintenance, jsonpstrCompanyId:companyId },
        datatype: "text",
        async: false,
        cache: false,
        success: function (data) {
            //alert(data);
        },
        error: function () {
            alert('there was an error while adding events!');
        },
    });

    $("#addDayClickEventModal").on('hide.bs.modal', function () {
        $("#addDayClickEventModal").removeClass("modal-backdrop fade in");
    });
    $("#masterPageContentPlace").load("/Admin/EditMaintenanceCalendar");      /////?commonCheck=CheckIfTrueErrorWin
}




//--------------------------------------------------------------------------------------------------2-----------------------------------------------------------------------------------------
//function for eventClick
function modalClickEventDetails() {
    displayEventDetails();
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
    $("#eventClickEventModal").modal('show');
}
//to show event details
function displayEventDetails() {

    var getid = $("#HiddenEventId").val();
    $.ajax({
        url: '/Admin/jsonEventClick',
        type: 'POST',
        data: { jsonpstrgetEventId: getid },
        datatype: "json",
        async: false,
        cache: false,
        success: function (data) {
            $("#EventTitleShow").val(data[0].Title);
            $("#EventDetailsShow").val(data[0].Details);
            $("#datetimepickerStartTimeShow").val(moment(data[0].Start).format('YYYY-MM-DD HH:mm'));
            $("#datetimepickerEndTimeShow").val(moment(data[0].EndTime).format('YYYY-MM-DD HH:mm'));
            
            //conditions for Maintenance 
            if (data[0].Maintenance == "Maintenance") {
                $("#selectTypeMaintenanceShow").val("Maintenance");
                $("#selectTypeMaintenanceShow").css("color", "purple");
            }
            else {
               
                    $("#selectTypeMaintenanceShow").val("Updates");
                    $("#selectTypeMaintenanceShow").css("color", "blue");

            }
           
            //Conditions for status
            if (data[0].Status == "Active") {
                $("#StatusShow").val("Active");
                $("#StatusShow").css("color", "green");
            }
            else {
                $("#StatusShow").val("Inactive");
                $("#StatusShow").css("color", "orange");
            }
        },
        error: function () {
            alert('there was an error while displaying events!');
        },
    });
}

//for deleting event
function deleteEvent() {

    var EventId = $("#HiddenEventId").val();

    $.ajax({
        url: '/Admin/jsonEventClickDelete',
        type: 'POST',
        data: { jsonpstrEventId : EventId },
        datatype: "text",
        async: false,
        cache: false,
        success: function (data) {
            //alert(data);
        },
        error: function () {
            alert('there was an error while deleting events!');
        },
    });

    $("#eventClickEventModal").on('hide.bs.modal', function () {
        $("#eventClickEventModal").removeClass("modal-backdrop fade in");
    });
    $("#masterPageContentPlace").load("/Admin/EditMaintenanceCalendar");
}

//function to show calender view of maintenance
function showCalenderViewOfMaintenance()
{
    document.getElementById("calenderViewSelect").style.backgroundColor = "#cccccc";
    document.getElementById("listViewSelect").style.backgroundColor = "#f5f5f5";
    $("#calendar").show();
    $("#listView").hide();
    if (!document.getElementById('calendar').hasChildNodes())
    {
        readyCalender();
    }

    //jquery on onready page event to enable and diable time and date picker  
    $('#datetimepickerStartTime').hover(function () {
        $('#datetimepickerStartTime').datetimepicker(datetimepickerOptions);
        if ($('#datetimepickerStartTime:hover') == false) {
            $('#datetimepickerStartTime').datetimepicker('disable');
        }
    });
    $('#datetimepickerEndTime').hover(function () {
        $('#datetimepickerEndTime').datetimepicker(datetimepickerOptions);
        if ($('#datetimepickerEndTime:hover') == false) {
            $('#datetimepickerEndTime').datetimepicker('disable');
        }
    });

    var datetimepickerOptions = {
        lang: 'cs',
        step: 30,
        dayOfWeekStart: 1,      //week begins with monday
    }

    InactivateEvents();

    
}

//function to show list view of maintenance
function showListViewOfMaintenance()
{
    document.getElementById("calenderViewSelect").style.backgroundColor = "#f5f5f5";
    document.getElementById("listViewSelect").style.backgroundColor = "#cccccc";
    var companyId = document.getElementById("companyIdStoringTextField").value;
    $("#calendar").hide();
    $("#listView").show();
    $("#listViewBody").html("");

    $.ajax({
        url: '/Admin/getExistingEvents',
        type: 'post',
        data: { pstrCompanyId: companyId },
        datatype: 'json',
        async: false,
        cache: false,
        success: function (data) {
            var newRow, i;
            for (i = 0; i < data.length; i++) {
                $.ajax({
                    url: '/Admin/jsonEventClick',
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
    });
    $("#listViewTable").DataTable(
        {
            "lengthMenu": [[10, 20, 50], [10, 20, 50]]
        });

}