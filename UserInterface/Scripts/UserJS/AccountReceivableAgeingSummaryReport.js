﻿var DataTables = {};
$(document).ready(function () {


    try {
        $("#customerCode").select2({
            placeholder: "Select a Customers..",

        });

        DataTables.ReceivableAgeingSummaryReportTable = $('#ReceivableAgeingSummaryTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: true,
             paging: true,
             data: GetReceivableAgeingSummaryReport(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

         
              
               { "data": "Customer", "defaultContent": "<i>-</i>" },
               { "data": "Current", "defaultContent": "<i>-</i>" },
               { "data": "OneToThirty", "defaultContent": "<i>-</i>" },
               { "data": "ThirtyOneToSixty", "defaultContent": "<i>-</i>" },
               { "data": "SixtyOneToNinety", "defaultContent": "<i>-</i>" },
               { "data": "NinetyOneAndOver", "defaultContent": "<i>-</i>" }
              
             ],
             columnDefs: [
             { className: "text-left", "targets": [0] },
             { className: "text-center", "targets": [1,2,3,4,5] }]
            
         });

        $(".buttons-excel").hide();
        $("#ddlCustomer").attr('style', 'visibility:true');

    } catch (x) {
        notyAlert('error', x.message);
    }
});


function GetReceivableAgeingSummaryReport() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var customerids = $("#customerCode").val();
        var invoicetype = $("#ddlInvoiceTypes").val();
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "Customerids": customerids, "InvoiceType": invoicetype };
            var ds = {};
            ds = GetDataFromServerTraditional("Report/GetAccountsReceivableAgeingSummary/", data);
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



    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function RefreshReceivableAgeingReportTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        if (DataTables.ReceivableAgeingSummaryReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.ReceivableAgeingSummaryReportTable.clear().rows.add(GetReceivableAgeingSummaryReport()).draw(true);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Reset()
{
    $("#todate").val(today);
    $("#fromdate").val(fromday);
    $("#CompanyCode").val('ALL');
    $("#ddlInvoiceTypes").val('RB');
    $("#customerCode").val('').trigger('change');
    RefreshReceivableAgeingReportTable();
}

function PrintReport() {
    try {

        $(".buttons-excel").trigger('click');


    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Back() {
    window.location = appAddress + "Report/Index/";
}




