﻿var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    debugger;
    try {
        $("#ddlSupplier,#Supplierddl,#filterEmpID,#ddlfilterAccountCode,#AccountCode,#EmpID").select2({
        });
        $('#btnUpload').click(function ()
        {
            //Pass the controller name
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != emptyGUID) {
                FileObject.ParentID = (($('#ID').val()) != "" ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else
            {
                FileObject.ParentID = $('#ID').val();
            }

            FileObject.ParentType = "SuppInvoice";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });
        DataTables.SupplInvTable = $('#SuppInvTable').DataTable(
        {
            //dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                             }
            }],
            order: [],
            searching: false,
            paging: true,
            data: null,
            pageLength: 15,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
              { "data": "ID" },
              {
                  "data": "InvoiceNo", render: function (data, type, row) {
                      
                      if (row.InvoiceType=='WP')
                          return data+' (WP)';
                      return data;
                  },"width":"5%"
              },
              { "data": "InvoiceDateFormatted","width":"8%" },
              {
                  "data": "suppliersObj.CompanyName", render: function (data, type, row) {
                     
                      if (row.AccountCode != null && row.EmpName != null)
                          return data + '<br/><b>Acc.Head:</b>' + row.AccountCode + '<br/><b>Sub Type:</b>' + row.EmpName;
                      else if (row.AccountCode != null && row.EmpName == null)
                          return data + '<br/><b>Acc.Head:</b>' + row.AccountCode;
                      else
                          return data;
                  }, "defaultContent": "<i>-</i>", "width": "15%"
              },
              { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>", "width": "8%" },
              { "data": "TotalInvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>", "width": "10%" },
              { "data": "PaidAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>", "width": "10%" },
              { "data": "PaymentProcessed", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>", "width": "10%" },
              { "data": "BalanceDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>", "width": "10%" },
              { "data": "LastPaymentDateFormatted", "defaultContent": "<i>-</i>","width":"8%" },
              { "data": "companiesObj.Name", "defaultContent": "<i>-</i>" },
              { "data": "Status", "defaultContent": "<i>-</i>"},
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
               { className: "text-right", "targets": [5, 6,7,8] },
               { className: "text-Left", "targets": [1,3,10] },
               { className: "text-center", "targets": [ 2,4,9,11] }
            ]
        });
        $(".buttons-excel").hide();        
        $('.Roundoff').on('change', function () {
            debugger;
            if ((parseFloat($('#txtDiscount').val())) > (parseFloat($('#txtGrossAmt').val())) || $("#txtDiscount").val() == "") {
                $('#txtDiscount').val("0.00");
            }
            var SupplierInvoiceViewModel = new Object();
            SupplierInvoiceViewModel.GrossAmount = parseFloat($('#txtGrossAmt').val());
            SupplierInvoiceViewModel.Discount = parseFloat($('#txtDiscount').val());
            SupplierInvoiceViewModel.NetTaxableAmount = (SupplierInvoiceViewModel.GrossAmount - SupplierInvoiceViewModel.Discount)
            SupplierInvoiceViewModel.TaxAmount = parseFloat($('#txtTaxAmt').val() == "" ? "0.00" : $('#txtTaxAmt').val());
            SupplierInvoiceViewModel.ShippingCharge = parseFloat($('#ShippingCharge').val() == "" ? "0.00" : $('#ShippingCharge').val());
            SupplierInvoiceViewModel.TotalInvoiceAmount = (SupplierInvoiceViewModel.NetTaxableAmount + SupplierInvoiceViewModel.ShippingCharge
                                                            + SupplierInvoiceViewModel.TaxAmount);
            $('#txtNetTaxableAmt').val(SupplierInvoiceViewModel.NetTaxableAmount);
            $('#ShippingCharge').val(roundoff(SupplierInvoiceViewModel.ShippingCharge));
            $('#txtTaxAmt').val(roundoff(SupplierInvoiceViewModel.TaxAmount));
            $('#txtTotalInvAmt').val(SupplierInvoiceViewModel.TotalInvoiceAmount);         

        });
        $('#txtTaxPercApp').on('keypress', function ()
        {
            if ($('#ddlTaxType').val() != "")
                $('#ddlTaxType').val('')
        });
        $('#SuppInvTable tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
      

        $('input[type="text"].Roundoff').on('focus', function () {
            $(this).select();
        });
        showLoader();
        List();


        //------------------------Modal Popup Advance Adujustment-------------------------------------//
        DataTables.OutStandingInvoices = $('#tblOutStandingDetails').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: false,
            paging: false,
            data: null,
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": null, "defaultContent": "" },
                 {
                     "data": "Description", 'render': function (data, type, row) {
                         return ' Invoice # ' + row.InvoiceNo + '(Date:' + row.InvoiceDateFormatted + ')'
                     }, "width": "30%"
                 },
                 { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "TotalInvoiceAmount", "defaultContent": "<i>-</i>", "width": "15%",
                     'render': function (data, type, row)
                     {
                         return roundoff(row.TotalInvoiceAmount)
                     }
                 },
                 {
                     "data": "OtherPayments", "defaultContent": "<i>-</i>", "width": "15%",
                     'render': function (data, type, row)
                     {
                         return roundoff(row.OtherPayments)
                     }
                 },
                 {
                     "data": "BalanceDue", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row)
                     {
                         return roundoff(row.BalanceDue)
                     }
                 },
                 {
                     "data": "SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount", 'render': function (data, type, row) {
                         return '<input class="form-control text-right paymentAmount" name="Markup" value="' + roundoff(data) + '" onfocus="this.select();" onchange="PaymentAmountChanged(this);" id="PaymentValue" type="text">';
                     }, "width": "15%"
                 },
                 { "data": "SuppPaymentObj.supplierPaymentsDetailObj.ID", "defaultContent": "<i>-</i>" }
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 }
                , { className: "text-right", "targets": [4, 5, 6] }
                , { "targets": [0, 8], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6, 7], "bSortable": false }],

            select: { style: 'multi', selector: 'td:first-child' }
        });

        $('#tblOutStandingDetails tbody').on('click', 'td:first-child', function (e) {
            debugger;
            var rt = DataTables.OutStandingInvoices.row($(this).parent()).index()
            var table = $('#tblOutStandingDetails').DataTable();
            var allData = table.rows().data();
            if ((allData[rt].CustPaymentObj.CustPaymentDetailObj.PaidAmount) > 0) {
                allData[rt].CustPaymentObj.CustPaymentDetailObj.PaidAmount = roundoff(0)
                DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
                var sum = 0;
                AmountReceived = parseFloat($('#AdvanceAmount').text());
                for (var i = 0; i < allData.length; i++) {
                    sum = sum + parseFloat(allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount);
                }
                $('#lblPaymentApplied').text(roundoff(sum));
                $('#lblCredit').text(roundoff(AmountReceived - sum));
                Selectcheckbox();
            }
        });
    }
    catch (e) {

    }

    if ($('#BindValue').val() != '')
    {
        dashboardBind($('#BindValue').val())
    }

});

