//master page tab panel button click page loads
//pageload function when dashboard tab clicked
function masterPageDashBoardClicked() {
    $("#masterPageContentPlace").load("CompanyDashboard");
}

//pageload function when component tab clicked
function masterPageComponentClicked() {
    $("#masterPageContentPlace").load("ComponentandStatus");
}

//pageload function when Metrics tab clicked
function masterPageMetricsClicked() {
    $("#masterPageContentPlace").load("PerformanceMetrics");
}

//pageload function when activate page tab clicked
function masterPageAcivatePageClicked() {
    $("#masterPageContentPlace").load("ActivatePage");
}

//pageload function when regitered admin tab clicked
function masterPageRegisteredAdminClicked() {
    $("#masterPageContentPlace").load("RegisteredAdmin");
}

//pageload function when settings tab clicked
function masterPageSettingsClicked() {
    $("#masterPageContentPlace").load("Settings");
}


//Company Dashboard functions
//function to open add company account modal
function addAccountCompanyDashboard() {
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
    $("#addCompanyAccountModal").modal('show');
}


//Setting Page Functions
//function to open change password modal
function changePasswordSettings() {
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
    $("#changePasswordModal").modal('show');
}


//Component Page Functions
//function to toggle state of checkboxes when state of master checkbox is changed in component page
function checkUnCheck(source, name) {
    checkboxes = document.getElementsByName(name);
    var noOfCheckBoxes = checkboxes.length;
    var i;
    for (i = 0; i < noOfCheckBoxes; i++)
        checkboxes[i].checked = source.checked;
}




//function to open add master component modal
function openAddMasterComponentModalFuntion() {

    $.ajax({                                                                    //to add master components from database to the modal
        url: 'getMasterComponentDataFromDataBase',
        type: 'get',
        async: false,
        datatype: 'json',
        cache:false,
        success: function (data) {
            var newRow, i, eachRow, rowCountHtmlPage;
            rowCountHtmlPage = document.getElementById("addMasterComponentModalTable").rows.length;
            if (rowCountHtmlPage <= data.length) {
                for (i = 0; i < data.length; i++) {
                    newRow = '<tr style="font-size:medium; font-weight:500; color:black;"><td><input type="checkBox" name="masterComponentListfromDBCheckBoxes" value="' + data[i].DBMasterComponentName + '"></td><td>' + data[i].DBMasterComponentName + '</td><tr>';
                    $("#addMasterComponentModalTableBody").append(newRow);
                }
            }
        },
        error: function () {
            alert("error");
        }
    });

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
    $("#AddMasterComponentModal").modal('show');
}


//function to load existing components from database to web page
function loadComponentsFromDB() {
    var companyId = document.getElementById("companyIdStoringTextField").value;
    var incidentName;
    $.ajax({
        url: "ComponentandStatusFromDB",
        type: 'get',
        async: false,
        cache: false,
        data: { pstrCompanyIdFromPage: companyId },
        datatype: 'json',
        success: function (data) {
            var i;
            for (i = 0; i < data.length; i++) {
                if (data[i].mstrIncidentName == null) {
                    incidentName = "";
                }
                else
                    incidentName = data[i].mstrIncidentName;
                newRow = '<tr style="font-size:medium; font-weight:500; color:black;"><td><input type="checkbox" value="' + data[i].mstrComponentId + '" name="ComponentListfromComponentPageCheckBoxes"></td><td>' + data[i].mstrComponentName +
                    '</td><td>' + data[i].mstrComponentType + '</td><td>' + data[i].mstrComponentStatus + '</td><td>' + data[i].mstrCompanyName + '</td><td>' + incidentName + '</td></tr>';
                $("#existingComponentTableBody").append(newRow);
            }
        },
        error: function () { alert("error") }

    });
}


