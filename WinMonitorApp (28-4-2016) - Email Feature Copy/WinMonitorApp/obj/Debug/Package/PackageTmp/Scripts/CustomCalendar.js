$(document).ready(function () {

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
                events: 'getExistingEvents?pstrCompanyId=' + companyId,
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
                },

                eventDrop: function (event, delta, revertFunc) {
                    updateCalenderInDB(event.id, event.start, event.end);
                }
            });
        }
        
    });
});
//function to update calender in DB
    function updateCalenderInDB(eventId, startTime, endTime)
    {
        $.ajax({
            url: 'UpdateCalenderInDB',
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
            url: 'InactivateEvents',
            async: false,
            type: 'get',
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
    var Repetition = $("#selectCronString option:selected").val();
    var Maintenance = $("#selectTypeMaintenance option:selected").val();
    var companyId = document.getElementById("companyIdStoringTextField").value;

    $.ajax({
        url: 'jsonAddDayClickEvents',
        type: 'POST',
        data: { jsonpstrTitle: Title, jsonpstrDetails: Details, jsonpstrStartTime: StartTime, jsonpstrEndTime: EndTime, jsonpstrRepetition: Repetition, jsonpstrMaintenance: Maintenance, jsonpstrCompanyId:companyId },
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
    $("#masterPageContentPlace").load("EditMaintenanceCalendar");
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
        url: 'jsonEventClick',
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
            $("#selectCronStringShow").val(data[0].Repetition);
            
            //conditions for Maintenance 
            if (data[0].Maintenance == "Maintenance") {
                $("#selectTypeMaintenanceShow").val("Maintenance");
                $("#selectTypeMaintenanceShow").css("color", "green");
            }
            else {
                if (data[0].Maintenance == "Performance Degradation") {
                    $("#selectTypeMaintenanceShow").val("Performance Degradation");
                    $("#selectTypeMaintenanceShow").css("color", "orange");
                }
                else {
                    $("#selectTypeMaintenanceShow").val("Service Disruption");
                    $("#selectTypeMaintenanceShow").css("color", "red");
                }

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
        url: 'jsonEventClickDelete',
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
    $("#masterPageContentPlace").load("EditMaintenanceCalendar");
}