//To trigger export button
function PrintReport()
{
    try {
        debugger;

        $(".buttons-excel").trigger('click');


    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}

function dashboardBind(ID)
{
    ResetForm(); 
    $('#ID').val(ID);
    PaintInvoiceDetails();
    openNav();
}

//To reset form
function ResetForm() {
    var validator = $("#SupplierInvoiceForm").validate();
    $('#SupplierInvoiceForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
    $('#SupplierInvoiceForm')[0].reset();
}

function Edit(Obj)
{
    debugger;
    ResetForm(); 
    var rowData = DataTables.SupplInvTable.row($(Obj).parents('tr')).data();
    $('#ID').val(rowData.ID);
    PaintInvoiceDetails();
    openNav();
}

function PaintInvoiceDetails()
{
    ChangeButtonPatchView("SupplierInvoices", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var InvoiceID = $('#ID').val();
    var SupplierInvoiceViewModel = GetSupplierInvoiceDetails(InvoiceID);
    $('#ddlInvoiceType').val(SupplierInvoiceViewModel.InvoiceType);
    $('#ddlInvoiceType').prop('disabled', true);
    InvoicesTypeChange();

    $('#lblInvoiceNo').text(SupplierInvoiceViewModel.InvoiceNo);
    $('#txtInvNo').val(SupplierInvoiceViewModel.InvoiceNo);
    $('#txtInvDate').val(SupplierInvoiceViewModel.InvoiceDateFormatted);
    $('#ddlCompany').val(SupplierInvoiceViewModel.companiesObj.Code);
    $("#ddlSupplier").select2();
    $("#ddlSupplier").val(SupplierInvoiceViewModel.suppliersObj.ID).trigger('change');
    //$('#ddlSupplier').val(SupplierInvoiceViewModel.suppliersObj.ID);
    $('#txtBillingAddress').val(SupplierInvoiceViewModel.BillingAddress);
    $('#ddlPaymentTerm').val(SupplierInvoiceViewModel.paymentTermsObj.Code);
    $('#txtPayDueDate').val(SupplierInvoiceViewModel.PaymentDueDateFormatted);
    $('#txtGrossAmt').val(SupplierInvoiceViewModel.GrossAmount);
    $('#txtDiscount').val(SupplierInvoiceViewModel.Discount);
    $('#txtNetTaxableAmt').val(SupplierInvoiceViewModel.GrossAmount - SupplierInvoiceViewModel.Discount);
    $('#ShippingCharge').val(SupplierInvoiceViewModel.ShippingCharge);
   // $('#ddlTaxType').val(SupplierInvoiceViewModel.TaxTypeObj.Code);
  //  $('#txtTaxPercApp').val(SupplierInvoiceViewModel.TaxPercApplied);
    $('#txtTaxAmt').val(SupplierInvoiceViewModel.TaxAmount);
    $('#txtTotalInvAmt').val(SupplierInvoiceViewModel.TotalInvoiceAmount);
    $('#txtNotes').val(SupplierInvoiceViewModel.Notes);
    $('#ID').val(SupplierInvoiceViewModel.ID);
    $('#lblinvoicedAmt').text(SupplierInvoiceViewModel.TotalInvoiceAmountstring);
    $('#lblpaidAmt').text(SupplierInvoiceViewModel.PaidAmountstring);
    $('#lblPaymentProcessed').text("(Payment Processed: " + SupplierInvoiceViewModel.PaymentProcessed+" )");

    $("#AccountCode").val(SupplierInvoiceViewModel.AccountCode).trigger('change');
    $("#EmpID").val(SupplierInvoiceViewModel.EmpID).trigger('change');
    accountCodeOnChange();

    $('#lblbalalnceAmt').text(SupplierInvoiceViewModel.BalanceDuestring);
    clearUploadControl();
    PaintImages(InvoiceID);
}
function List()
{
    $('#filter').hide();
    var result = GetAllInvoicesAndSummary();
    if (result != null)
    {
        if (result.SupplierInvoices != null)
            DataTables.SupplInvTable.clear().rows.add(result.SupplierInvoices).draw(false);
        if (result.SupplierInvoiceSummary != null)
        {
            Summary(result.SupplierInvoiceSummary);
        }
    }
}

function Summary(Records)
{
    $('#overdueamt').html(Records.OverdueAmountFormatted);
    $('#overdueinvoice').html(Records.OverdueInvoices);
    $('#openamt').html(Records.OpenAmountFormatted);
    $('#openinvoice').html(Records.OpenInvoices);
    $('#paidamt').html(Records.PaidAmountFormatted);
    $('#paidinvoice').html(Records.PaidInvoices);
}
//---------------Bind logics-------------------
function GetAllInvoicesAndSummary(filter)
{
    try
    {
        if ($("#fromdate").val() !== "")
            var fromdate = $("#fromdate").val();
        if ($("#todate").val() !== "")
            var todate = $("#todate").val();
        if ($("#Supplierddl").val() != "")
            var suppliercode = $("#Supplierddl").val();
        if ($("#ddlInvoiceTypes").val() != "")
            var invoicetype = $("#ddlInvoiceTypes").val();
        if ($("#Companyddl").val() != "")
            var companycode = $("#Companyddl").val();
        if ($("#ddlInvoiceTypesStatus").val() != "")
            var status = $("#ddlInvoiceTypesStatus").val();
        if ($("#ddlfilterAccountCode").val() != "")
            var AccountCode = $("#ddlfilterAccountCode").val();
        if ($("#filterEmpID").val() != "")
            var EmpID = $("#filterEmpID").val();
        if ($("#search").val() != "")
                var search = $("#search").val();
        var data = { "filter": filter, "FromDate": fromdate, "ToDate": todate, "Supplier": suppliercode, "InvoiceType": invoicetype, "Company": companycode, "Status": status, "Search": search, "AccountCode": AccountCode, "EmpID": EmpID };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetInvoicesAndSummary/", data);
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


function RefreshInvoicesAndSummary()
{
    try
    {
        $('#filter').hide();
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#Companyddl").val();
        var result = GetAllInvoicesAndSummary();
        if (result != null) {
                if (result.SupplierInvoices != null)
                    DataTables.SupplInvTable.clear().rows.add(result.SupplierInvoices).draw(true);
                if (result.SupplierInvoiceSummary != null)
                {
                    Summary(result.SupplierInvoiceSummary);
                }
        }
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}

function save()
{
    debugger;

    $('#btnSave').trigger('click');
}

function saveInvoices() {
    debugger;
    //if ($('#txtTotalInvAmt').val() == 0) {
    //    notyAlert('error', 'Please Enter Amount');
    //}   
    try
    {
     var invoiceNo = $('#txtInvNo').val();
     var supplierID = $('#ddlSupplier').val();
     var id = $("#ID").val();
     if (id == "" || id == null)
     {
         if ((invoiceNo != '' && invoiceNo != null) && (supplierID != '' && supplierID != null)) {
             var data = { "InvoiceNo": invoiceNo, "SupplierID": supplierID, "ID": emptyGUID };
             var ds = {};
             ds = GetDataFromServer("SupplierInvoices/CheckProfileExists/", data);
             debugger;

             if (ds != '') {
                 ds = JSON.parse(ds);
                 if (ds.Message == true) {
                     notyConfirm('Same Invoice No. Already Exists,Do You Want To Continue?', 'save()',"", "Continue!",1);
                 }
                 if (ds.Message == false) {
                     save();
                 }
             }
             if (ds.Result == "OK") {                
                 //save();
                 //notyAlert('success', ds.Message.Message);
             }
             if (ds.Result == "ERROR") {
                 notyAlert('error', ds.Message);
                 return 0;
             }
             return 1;
         }
         else
         {
           save();
         }
     }   

   else  if (id != "" || id != null)
     {
         if ((invoiceNo != '' && invoiceNo != null) && (supplierID != '' && supplierID != null)) {
             var data = { "InvoiceNo": invoiceNo, "SupplierID": supplierID,"ID":id };
             var ds = {};
             ds = GetDataFromServer("SupplierInvoices/CheckProfileExists/", data);
             debugger;

             if (ds != '') {
                 ds = JSON.parse(ds);
                 if (ds.Message == true) {
                     notyConfirm('Same Invoice No. Already Exists,Do You Want To Continue?', 'save()',"", "Continue!",1);
                 }
                 if (ds.Message == false) {
                     save();
                 }
             }
             if (ds.Result == "OK") {                
                 //save();
                 //notyAlert('success', ds.Message.Message);
             }
             if (ds.Result == "ERROR") {
                 notyAlert('error', ds.Message);
                 return 0;
             }
             return 1;
         }
         else
         {
             save();
         }     
   }
   else
   {
       save();
   }
     
}
    catch (e)
    {
        notyAlert('error', e.message);
        return 0;
    }       
         
}

function SaveSuccess(data, status) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            var res;
            if ($('#ID').val() == "") {
                debugger;
              res = Advanceadjustment(); //calling advance adjustment popup if inserting
            }
            if(!res){
                notyAlert('success', JsonResult.Message);
            }
            $('#ID').val(JsonResult.Records.ID);
            PaintInvoiceDetails()
            List();
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
//------------------------Modal Popup Advance Adujustment-------------------------------------//
function Advanceadjustment() {
    debugger;
    var SupplierId = $('#ddlSupplier').val();
    //get advances of Supplier
    var thisitem = GetSupplierAdvances(SupplierId);
    if (thisitem != null && thisitem.suppliersObj.AdvanceAmount>0) {
        $('#AdvanceAmount').text(roundoff(thisitem.suppliersObj.AdvanceAmount));
        $('#AdvAdjustmentModel').modal('show');
        DataTables.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices(SupplierId)).draw(false);
        AmountChanged();
        return true;
    }
    else {
        return false;
    }
}
function SaveAdvanceAdujust() {
    debugger;
    var SelectedRows = DataTables.OutStandingInvoices.rows(".selected").data();
    if ((SelectedRows) && (SelectedRows.length > 0)) {
        var SupplierPaymentsViewModel = new Object();
        SupplierPaymentsViewModel.SupplierPaymentsDetail = []
        for (var r = 0; r < SelectedRows.length; r++) {
            var PaymentDetailViewModel = new Object();

            PaymentDetailViewModel.InvoiceID = SelectedRows[r].ID;//Invoice ID
            PaymentDetailViewModel.ID = SelectedRows[r].SuppPaymentObj.supplierPaymentsDetailObj.ID//Detail ID
            PaymentDetailViewModel.PaidAmount = SelectedRows[r].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount;
            SupplierPaymentsViewModel.SupplierPaymentsDetail.push(PaymentDetailViewModel)
        }
        var SupplierViewModel = new Object();
        SupplierViewModel.ID = $('#ddlSupplier').val();
        SupplierPaymentsViewModel.supplierObj = SupplierViewModel;
        //PostDataToServer
        try {
            var data = "{'_supplierpayObj':" + JSON.stringify(SupplierPaymentsViewModel) + "}";
            PostDataToServer("SupplierPayments/InsertPaymentAdjustment/", data, function (JsonResult) {
                if (JsonResult != '') {
                    switch (JsonResult.Result) {
                        case "OK":
                            notyAlert('success', JsonResult.Message);
                            List();
                            break;
                        case "ERROR":
                            notyAlert('error', JsonResult.Message);
                            break;
                        default:
                            break;
                    }
                }
            });
        }
        catch (e) {
            notyAlert('error', e.message);
        }
    }
    $('#AdvAdjustmentModel').modal('hide');
}

function AmountChanged() {
    debugger;
    var sum = 0;
    DataTables.OutStandingInvoices.rows().deselect();
    AmountReceived = parseFloat($('#AdvanceAmount').text()) 
    if (!isNaN(AmountReceived)) {
        var table = $('#tblOutStandingDetails').DataTable();
        var allData = table.rows().data();
        var RemainingAmount = AmountReceived;
        for (var i = 0; i < allData.length; i++) {
            var SuppPaymentObj = new Object;
            var supplierPaymentsDetailObj = new Object;
            SuppPaymentObj.supplierPaymentsDetailObj = supplierPaymentsDetailObj;
            if (RemainingAmount != 0) {
                if (parseFloat(allData[i].BalanceDue) < RemainingAmount) {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = parseFloat(allData[i].BalanceDue)
                    RemainingAmount = RemainingAmount - parseFloat(allData[i].BalanceDue);
                    sum = sum + parseFloat(allData[i].BalanceDue);
                }
                else {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = RemainingAmount
                    sum = sum + RemainingAmount;
                    RemainingAmount = 0
                }
            }
            else {
                allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = 0;
            }
        }
        DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
        Selectcheckbox();
        $('#lblPaymentApplied').text(roundoff(sum));
        $('#lblCredit').text(roundoff(AmountReceived - sum));
    }
}

function PaymentAmountChanged(this_Obj) {
    debugger;
    AmountReceived = parseFloat($('#TotalPaidAmt').val())
    sum = 0;
    var allData = DataTables.OutStandingInvoices.rows().data();
    var table = DataTables.OutStandingInvoices;
    var rowtable = table.row($(this_Obj).parents('tr')).data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].ID == rowtable.ID) {
            var oldamount = parseFloat(allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount)
            var credit = parseFloat($("#lblCredit").text())
            if (credit > 0) {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - (credit + oldamount)
            }
            else {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - oldamount
            }
            if (parseFloat(allData[i].BalanceDue) < parseFloat(this_Obj.value)) {
                if (currenttotal < AmountReceived) {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = parseFloat(allData[i].BalanceDue)
                    sum = sum + parseFloat(allData[i].BalanceDue);
                }
                else {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = oldamount
                    sum = sum + oldamount
                }
            }
            else {
                if (currenttotal > AmountReceived) {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = oldamount
                    sum = sum + oldamount;
                }
                else {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = this_Obj.value;
                    sum = sum + parseFloat(this_Obj.value);
                }
            }
        }
        else {
            sum = sum + parseFloat(allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount);
        }
    }
    DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
    $('#lblPaymentApplied').text(roundoff(sum));
    $('#lblCredit').text(roundoff(AmountReceived - sum));
    Selectcheckbox();
}

function Selectcheckbox() {
    debugger;
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount == "" || allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount == roundoff(0)) {
            DataTables.OutStandingInvoices.rows(i).deselect();
        }
        else {
            DataTables.OutStandingInvoices.rows(i).select();
        }
    }
}


