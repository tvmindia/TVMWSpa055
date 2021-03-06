﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{

    public class ChartOfAccountsViewModel
    {
        [Required(ErrorMessage = "Code is missing")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Type is missing")]
        public string Type { get; set; }
        [Display(Name = "Type Description")]
        public string TypeDesc { get; set; }
        public string OpeningPaymentMode { get; set; }
        public decimal OpeningBalance { get; set; }
        public DateTime OpeningAsOfDate { get; set; }
        [Display(Name = "Is Reverse")]
        public bool IsReverse { get; set; }
        [Display(Name = "Head applicable for Purchase Invoice (Y/N)")]
        public bool IsPurchase { get; set; }
        [Display(Name = "Subtype Applicable (Y/N)")]
        [Required(ErrorMessage = "Is Subtype is missing")]
        public bool ISEmploy { get; set; }
        public bool IsAvailLEReport { get; set; }
        public decimal Amount { get; set; }
        //public string account { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public int days { get; set; }
        public CommonViewModel CommonObj { get; set; }
        public string isUpdate { get; set; }
        public string hdnCode { get; set; }
        public string hdnType { get; set; }
        public string AssignRowValues { get; set; }
        public List<ChartOfAccountsViewModel> CheckedRows { get; set; }
    }
}