//function to add selected master component to database and on component page
function addMasterComponentToDBAndPage() {
    var masterComponentSelected = ($('input[name="masterComponentListfromDBCheckBoxes"]:checked').serialize());
    var companyId = document.getElementById("companyIdStoringTextField").value;

    $('input[name="masterComponentListfromDBCheckBoxes"]:checked').closest("tr").css("background-color", "#CCFFCC");
    $.ajax({
        url: "addMasterComponentToDB",
        type: 'get',
        async: false,
        datatype: 'json',
        cache: false,
        data: { pstrMasterComponentListFromPage: masterComponentSelected, pstrCompanyId: companyId },
        success: function (data) {
            //alert(data);
        },
        error: function () { alert("error"); }
    });
    $('#AddMasterComponentModal').on('hide.bs.modal', function () {
        $('#AddMasterComponentModal').removeClass("modal-backdrop fade in");
    });
    $("#masterPageContentPlace").load("ComponentandStatus");


}

//function to open add specific component modal
function openAddSpecificComponentModalFuntion() {
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
    $("#AddSpecificComponentModal").modal('show');
}

//Query to set incident status as operational
$("#liOperationalStatus").click(function () {
    document.getElementById("statusStoringfeild").value = "Operational";
    updateStatusOfSelectedComponents();
    $("#masterPageContentPlace").load("ComponentandStatus");
});

//function to open create incident modal
function openCreateIncidentModalFunction() {
    var statusSelected;
    if (event.target.id == "liMajorOutageStatus")
        statusSelected = "Major";
    else
        statusSelected = "Minor";

    document.getElementById("statusStoringfeild").value = statusSelected;

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
    $("#raiseIncidentModal").modal('show');
}


