﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class SupplierPaymentsViewModel
    {
        public Guid ID { get; set; }
        public string EntryNo { get; set; }

        [Display(Name = "Payment/Credit Adjustment")]
        public string Type { get; set; }

        [Display(Name = "Credit Note To Adjust")]
        public Guid CreditID { get; set; }
        public string CreditNo { get; set; }
        public string hdfType { get; set; }
        public string hdfCreditID { get; set; }
        [Display(Name = "Reference Bank")]
        public string ReferenceBank { get; set; }

        [Required(ErrorMessage = "Paid From Company is missing")]
        [Display(Name = "Paid From Company")]
        public string PaidFromComanyCode { get; set; }

        [Required(ErrorMessage = "Payment Mode is missing")]
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }

        public Guid DepWithdID { get; set; }

        [Display(Name = "Withdrawal From")]
        public string BankCode { get; set; }

        [Display(Name = "Reference No.")]
        public string PaymentRef { get; set; }

        [Required(ErrorMessage = "Payment Date is missing")]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Cheque Date is missing")]
        [Display(Name = "Cheque Date")]
        public string ChequeDate { get; set; }

        [Display(Name = "Cheque Cleared Date")]
        public string ChequeClearDate { get; set; }

        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }

        [Display(Name = "Approval Status")]
        public int ApprovalStatus { get; set; }
      
        [Display(Name = "Approval Date")]
        public string ApprovalDate { get; set; }
        

        [Required(ErrorMessage = "Amount Paid is missing")]
        [Display(Name = "Amount Paid")]
        public decimal TotalPaidAmt { get; set; }
        public decimal AdvanceAmount { get; set; }
        public Guid hdnFileID { get; set; }
        public string PaymentDateFormatted { get; set; }
        public List<SupplierPaymentsDetailViewModel> supplierPaymentsDetail { get; set; }
        public SupplierPaymentsDetailViewModel supplierPaymentsDetailObj { get; set; }

        public SuppliersViewModel supplierObj { get; set; }
        public CommonViewModel CommonObj { get; set; }
        public PaymentModesViewModel PaymentModesObj { get; set; }
        public BankViewModel bankObj { get; set; }
        public CompaniesViewModel CompanyObj { get; set; }
        public SupplierCreditNoteViewModel CreditObj { get; set; }
        public ApprovalStatusViewModel ApprovalStatusObj { get; set; }
        public ApprovalStatusViewModel ApproveStatusObj { get; set; }

        public string paymentDetailhdf { get; set; }
        public string hdfSupplierID { get; set; }
        public string hdfCreditAmount { get; set; }
        public string OutstandingAmount { get; set; }
        public string InvoiceOutstanding { get; set; }
        public string CreditOutstanding { get; set; }
        public string AdvOutstanding { get; set; }
        public string PaymentOutstanding { get; set; }
        public string PaymentBooked { get; set; }

        public bool HasAccess { get; set; }
        public string IsNotificationSuccess { get; set; }
        public string Search { get; set; }

        public DocumentLogViewModel DocumentLogObj { get; set; }


    }
    public class SupplierPaymentsDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid InvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string CreatedDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PrevPayment { get; set; }
        public decimal CurrPayment { get; set; }
        public decimal BalancePayment { get; set; }
        public string PaymentDueDate { get; set; }
        public decimal OriginalInvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal PaidAmount { get; set; }
        public string DueDays { get; set; }
        public string PaymentDueDateFormatted { get; set; }
        public SupplierPaymentsDetailViewModel supplierPaymentDetailObj { get; set; }

    }
    
    public class SupplierPaymentsAdvanceSearchViewModel
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Company { get; set; }
        public string PaymentMode { get; set; }
        public string Customer { get; set; }
        public string Search { get; set; }
    }

}