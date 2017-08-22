﻿var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.DepartmentTable = $('#DepartmentTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllDepartments(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Code", "defaultContent": "<i>-</i>" },
               { "data": "Name", "defaultContent": "<i>-</i>" },
               { "data": "commonObj.CreatedDateString", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit OtherIncome" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [

               { className: "text-left", "targets": [0,1] },
             { className: "text-center", "targets": [2] }

             ]
         });

        $('#DepartmentTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });



    } catch (x) {

        notyAlert('error', x.message);

    }

});

function GetAllDepartments() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Department/GetAllDepartments/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
           
            notyAlert('error', ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function openNav(id) {
    var left = $(".main-sidebar").width();
    var total = $(document).width();

    $('.main').fadeOut();
    document.getElementById("myNav").style.left = "3%";
    $('#main').fadeOut();

    if ($("body").hasClass("sidebar-collapse")) {

    }
    else {
        $(".sidebar-toggle").trigger("click");
    }
    if (id != "0") {
        ClearFields();
        $("#Operation").val('Insert');
    }
   
}

function goBack() {
    ClearFields();
    closeNav();
    BindAllDepartments();
}

function Save() {

    try {
        //    $('#EmployeeType').val("EMP");
        $("#btnInsertUpdateDepartment").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
       
    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteDepartment()', '', "Yes, delete it!");
}

function DeleteDepartment() {
    try {
        var id = $('#Code').val();
        if (id != '' && id != null) {
            var data = { "Code": id };
            var ds = {};
            ds = GetDataFromServer("Department/DeleteDepartment/", data);
         
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message.Message);
                goBack();
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function ClearFields() {
   
    $("#Code").val("");
    $("#Name").val("");
    $("#Code").prop("readonly", false);
   
    ResetForm();
    ChangeButtonPatchView("Department", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#DepartmentForm").validate();
    $('#DepartmentForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function BindAllDepartments() {
    try {
        DataTables.DepartmentTable.clear().rows.add(GetAllDepartments()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllDepartments();
            $("#Operation").val('Update');
            $("#Code").prop("readonly", true);
            notyAlert('success', JsonResult.Record.Message);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}

function Reset() {
    if ($("#Code").val() == "") {
        ClearFields();
    }
    else {
        FillDepartmentDetails($("#Code").val());
    }
    ResetForm();
}

function GetDepartmentByID(Code) {
    try {

        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("Department/GetDepartmentDetails/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
           
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//---------------------------------------Fill Customer Details--------------------------------------------------//
function FillDepartmentDetails(Code) {
   
    ChangeButtonPatchView("Department", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetDepartmentByID(Code); //Binding Data
    //Hidden
    $("#Code").val(thisItem.Code);
    $("#Name").val(thisItem.Name);
    
}

//---------------------------------------Edit Bank--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
   
    openNav("0");
    ResetForm();
    $("#Code").prop("readonly", true);
    $("#Operation").val('Update');
    var rowData = DataTables.DepartmentTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.Code != null)) {
        FillDepartmentDetails(rowData.Code);
    }
}