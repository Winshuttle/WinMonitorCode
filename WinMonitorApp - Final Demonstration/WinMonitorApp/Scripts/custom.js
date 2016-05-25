//master page tab panel button click page loads
//pageload function when dashboard tab clicked
function masterPageDashBoardClicked(button) {
    button.blur();
    $("#masterPageContentPlace").load("CompanyDashboard/?commonCheck=CheckIfTrueErrorWin");
}

//pageload function when component tab clicked
function masterPageComponentClicked(button) {
    button.blur();
    var companyId = document.getElementById("companyIdStoringTextField").value;
    if (companyId == '') {
        alert("please select the company to proceed");
        $("#masterPageContentPlace").load("CompanyDashboard/?commonCheck=CheckIfTrueErrorWin");
    }
    else {
        $("#masterPageContentPlace").load("ComponentandStatus/?commonCheck=CheckIfTrueErrorWin&companyId=" + companyId);
    }
}

//pageload function when activate page tab clicked
function masterPageMaintenanceCalendarClicked(button) {
    button.blur();
    var companyId = document.getElementById("companyIdStoringTextField").value;
    if (companyId == '') {
        alert("please select the company to proceed");
        $("#masterPageContentPlace").load("CompanyDashboard/?commonCheck=CheckIfTrueErrorWin");
    }
    else {
        $("#masterPageContentPlace").load("EditMaintenanceCalendar/?commonCheck=CheckIfTrueErrorWin&companyId=" + companyId);
    }
}

//pageload function when regitered admin tab clicked
function masterPageRegisteredAdminClicked(button) {
    button.blur();
    $("#masterPageContentPlace").load("RegisteredAdmin/?commonCheck=CheckIfTrueErrorWin");
}

//pageload function when settings tab clicked
function masterPageSettingsClicked(button) {
    button.blur();
    $("#masterPageContentPlace").load("Settings/?commonCheck=CheckIfTrueErrorWin");
}


//Login Screen Functions
function LoginSubmit() {
    if (($('#GetUsername').val()) && ($('#GetPassword').val())) {
        
        //ajax call to check credentials
        $.ajax({
            url: 'jsonCredentialsCheck',
            type: 'post',
            async: false,
            data: { pstrUsername: $('#GetUsername').val(), pstrPassword: $('#GetPassword').val() },
            datatype: 'text',
            cache:false,
            success: function (data) {
                if (data == "true") {
                    window.sessionStorage["AdminName"] = $('#GetUsername').val();
                    window.location.replace("Master?CheckLogin=CredentialTrue&UserName=" + $('#GetUsername').val());
                }
                else {
                    $('#LoginMessage').val("Incorrect Details Entered");
                    $('#LoginMessage').css("color", "red");
                }
            },
            error: function () {
                alert("Login Function not reachable");
            }
        });

    }
    else {
        $('#LoginMessage').val("Username or Password not Entered");
        $('#LoginMessage').css("color", "red");
    }
}



