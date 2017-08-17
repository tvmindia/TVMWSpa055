﻿var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.ChartOfAccounts = $('#ChartOfAccountsTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllChartOfAccounts(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Code", "defaultContent": "<i>-</i>" },
               { "data": "Type", "defaultContent": "<i>-</i>" },
                { "data": "TypeDesc", "defaultContent": "<i>-</i>" },
               { "data": "ISEmploy", "defaultContent": "<i>-</i>" },
                { "data": "IsReverse", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0, 1, 2, 3, 4] },
             { className: "text-center", "targets": [5] },
               {
                   "render": function (data, type, row) {
                       return (data == false ? "No " : "Yes");
                   },
                   "targets": 3

               },
                  {
                      "render": function (data, type, row) {
                          return (data == false ? "No " : "Yes");
                      },
                      "targets": 4

                  },
                    {
                        "render": function (data, type, row) {
                            return (data == "OE" ? "Other Expense" : "Other Income");
                        },
                        "targets": 1

                    }

             ]
         });

        $('#ChartOfAccountsTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });






    } catch (x) {

        notyAlert('error', x.message);

    }

});


//---------------------------------------Edit ChartOfAccounts--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    openNav("0");
    ResetForm();


    var rowData = DataTables.ChartOfAccounts.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.Code != null)) {
        FillChartOfAccountDetails(rowData.Code);
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
    }
}

function goBack() {
    ClearFields();
    closeNav();
    BindAllChartOfAccounts();
}

//---------------------------------------Fill Chart Of Account Details--------------------------------------------------//
function FillChartOfAccountDetails(Code) {
    debugger;
    ChangeButtonPatchView("ChartOfAccounts", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetChartOfAccountDetailsByCode(Code); //Binding Data
    //Hidden
    debugger;
    $("#Code").val(thisItem.Code);
    $("#Type").val(thisItem.Type);
    $("#Type").prop('disabled', true);
    $("#TypeDesc").val(thisItem.TypeDesc);
    if (thisItem.ISEmploy == true)
    {
        $('#ISEmploy').attr('checked', true);
    }
    else
    {
        $('#ISEmploy').attr('checked', false);
    }
    if (thisItem.IsReverse == true) {
        $('#IsReverse').attr('checked', true);
    }
    else {
        $('#IsReverse').attr('checked', false);
    }
    $("#isUpdate").val("1");
    $("#Code").prop('disabled', true);
    $("#hdnType").val(thisItem.Type);
    $("#hdnCode").val(thisItem.Code);
}

function GetChartOfAccountDetailsByCode(Code) {
    try {

        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("ChartOfAccounts/GetChartOfAccountDetails/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function Reset() {
    if ($("#isUpdate").val() == "0") {
        ClearFields();
    }
    else {
        FillChartOfAccountDetails($("#hdnCode").val());
    }
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#ChartOfAccountsForm").validate();
    $('#ChartOfAccountsForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}


function GetAllChartOfAccounts() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("ChartOfAccounts/GetAllChartOfAccounts/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function ClearFields() {
    debugger;
    $("#isUpdate").val("0");
    $('#ISEmploy').prop('checked', false);
    $('#IsReverse').prop('checked', false);
    $("#Code").val("");
    $("#Type").val("");
    $("#TypeDesc").val("");
    $("#Code").prop('disabled', false);
    $("#Type").prop('disabled', false);
    $("#hdnCode").val("");
    $("#hdnType").val("");
    ResetForm();
    ChangeButtonPatchView("ChartOfAccounts", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}


function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllChartOfAccounts();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#hdnCode").val() != "") {
                FillChartOfAccountDetails($("#hdnCode").val());
            }
            else {
                FillChartOfAccountDetails(JsonResult.Records.Code);
            }
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}

function BindAllChartOfAccounts() {
    try {
        DataTables.ChartOfAccounts.clear().rows.add(GetAllChartOfAccounts()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Save() {
    try {
        $("#btnInsertUpdateChartOfAccounts").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteChartOfAccounts()', '', "Yes, delete it!");
}

function DeleteChartOfAccounts() {
    try {
        var code = $('#hdnCode').val();
        if (code != '' && code != null) {
            var data = { "code": code };
            var ds = {};
            ds = GetDataFromServer("ChartOfAccounts/DeleteChartOfAccounts/", data);
            debugger;
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