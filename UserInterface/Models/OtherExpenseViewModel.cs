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
        [Required(ErrorMessage = "Expense date required")]
        [Display(Name = "Date")]
        public string ExpenseDate { get; set; }
        public Guid ID { get; set; }

        [Display(Name = "Expense Date")]
        public string ExpenseDateFormatted { get; set; }
        public string DefaultDate { get; set; }
        

        [Required(ErrorMessage = "Company required")]
        [Display(Name = "Company")]
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
        public string ExpenseRef { get; set; }
        [Display(Name = "Decription")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Amount required")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Account Type required")]
        [Display(Name = "Account")]
        public string AccountCode { get; set; }
        public DashBoardViewModel chartOfAccounts { get; set; }
        public List<SelectListItem> AccountTypes { get; set; }

       
        [Display(Name = "Employee")]
        public Guid? EmpID { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public EmployeeViewModel employee { get; set; }

        
        [Display(Name = "Employee Type")]
        public string EmpTypeCode { get; set; }
        public List<SelectListItem> EmployeeTypeList { get; set; }
        public CommonViewModel commonObj { get; set; }
    }
}