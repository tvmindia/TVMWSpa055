﻿var DataTables = {};
var startdate = '';
var enddate = '';
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {
        $("#CompanyCode,#SupplierCode").select2({
        });

        DataTables.purchaseDetailReportTable = $('#PurchaseDetailTable').DataTable(
         {
             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                columns: [0, 1, 2, 3, 4, 5, 6,7,8,9,11]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetPurchaseDetail(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [

               { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
               { "data": "SupplierName", "defaultContent": "<i>-</i>" },
                { "data": "Date", "defaultContent": "<i>-</i>", "width": "10%" },
                { "data": "PaymentDueDate", "defaultContent": "<i>-</i>", "width": "10%" },
                
               { "data": "InvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "PaidAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "PaymentProcessed", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "BalanceDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "Credit", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                { "data": "GeneralNotes", "defaultContent": "<i></i>" },
             { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
             { "data": "Origin", "defaultContent": "<i>-</i>" }

             ],
             columnDefs: [{ "targets": [10, 8], "visible": false, "searchable": false },
                   { className: "text-left", "targets": [0, 1] },
                   { className: "text-center", "targets": [ 2, 3] },
                  { className: "text-right", "targets": [4, 5, 6,7,9] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(10, { page: 'current' }).data().each(function (group, i) {
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="8" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         last = group;
                     }
                 });
             }
         });

        $(".buttons-excel").hide();
        startdate = $("#todate").val();
        enddate = $("#fromdate").val();
        //$('input[name="GroupSelect"]').on('change', function () {
        //    RefreshPurchaseDetailTable();
        //});
        $("#purchasedetailtotals").attr('style', 'visibility:true');

    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetPurchaseDetail() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var search = $("#Search").val();
        $('#IncludeInternal').attr('checked', false);
        $('#IncludeTax').attr('checked', true);
        var supplier = $("#SupplierCode").val();
        if (supplier == 'ALL') {
            supplier = emptyGUID;
        }
        else {
            supplier = $("#SupplierCode").val();
        }
        var internal;
        if ($('#IncludeInternal').prop("checked") == true) {
            internal = true;
        }
        else {
            internal = false;
        }
        //if (companycode === "ALL") {
        //    if ($("#all").prop('checked')) {
        //        companycode = $("#all").val();
        //    }
        //    else {
        //        companycode = $("#companywise").val();
        //    }
        //}
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "search": search, "IsInternal": internal,"Supplier":supplier };
            var ds = {};
            ds = GetDataFromServer("Report/GetPurchaseDetails/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.TotalAmount != '') {
                $("#purchasedetailsamount").text(ds.TotalAmount);
            }
            if (ds.InvoicedAmount != '') {
                $("#purchasedetailsinvoiceamount").text(ds.InvoicedAmount);
            }
            if (ds.PaidAmount != '') {
                $("#purchasedetailspaidamount").text(ds.PaidAmount);
            }
            if (ds.PaymentProcessed != '') {
                $("#purchasedetailspaymentprocessed").text(ds.PaymentProcessed);
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


function RefreshPurchaseDetailTable() {
    try {
        debugger;
        var IsInternalCompany = $('#IncludeInternal').prop('checked');
        var IsTax = $('#IncludeTax').prop('checked');
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        //if (companycode === "") {
        //    return false;
        //}

        //if (companycode === "ALL") {
        //    $("#all").prop("disabled", false);
        //    $("#companywise").prop("disabled", false);
        //}
        //else {
        //    $("#all").prop("disabled", true);
        //    $("#companywise").prop("disabled", true);

        //}
        if (DataTables.purchaseDetailReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.purchaseDetailReportTable.clear().rows.add(GetPurchaseDetail()).draw(false);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
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

function Reset() {
    debugger;
    $("#todate").val(startdate);
    $("#fromdate").val(enddate);
    $("#CompanyCode").val('ALL').trigger('change');
    $("#Search").val('');
    $("#all").prop('checked', true).trigger('change');
    $("#SupplierCode").val('ALL').trigger('change');;
}


function OnChangeCall() {
    debugger;
    RefreshPurchaseDetailTable();

}