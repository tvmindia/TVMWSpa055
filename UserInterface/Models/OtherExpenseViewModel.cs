﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class OtherExpenseViewModel
    {
        [Required(ErrorMessage = "Transaction date required")]
        [Display(Name = "Transaction Date")]
        public string ExpenseDate { get; set; }
        [Display(Name = "Cheque Date")]
        public string ChequeDate { get; set; }
        [Display(Name = "Cheque Cleared Date")]
        public string ChequeClearDate { get; set; }
        public Guid ID { get; set; }

        [Display(Name = "Reference No")]
        public string RefNo { get; set; }
        [Display(Name = "Reversal Of")]
        public string ReversalRef { get; set; }

        [Display(Name = "Expense Date")]
        public string ExpenseDateFormatted { get; set; }
        [Display(Name = "Filter")]
        public string DefaultDate { get; set; }
        public string creditAmountFormatted { get; set; }
        [Display(Name = "Reference Bank")]
        public string ReferenceBank { get; set; }
        public string ReferenceNo { get; set; }

        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Expense From Company")]
        public string PaidFromCompanyCode { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public CompaniesViewModel companies { get; set; }
       
        [Required(ErrorMessage = "Payment required")]
        [Display(Name = "Payment mode")]
        public string PaymentMode { get; set; }
        public List<SelectListItem> paymentModeList { get; set; }
        public Guid DepWithID { get; set; }
        public DepositAndWithdrwalViewModel depositAndWithdrwal { get; set; }
        
        [Display(Name = "Bank")]
        public string BankCode { get; set; }
        public List<SelectListItem> bankList { get; set; }
        [Display(Name = "Payment Reference")]
        public string ExpenseRef { get; set; }
        [Display(Name = "Decription")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Amount required")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Account Head required")]
        [Display(Name = "Account Head")]
        public string AccountCode { get; set; }

        [Display(Name = "Regular/Reversal")]
        public bool IsReverse { get; set; }
        
        public ChartOfAccountsViewModel chartOfAccountsObj { get; set; }
        public List<SelectListItem> AccountTypes { get; set; }

        
        [Display(Name = "Employee/Other")]        
        public Guid? EmpID { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public EmployeeViewModel employee { get; set; }
        public string EmpName { get; set; }

     
        [Display(Name = "Subtype (Employee,Other,etc.)")]
        public string EmpTypeCode { get; set; }
        public List<SelectListItem> EmployeeTypeList { get; set; }
        public CommonViewModel commonObj { get; set; }
        public bool ShowDaysFilter { get; set; }
        public bool ApprovalFilter { get; set; }

        public string OpeningBank { get; set; }
        public string OpeningNCBank { get; set; }
        public string OpeningCash { get; set; }
        public string UndepositedCheque { get; set; }
//For BankWise Balance//
        public string BankName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal UnClearedAmount { get; set; }

        public decimal ReversableAmount { get; set; }
        [Display(Name = "Approval Status")]
        public int? ApprovalStatus { get; set; }
        public string ApprovalDate { get; set; }
        public bool? IsNotified { get; set; }
        public string OELimitOnEntry { get; set; }
        [Display(Name = "Approval Required Above Amount")]
        public SysSettingsViewModel SysSettingsObj { get; set; }
        public List<SelectListItem> ApprovalStatusList { get; set; }
        
        public decimal UnderClearingAmount { get; set; }
        public string Today { get; set; }
    }


    public class OtherExpSummaryViewModel
    {

        public List<OtherExpSummaryItemViewModel> ItemsList { get; set; }
        public string Month { get; set; }
        public decimal Total { get; set; }
        public String TotalFormatted { get; set; }
    }
    public class OtherExpSummaryItemViewModel
    {
        public string Head { get; set; }
        public decimal Amount { get; set; }
        public string AmountFormatted { get; set; }
        public string color { get; set; }
    }
}