//Company Dashboard functions
    //function to open add company account modal
    function addAccountCompanyDashboard() {
        fillDataCenterList();
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


    //function to fill drop down list of primary 
    function fillDataCenterList() {
        var typeArray = ['primary', 'secondary'];
        var i;
        var id;
        for (i = 0; i < typeArray.length; i++) {
            $.ajax({
                url: 'fillDataCenterListOnPage',
                type: 'get',
                async: false,
                data: { pstrDataCenterType: typeArray[i] },
                datatype: 'json',
                cache: false,
                success: function (data) {
                    var randomVariable;
                    if (i == 0)
                        id = "selectPrimary";
                    else
                        id = "selectSecondary";

                    var dropDownList = document.getElementById(id);
                    $("#" + id).empty();
                    for (randomVariable = 0; randomVariable < data.length; randomVariable++) {
                        
                        var listElement = document.createElement("option");
                        listElement.textContent = data[randomVariable].DBDataCenterName;
                        listElement.value = data[randomVariable].DBDataCenterName;
                        dropDownList.appendChild(listElement);
                    }

                },
                error: function () { alert("something went wrong in populating lists"); }
            });
        }
    }


    //function to open add data center modal
    function addNewDataCenter() {
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
        $("#addDataCenterModal").modal('show');
    }

    //function to add company details from page to data and reflecting changes in company dashboard page
    function addCompanyToDbAndView() {

        var primaryselect = $("#selectPrimary").val();
        var secondaryselect = $("#selectSecondary").val();
            if (($("#CompanyName").val()) && ($("#CompanyURL").val()) &&  primaryselect !="default" &&  secondaryselect !="default")
            {
                var inputVal = $("#CompanyURL").val();
                var characterReg = /^(http|https)?:\/\/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/;
                if (characterReg.test(inputVal)) {
                    var jsonCompanyName = $("#CompanyName").val();
                    var jsonCompanyURL = $("#CompanyURL").val();
                    var jsonPrimary = $("#selectPrimary").val();
                    var jsonSecondary = $("#selectSecondary").val();
                    $.ajax({
                        type: 'get',
                        url: 'jsonCompanyRetrieve',
                        // The key needs to match your method's input parameter (case-sensitive).
                        data: { pstrJsonCompanyName: jsonCompanyName, pstrJsonCompanyURL: jsonCompanyURL, pstrJsonPrimary: jsonPrimary, pstrJsonSecondary: jsonSecondary },
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
                else {
                    alert("Incorrect URL format");
                }
            }
            else {
                alert("CompanyName, URL not entered or Data Centers set to default");
            }
            $('#addCompanyAccountModal').on('hide.bs.modal', function () {
                $('#addCompanyAccountModal').removeClass("modal-backdrop fade in");
            });
            $("#masterPageContentPlace").load("CompanyDashboard/?commonCheck=CheckIfTrueErrorWin");
        }


    //add Data Center to Database
    function addDataCenterToDb() {
        var dataCenterName = document.getElementById("DataCenterName").value;
        var dataCenterType = document.getElementById("selectDataCenterType").value;
        if (dataCenterName != null) {
            $.ajax({
                url: 'addDataCenterToDB',
                type: 'get',
                async: false,
                data: { pstrDataCenterName: dataCenterName, pstrDataCentertype: dataCenterType },
                datatype: 'json',
                cache:false,
                success: function () {
                    //alert("data center added successfully");
                },
                error: function () {
                    alert("error occured in adding datacenter");
                }
            });
            $('#addDataCenterModal').on('hide.bs.modal', function () {
                $('#addDataCenterModal').removeClass("modal-backdrop fade in");
            });
            $("#masterPageContentPlace").load("CompanyDashboard/?commonCheck=CheckIfTrueErrorWin");
        }
        else {
            alert("Enter Data Center Name");
        }

    }

    //Company Select button using radio selection
        $('input:radio[name="radioCompanySelect"]').click(function ()
        {
            if ($('input:radio[name="radioCompanySelect"]').is(":checked"))
            {
                var tempCompany = $('input:radio[name="radioCompanySelect"]:checked').closest("tr");
                var giveCompanyName = tempCompany.find("#RazorCompanyName").text();

                tempCompany.css("background-color", "#CCFFCC");
                setTimeout(function ()
                {
                    tempCompany.css("background-color", "transparent");
                }, 2000);

                //global company id passed
                document.getElementById("companyIdStoringTextField").value = tempCompany.find("#RazorCompanyId").text();
                //     alert("Selected Company: " + giveCompanyName);
                $("#masterPageContentPlace").load("ComponentandStatus/?commonCheck=CheckIfTrueErrorWin");
            }
            else {
                alert("No Company Selected");
            }
        });

    //Table break events for CompanyDashboard Table
        $("#companytable").ready(function ()
        {
            $("#companytable").DataTable({
                    "lengthMenu": [[10, 20, 50], [10, 20, 50]]
                });
        });


//Component Page Functions
    //function to toggle state of checkboxes when state of master checkbox is changed in component page
        function checkUnCheck(source, name)
        {
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
                    if (rowCountHtmlPage <= data.length)
                    {
                        for (i = 0; i < data.length; i++)
                        {
                            newRow = '<tr style="font-size:medium; font-weight:500; color:black;"><td><input type="checkBox" name="masterComponentListfromDBCheckBoxes" value="' + data[i].DBMasterComponentName + '"></td><td>' + data[i].DBMasterComponentName + '</td><tr>';
                            $("#addMasterComponentModalTableBody").append(newRow);
                        }
                    }
                },
                error: function () { alert("error"); }
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
            var dataCenterName;
            if (companyId !=null) {
                $.ajax({
                    url: 'getCompanyDetailWithId',
                    type: 'get',
                    cache: false,
                    async: false,
                    data: { pstrCompanyId: companyId },
                    datatype: 'Json',
                    success: function (data) {
                        var primaryDataCenterOfCompany = data.DBPrimaryCenter;               
                        $.ajax({
                            url: "ComponentandStatusFromDB",
                            type: 'get',
                            async: false,
                            cache: false,
                            data: { pstrCompanyIdFromPage: companyId },
                            datatype: 'json',
                            success: function (data) {
                                var i;
                                var imageVariable;
                                for (i = 0; i < data.length; i++) {
                                    if (data[i].mstrIncidentName == null) {
                                        incidentName = "";
                                        imageVariable = "";
                                    }
                                    else {
                                        incidentName = data[i].mstrIncidentName;
                                        imageVariable = '<img src="../Content/Images/questionMark.jpg" id="' + data[i].mstrComponentId + '" style="height:20px; width:10px;" onclick="showIncidentDetails(this)" class="questionMarkForIncident"/>';
                                        
                                    }
                                    if (data[i].mstrCenterName == "None")
                                        dataCenterName = "";
                                    else
                                        if (data[i].mstrCenterName == primaryDataCenterOfCompany) {
                                            dataCenterName = "Primary:" + data[i].mstrCenterName;
                                        }
                                        else
                                            dataCenterName = "Secondary:" + data[i].mstrCenterName;

                                    newRow = '<tr style="font-size:medium; font-weight:500; color:black;"><td><input type="checkbox" value="' + data[i].mstrComponentId + '" name="ComponentListfromComponentPageCheckBoxes" class="ComponentListfromComponentPageCheckBoxesClass"></td><td>' + data[i].mstrComponentName +
                                        '</td><td>' + data[i].mstrComponentType + '</td><td>' + data[i].mstrComponentStatus + '</td><td>' + data[i].mstrCompanyName + '</td><td>' + incidentName +'  '+ imageVariable+'</td><td>'
                                         + dataCenterName + '</td></tr>';
                                    $("#existingComponentTableBody").append(newRow);

                                }
                            },
                            error: function () { alert("error") }

                        });
                    },
                    error: function () {
                        //alert("unable to load components with corresponding company");
                    }
                });
            }
            else
                alert("No Company Selected");
            
        }

    //function to show details of incident
        function showIncidentDetails(event)
        {
            var target = (window.event) ? window.event.srcElement : event.targe
            componentId = target.id;
            
            $.ajax({
                url: 'getIncidentDetails',
                type: 'get',
                data: { pstrComponentId: componentId },
                datatype: 'json',
                cache: false,
                async: false,
                success: function (data)
                {
                    document.getElementById(componentId).title = "Incident Details : " + data.DBDescription;
                },
                error:function(){alert("error occured");}
            });

        }



    //function to add selected master component to database and on component page
        function addMasterComponentToDBAndPage() {
            var masterComponentSelected = ($('input[name="masterComponentListfromDBCheckBoxes"]:checked').serialize());
            var companyId = document.getElementById("companyIdStoringTextField").value;
            var primaryDataCenterName = null;
            $.ajax({
                url: 'getCompanyDetailWithId',
                type : 'get',
                aync: false,
                data: { pstrCompanyId: companyId },
                datatype: 'json',
                cache: false,
                success: function (data) {
                    primaryDataCenterName = data.DBPrimaryCenter;
                    $.ajax({
                        url: "addMasterComponentToDB",
                        type: 'get',
                        async: false,
                        datatype: 'json',
                        cache: false,
                        data: { pstrMasterComponentListFromPage: masterComponentSelected, pstrCompanyId: companyId, pstrPrimaryDataCenterName: primaryDataCenterName },
                        success: function (data) {
                            //alert(data);
                        },
                        error: function () { alert("error"); }
                    });
                    $("#masterPageContentPlace").load("ComponentandStatus/?commonCheck=CheckIfTrueErrorWin");
                },
                error: function () {
                    alert("unable to fetch company Details to add components");
                }
            });
            
            $('input[name="masterComponentListfromDBCheckBoxes"]:checked').closest("tr").css("background-color", "#CCFFCC");
            
            $('#AddMasterComponentModal').on('hide.bs.modal', function () {
                $('#AddMasterComponentModal').removeClass("modal-backdrop fade in");
            });
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
            $("#masterPageContentPlace").load("ComponentandStatus/?commonCheck=CheckIfTrueErrorWin");
        });


    //function to set datacenter to selected components
        function updateDataCenter(dataCenterSelected)
        {
           
            var componentCheckedToChangeDataCenter = $('input[name="ComponentListfromComponentPageCheckBoxes"]:checked').serialize();
            var companyId = document.getElementById("companyIdStoringTextField").value;
            $.ajax({
                url: 'changeDataCenter',
                type:'get',
                cache:false,
                async: false,
                data: { pstrListOfCheckBoxes: componentCheckedToChangeDataCenter, pstrCompanyId: companyId, pstrDataCenterSelected: dataCenterSelected },
                datatype: 'json',
                success: function () {
                    //alert("succefully data center changed");
                },
                error:function(){alert("error in changing data center");}
            });
        }

    //function to open create incident modal
        function openCreateIncidentModalFunction(id) {
            document.getElementById("callSourceStoringField").value = id;
            var statusSelected = null;
            if (id == "liServiceDisruptionStatus")
                statusSelected = "Service Disruption";
            else
                statusSelected = "Performance Degradation";

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
                var primaryDataCenterName;
                $.ajax({
                    url: 'getCompanyDetailWithId',
                    type : 'get',
                    aync: false,
                    data: { pstrCompanyId: companyId },
                    datatype: 'json',
                    cache: false,
                    success: function (data) {
                        primaryDataCenterName = data.DBPrimaryCenter;
                        var jsonSpecificComponents = { "jsonSpecificComponentName": $("#formComponentName").val(), "jsonSpecificComponentStatus": "Operational", "jsonSpecificComponentCompanyId": companyId, "jsonSpecificComponentDataCenter": primaryDataCenterName };
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
                        $("#masterPageContentPlace").load("ComponentandStatus/?commonCheck=CheckIfTrueErrorWin");
                    },
                    error: function () {
                        alert("unable to fetch company Details to add components");
                    }
                });
                $('#AddSpecificComponentModal').on('hide.bs.modal', function () {
                    $('#AddSpecificComponentModal').removeClass("modal-backdrop fade in");
                });
            }
            else {
                alert("Enter Specific ComponentName");
            }
        });

    //JQuery to add and check new incident in the incident table and JQuery to check fields in minor and major event modal
        function addIncidentForSelectedComponents() {
            var companyId = document.getElementById("companyIdStoringTextField").value;
            var callSource = document.getElementById("callSourceStoringField").value;
            var componentCheckedToRaiseIncident = [];
            $.each($("input[name='ComponentListfromComponentPageCheckBoxes']:checked"), function () {
                componentCheckedToRaiseIncident.push($(this).val());
            });
            if (($("#formIncidentName").val()) && ($("#formIncidentDetails").val())) {
                var dataCenterSelectedId = null;
                if (callSource == 'liSecondaryCompanyCenter') {
                    var gotvalue = document.getElementById(callSource).textContent;
                    if (gotvalue.indexOf("Primary:") >= 0) {
                        gotvalue = gotvalue.replace('Primary:', '');
                    }
                    else {
                        gotvalue = gotvalue.replace('Secondary:', '');
                    }
                    dataCenterSelectedId = callSource;
                    updateDataCenter(gotvalue);

                }
                var randomVar;
                var companyId = document.getElementById("companyIdStoringTextField").value;
                        for (randomVar = 0; randomVar < componentCheckedToRaiseIncident.length; randomVar++) {
                            $.ajax({
                                url:'checkIfIncidentExists',
                                type:'get',
                                data: { pstrComponentId: componentCheckedToRaiseIncident[randomVar] },
                                datatype: 'text',
                                cache: false,
                                async: false,
                                success: function (data) {
                                    if (data == "true") {
                                        $.ajax({
                                            url: 'deleteExistingIncidentOfComponent',
                                            type: 'get',
                                            data: { pstrComponentId: componentCheckedToRaiseIncident[randomVar] , pstrCompanyId: companyId},
                                            datatype: 'Json',
                                            cache: false,
                                            async: false,
                                            success: function () {
                                                //alert("successfully deleted incident");
                                            },
                                            error: function () { alert("error in deletion of incident");}
                                        })
                                    }
                                },
                                error: function () {

                                }
                            });

                        }
                updateStatusOfSelectedComponents();
                
                

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
                $("#masterPageContentPlace").load("ComponentandStatus/?commonCheck=CheckIfTrueErrorWin");

            }
            else {
                alert("Enter IncidentName and IncidentDetails");
            }
        }


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
            $("#masterPageContentPlace").load("ComponentandStatus/?commonCheck=CheckIfTrueErrorWin");
        }

    //Table break events for Components and Status Table
        $("#existingComponentTable").ready(function () {
            $("#existingComponentTable").DataTable(
                {
                    "lengthMenu": [[10, 20, 50], [10, 20, 50]]
                });

        });


    //function to populate dropdown list with primary and secondary datacenter of selected company
        function populateDataCenterOnComponentPage()
        {
            var companyId = document.getElementById("companyIdStoringTextField").value;
            if (companyId != null) {
                $.ajax({
                    url: 'getCompanyDetailWithId',
                    type: 'get',
                    async: false,
                    data: { pstrCompanyId: companyId },
                    datatype: 'json',
                    cache: false,
                    success: function (data) {
                        var primaryCenter = data.DBPrimaryCenter;
                        var secondaryCenter = data.DBSecondaryCenter;
                        $('<li role="presentation" ><a role="menuitem" tabindex="-1" id="liPrimaryCompanyCenter" ><span>Primary:</span>' + primaryCenter + '</a></li>').appendTo("#dataCenterSelectDiv .dropdown ul");
                        $('<li role="presentation" ><a role="menuitem" tabindex="-1" id="liSecondaryCompanyCenter" onclick="openCreateIncidentModalFunction(this.id)"><span>Secondary:</span>' + secondaryCenter + '</a></li>').appendTo("#dataCenterSelectDiv .dropdown ul");
                    },
                    error: function () {
                        //alert("unable to load primary or secondary centers");
                    }
                });
            }
        }

    //function to change data center to primary data center of components selected
        $("#liPrimaryCompanyCenter").click(function () {
            var componentCheckedToUpdateDataCenter = $('input[name="ComponentListfromComponentPageCheckBoxes"]:checked').serialize();
            var companyId = document.getElementById("companyIdStoringTextField").value;
            $.ajax({
                url: 'getCompanyDetailWithId',
                type:'get',
                datatype: 'json',
                data: { pstrCompanyId: companyId },
                cache: false,
                async: false,
                success: function (data) {
                    var primaryDataCenter = data.DBPrimaryCenter;
                    $.ajax({
                        url: 'setPrimaryDataCenter',
                        type: 'get',
                        data: { pstrCompanyId: companyId, pstrComponentsSelected: componentCheckedToUpdateDataCenter, pstrSelectedDataCenter: primaryDataCenter },
                        async: false,
                        cache: false,
                        datatype: 'json',
                        success: function () {
                            //alert("successfully changed To primary");
                        },
                        error: function () {
                            alert("unable to change To primary");
                        }
                    });
                    $("#masterPageContentPlace").load("ComponentandStatus/?commonCheck=CheckIfTrueErrorWin");

                },
                error: function () {
                    alert("error in fetching data");
                }
            });
        });
        


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

    //JQuery for check password validation modal
    $("#btnChangePassword").click(function () {
        if (($("#OldPassword").val()) && ($("#NewPassword").val()) && ($("#ConfirmNewPassword").val())) {
            if ($("#NewPassword").val().length >= 8) {
                if ($("#NewPassword").val() == $("#ConfirmNewPassword").val()) {

                    //ajax call for changing the password/settings
                    $.ajax({
                        url: 'jsonChangeSettings',
                        type: 'post',
                        datatype: 'text',
                        data: { pstrUsername:$("#adminNameHiddenField").val(), pstrOldPassword: $("#OldPassword").val(), pstrNewPassword: $("#NewPassword").val() },
                        cache: false,
                        async: false,
                        success: function (data) {
                            alert(data);
                        },
                        error: function () {
                            alert("Unable to Change Password in DB");
                        }
                    });

                }
                else {

                    alert("New Password and Confirm New Password Not Match!");
                }
            }
            else {
                alert("Length of password should be 8 characters");
            }
        }
        else {
            alert("One of the field values not entered");
        }
    });


//Registered Page Functions
    //Table break events for Registered Administrators Table
    $("#RegisteredAdminTable").ready(function () {
        $("#RegisteredAdminTable").DataTable(
            {
                "lengthMenu": [[10, 20, 50], [10, 20, 50]]
            });
    });

    //Logout Function
    $("#btnLogout").click(function () {
        window.sessionStorage["AdminName"] = '';
        window.location.replace("LoginScreen");
    });