function GetSupplierAdvances(ID) {
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetSupplierAdvances/", data);
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
function GetOutStandingInvoices(supplierID) {
    try {
        debugger;
        var PaymentID = emptyGUID;
        var data = { "supplierID": supplierID, "PaymentID": PaymentID };
        var ds = {};
        ds = GetDataFromServer("SupplierPayments/GetOutStandingInvoices/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//------------------------Modal Popup Advance Adujustment functions Ends-------------------------------------//
function AddNew() {
 
    ResetForm();
    InvoicesTypeChange()
    $('#ddlInvoiceType').prop('disabled', false);
    $("#ddlSupplier").select2();
    $("#ddlSupplier").val('').trigger('change');

    $("#AccountCode").select2();
    $("#AccountCode").val('').trigger('change');
    $("#EmpID").select2();
    $("#EmpID").val('').trigger('change');

    $('#lblinvoicedAmt').text("₹ 0.00");
    $('#lblpaidAmt').text("₹ 0.00");
    $('#lblbalalnceAmt').text("₹ 0.00");
    $('#ID').val('');
    $('#lblInvoiceNo').text("New Invoice");
    openNav();
    ChangeButtonPatchView("SupplierInvoices", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
    clearUploadControl();
}

function Reset() {
    if ($("#ID").val() == "0") {
        ResetFrm();
    }
    else {
        PaintInvoiceDetails();
    }
}

function FilterReset() {
    $("#fromdate").val('');
    $("#todate").val('');
    $("#Supplierddl").val('').trigger('change')
    $("#ddlInvoiceTypes").val('');
    $("#Companyddl").val('');
    $("#ddlInvoiceTypesStatus").val('');
    $("#search").val('');
    $("#filterEmpID").val('');
    $("#ddlfilterAccountCode").val('');
    List();
}

function FillCustomerDefault(this_Obj) {
    try {
        if (this_Obj.value != "") {
            var ID = this_Obj.value;
            var SupplierViewModel = GetSupplierDetails(ID);
            $('#txtBillingAddress').val(SupplierViewModel.BillingAddress);
            $('#ddlPaymentTerm').val(SupplierViewModel.PaymentTermCode);
            $('#ddlPaymentTerm').trigger('change');
        }
    }
    catch (e) {

    }
}
function GetTaxRate(Code) {
    debugger;
    try {
        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetTaxRate/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            return 0;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function GetSupplierDetails(ID) {
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetSupplierDetails/", data);
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
function GetDueDate(this_Obj) {
    try {
        debugger;
        var Code = this_Obj.value;
        var PaymentTermViewModel = GetPaymentTermDetails(Code);
        $('#txtPayDueDate').val(PaymentTermViewModel);
    }
    catch (e) {

    }
}
function GetPaymentTermDetails(Code) {
    debugger;
    try {
        var data = { "Code": Code, "InvDate": $('#txtInvDate').val() }; //var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetDueDate/", data);
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
function GetSupplierInvoiceDetails() {
    try {
        var InvoiceID = $('#ID').val();
        var data = { "ID": InvoiceID };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetSupplierInvoiceDetails/", data);
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

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteSupplierInvoices()', '', "Yes, delete it!");
}

function DeleteSupplierInvoices() {
    try {
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("SupplierInvoices/DeleteSupplierInvoice/", data);
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
function goBack() {
    ResetForm();
    closeNav();
    List();
}


function InvoicesTypeChange() {
    debugger;
    if ($('#ddlInvoiceType').val() == "WB") {
        $('#txtInvNo').prop('disabled', true);
        $('#txtInvNo').val('');
    } 
    else {
        $('#txtInvNo').prop('disabled', false);  
    }
}
//------------------------------------------------Summary Filter clicks------------------------------------------------------------//

function Gridfilter(thisobj) {
    debugger;
    $('#filter').show();
    $('#ODfilter').hide();
    $('#OIfilter').hide();
    $('#FPfilter').hide();

    if (thisobj == 'OD') {
        $('#ODfilter').show();
    }
    else if (thisobj == 'OI') {
        $('#OIfilter').show();
    }
    else if (thisobj == 'FP') {
        $('#FPfilter').show();
    }
    var result = GetAllInvoicesAndSummary(thisobj);
    if (result != null) {
        if (result.SupplierInvoices != null)
            DataTables.SupplInvTable.clear().rows.add(result.SupplierInvoices).draw(false);
        if (result.SupplierInvoiceSummary != null) {
            Summary(result.SupplierInvoiceSummary);
        }
    }
}

function accountCodeOnChange() {
    debugger;
    var AccountCode = [];
    if ($("#AccountCode").val() !== null) {
        AccountCode = $("#AccountCode").val().split(':');
        if (AccountCode[1] == 'True') {
            $('#EmpID').prop('disabled', false);
        }
        else {
            $('#EmpID').prop('disabled', true);
            $("#EmpID").val('');
        }
    }
}
function onChangeFilterAccountCode() {
    debugger;
    var AccountCode = [];
    if ($("#AccountCode").val() !== null) {
        AccountCode = $("#ddlfilterAccountCode").val().split(':');
        if (AccountCode[1] == 'True') {
            $('#filterEmpID').prop('disabled', false);
        }
        else {
            $('#filterEmpID').prop('disabled', true);
            $("#filterEmpID").val('');
        }
    }
}