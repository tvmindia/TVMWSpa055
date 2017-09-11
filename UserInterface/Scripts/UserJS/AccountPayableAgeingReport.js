﻿var DataTables = {};
$(document).ready(function () {


    try {

        DataTables.PayableAgeingReportTable = $('#PayableAgeingTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetPayableAgeingReport(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

               { "data": "TransactionDate", "defaultContent": "<i>-</i>" },
               { "data": "DocNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "DueDate", "defaultContent": "<i>-</i>" },
               { "data": "DaysPastDue", "defaultContent": "<i>-</i>" },
               { "data": "Invoiced", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Paid", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Balance", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "OriginatedCompany", "defaultContent": "<i>-</i>" },
               { "data": "Group", "defaultContent": "<i>-</i>" }
             ],
             columnDefs: [{ "targets": [9], "visible": false, "searchable": false },
             { className: "text-left", "targets": [0, 1, 2, 3, 8] },
             { className: "text-right", "targets": [5, 6, 7] },
             { className: "text-center", "targets": [4] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(9, { page: 'current' }).data().each(function (group, i) {

                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="9" class="rptGrp">' + '<b>Days Past Due</b> : ' + group + '</td></tr>');
                         last = group;

                     }

                 });
             }
         });

        $(".buttons-excel").hide();

    } catch (x) {
        notyAlert('error', x.message);
    }
});


function GetPayableAgeingReport() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode };
            var ds = {};
            ds = GetDataFromServer("Report/GetAccountsPayableAgeingDetails/", data);
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

function RefreshPayableAgeingReportTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        if (DataTables.PayableAgeingReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.PayableAgeingReportTable.clear().rows.add(GetPayableAgeingReport()).draw(false);
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