//function to add company details from page to data and reflecting changes in company dashboard page
function addCompanyToDbAndView() {

    if (($("#CompanyName").val()) && ($("#CompanyURL").val()) ) {
        
        var inputVal = $("#CompanyURL").val();
        var characterReg = /^(http|https)?:\/\/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/;
        
        if(characterReg.test(inputVal))
        {
            var jsonCompanyName = $("#CompanyName").val();
            var jsonCompanyURL = $("#CompanyURL").val();

        $.ajax({
            type: 'get',
            url: 'jsonCompanyRetrieve',
            // The key needs to match your method's input parameter (case-sensitive).
            data: { pstrJsonCompanyName: jsonCompanyName, pstrJsonCompanyURL: jsonCompanyURL },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (data) {
                //              alert(data);
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
        }
        else
        {
            alert("Incorrect URL format");
        }
        $('#addCompanyAccountModal').on('hide.bs.modal', function () {
            $('#addCompanyAccountModal').removeClass("modal-backdrop fade in");
        });
        $("#masterPageContentPlace").load("CompanyDashboard");
    }
    else {
        alert("Enter CompanyName and CompanyURL");
    }
}


//Company Select button using radio selection
$('input:radio[name="radioCompanySelect"]').click(function () {
    if ($('input:radio[name="radioCompanySelect"]').is(":checked")) {
        var tempCompany = $('input:radio[name="radioCompanySelect"]:checked').closest("tr");
        var giveCompanyName = tempCompany.find("#RazorCompanyName").text();

        tempCompany.css("background-color", "#CCFFCC");
        setTimeout(function () {
            tempCompany.css("background-color", "transparent");
        }, 2000);

        //global company id passed
        document.getElementById("companyIdStoringTextField").value = tempCompany.find("#RazorCompanyId").text();
  //     alert("Selected Company: " + giveCompanyName);
        
    }
    else {
        alert("No Company Selected");
    }
});

//function to update Status of selected Components
function updateStatusOfSelectedComponents() {
    var statusOfComponentsSelected = document.getElementById("statusStoringfeild").value;
    var componentCheckedToUpdateStatus = $('input[name="ComponentListfromComponentPageCheckBoxes"]:checked').serialize();
    var companyId = document.getElementById("companyIdStoringTextField").value;

    $('input[name="ComponentListfromComponentPageCheckBoxes"]:checked').closest("tr").css("background-color", "#CCFFCC");

    $.ajax({
        url: "updateStatusOfSelectedComponents",
        type: 'get',
        async: false,
        cache: false,
        data: { pstrListOfCheckBoxes: componentCheckedToUpdateStatus, pstrStatusSelected: statusOfComponentsSelected, pstrCompanyId: companyId },
        datatype: 'json',
        success: function () {
   //         alert("status succesfully updated");

        },
        error: function () { alert("error"); }
    });
}



//JQuery to check specific components modal and to  add specific component(new row) in existing component table on component page  
$("#btnaddSpecificComponent").click(function () {

    if (($("#formComponentName").val())) {
        var companyId = document.getElementById("companyIdStoringTextField").value;
        var jsonSpecificComponents = { "jsonSpecificComponentName": $("#formComponentName").val(), "jsonSpecificComponentStatus": "Operational", "jsonSpecificComponentCompanyId": companyId };

        $.ajax({
            type: "POST",
            url: "/Admin/jsonSpecificComponentRetrieve",
            // The key needs to match your method's input parameter (case-sensitive).
            data: JSON.stringify({ jsonSpecificComponentServers: jsonSpecificComponents }),
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            async: false,
            cache: false,
            success: function (data) {
//              alert(data);
            },
            failure: function () {
                alert("error");
            }
        });
        $('#AddSpecificComponentModal').on('hide.bs.modal', function () {
            $('#AddSpecificComponentModal').removeClass("modal-backdrop fade in");
        });
        $("#masterPageContentPlace").load("ComponentandStatus");

    }
    else {
        alert("Enter Specific ComponentName");
    }
});


//JQuery to add and check new incident in the incident table and JQuery to check fields in minor and major event modal
$("#btnaddIncident").click(function () {
    updateStatusOfSelectedComponents();
    var companyId = document.getElementById("companyIdStoringTextField").value;
    if (($("#formIncidentName").val()) && ($("#formIncidentDetails").val())) {
        var componentCheckedToRaiseIncident = [];
        $.each($("input[name='ComponentListfromComponentPageCheckBoxes']:checked"), function () {
            componentCheckedToRaiseIncident.push($(this).val());
        });
        var jsonIncidents = { "jsonIncidentName": $("#formIncidentName").val(), "jsonIncidentDetails": $("#formIncidentDetails").val(), "jsonComponentIdList": componentCheckedToRaiseIncident, "jsonCompanyID": companyId };

        $.ajax({
            type: "POST",
            url: "jsonIncidentRetreive",
            // The key needs to match your method's input parameter (case-sensitive).
            data: JSON.stringify({ jsonIncidentServers: jsonIncidents }),
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            async: false,
            cache: false,
            success: function (data) {
//                alert(data);
            },
            error: function () {
                alert("error");
            }
        });
        $('#raiseIncidentModal').on('hide.bs.modal', function () {
            $('#raiseIncidentModal').removeClass("modal-backdrop fade in");
        });
        $("#masterPageContentPlace").load("ComponentandStatus");

    }
    else {
        alert("Enter IncidentName and IncidentDetails");
    }
});


//function to delete slected components
function deleteSelectedComponents() {
    var componentCheckedToDelete = $('input[name="ComponentListfromComponentPageCheckBoxes"]:checked').serialize();
    var companyId = document.getElementById("companyIdStoringTextField").value;
    $.ajax({
        url: 'deleteSelectedComponents',
        type: 'get',
        async: false,
        cache: false,
        data: { pComponentList: componentCheckedToDelete, pCompanyId: companyId },
        datatype: 'json',
   //     success: function () { alert("successfully deleted"); },
        error: function () { alert("error in deletion"); }
    });
    $("#masterPageContentPlace").load("ComponentandStatus");
}


//JQuery for check password validation modal
$("#btnChangePassword").click(function () {
    if ($("#NewPassword").val() == $("#ConfirmNewPassword").val()) {


    }
    else {

        alert("New Password and Confirm New Password Not Match!");
    }
});


//Table break events for CompanyDashboard Table
$("#companytable").ready(function () {
    $("#companytable").DataTable(
        {
            "lengthMenu": [[10, 20, 50], [10, 20, 50]]
        });
});




//Table break events for Components and Status Table
$("#existingComponentTable").ready(function () {
    $("#existingComponentTable").DataTable(
        {
            "lengthMenu": [[10, 20, 50], [10, 20, 50]]
        });

});


//Table break events for Registered Administrators Table
$("#RegisteredAdminTable").ready(function () {
    $("#RegisteredAdminTable").DataTable(
        {
            "lengthMenu": [[10, 20, 50], [10, 20, 50]]
        });
});

//Logout Function
$("#btnLogout").click(function () {
    window.top.close();
});