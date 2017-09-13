﻿using AutoMapper;
using Newtonsoft.Json;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class SupplierInvoicesController : Controller
    {
        ISupplierInvoicesBusiness _supplierInvoicesBusiness;
        AppConst c = new AppConst();
        ISupplierBusiness _supplierBusiness;
        ITaxTypesBusiness _taxTypesBusiness;
        ICompaniesBusiness _companiesBusiness;
        IPaymentTermsBusiness _paymentTermsBusiness;
        ICommonBusiness _commonBusiness;
        public SupplierInvoicesController(ICommonBusiness commonBusiness, IPaymentTermsBusiness paymentTermsBusiness, ICompaniesBusiness companiesBusiness, ISupplierInvoicesBusiness supplierInvoicesBusiness, ISupplierBusiness supplierBusiness, ITaxTypesBusiness taxTypesBusiness)
        {
            _supplierInvoicesBusiness = supplierInvoicesBusiness;
            _supplierBusiness = supplierBusiness;
            _taxTypesBusiness = taxTypesBusiness;
            _companiesBusiness = companiesBusiness;
            _paymentTermsBusiness = paymentTermsBusiness;
            _commonBusiness = commonBusiness;
        }
        // GET: SupplierInvoices
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.value = id;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            SupplierInvoicesViewModel SI = new SupplierInvoicesViewModel();
            
            SI.suppliersObj = new SuppliersViewModel();
            SI.paymentTermsObj = new PaymentTermsViewModel();
            SI.companiesObj = new CompaniesViewModel();
            SI.TaxTypeObj = new TaxTypesViewModel();

            SI.suppliersObj.SupplierList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<SuppliersViewModel> SuppList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_supplierBusiness.GetAllSuppliers());
            foreach (SuppliersViewModel Supp in SuppList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Supp.CompanyName,
                    Value = Supp.ID.ToString(),
                    Selected = false
                });
            }
            SI.suppliersObj.SupplierList = selectListItem;

            SI.paymentTermsObj.PaymentTermsList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<PaymentTermsViewModel> PayTermList = Mapper.Map<List<PaymentTerms>, List<PaymentTermsViewModel>>(_paymentTermsBusiness.GetAllPayTerms());
            foreach (PaymentTermsViewModel PayT in PayTermList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = PayT.Description,
                    Value = PayT.Code,
                    Selected = false
                });
            }
            SI.paymentTermsObj.PaymentTermsList = selectListItem;

            SI.companiesObj.CompanyList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<CompaniesViewModel> CompaniesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            foreach (CompaniesViewModel Cmp in CompaniesList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Cmp.Name,
                    Value = Cmp.Code,
                    Selected = false
                });
            }
            SI.companiesObj.CompanyList = selectListItem;

            SI.TaxTypeObj.TaxTypesList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<TaxTypesViewModel> TaxTypeList = Mapper.Map<List<TaxTypes>, List<TaxTypesViewModel>>(_taxTypesBusiness.GetAllTaxTypes());
            foreach (TaxTypesViewModel TaTy in TaxTypeList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = TaTy.Description,
                    Value = TaTy.Code,
                    Selected = false
                });
            }
            SI.TaxTypeObj.TaxTypesList = selectListItem;
            return View(SI);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "R")]
        public string GetSupplierInvoiceDetails(string ID)
        {
            try
            {
                SupplierInvoicesViewModel SupplierInvoiceObj = Mapper.Map<SupplierInvoices, SupplierInvoicesViewModel>(_supplierInvoicesBusiness.GetSupplierInvoiceDetails(Guid.Parse(ID)));
                if (SupplierInvoiceObj != null)
                {
                    SupplierInvoiceObj.TotalInvoiceAmountstring = _commonBusiness.ConvertCurrency(SupplierInvoiceObj.TotalInvoiceAmount, 0);
                    SupplierInvoiceObj.BalanceDuestring = _commonBusiness.ConvertCurrency(SupplierInvoiceObj.BalanceDue, 0);
                    SupplierInvoiceObj.PaidAmountstring = _commonBusiness.ConvertCurrency((SupplierInvoiceObj.TotalInvoiceAmount - SupplierInvoiceObj.BalanceDue), 0);

                }
                return JsonConvert.SerializeObject(new { Result = "OK", Records = SupplierInvoiceObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "R")]
        public string GetInvoicesAndSummary()
        {
            try
            {

                SupplierInvoiceBundleViewModel Result = new SupplierInvoiceBundleViewModel();

                Result.SupplierInvoiceSummary = Mapper.Map<SupplierInvoiceSummary, SupplierInvoiceSummaryViewModel>(_supplierInvoicesBusiness.GetSupplierInvoicesSummary());
                Result.SupplierInvoices = Mapper.Map<List<SupplierInvoices>, List<SupplierInvoicesViewModel>>(_supplierInvoicesBusiness.GetAllSupplierInvoices());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "W")]
        public string InsertUpdateInvoice(SupplierInvoicesViewModel _supplierInvoicesObj)
        {
            try
            {
                if (_supplierInvoicesObj.TotalInvoiceAmount == 0)
                {
                    throw new Exception("Please Enter Amount");
                }
                AppUA _appUA = Session["AppUA"] as AppUA;
                _supplierInvoicesObj.commonObj = new CommonViewModel();
                _supplierInvoicesObj.commonObj.CreatedBy = _appUA.UserName;
                _supplierInvoicesObj.commonObj.CreatedDate = DateTime.Now;
                _supplierInvoicesObj.commonObj.UpdatedBy = _appUA.UserName;
                _supplierInvoicesObj.commonObj.UpdatedDate = DateTime.Now;
                SupplierInvoicesViewModel SIVM = Mapper.Map<SupplierInvoices, SupplierInvoicesViewModel>(_supplierInvoicesBusiness.InsertUpdateInvoice(Mapper.Map<SupplierInvoicesViewModel, SupplierInvoices>(_supplierInvoicesObj)));
                if(_supplierInvoicesObj.ID!=null&&_supplierInvoicesObj.ID!=Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = SIVM });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = SIVM });
                }
                
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "R")]
        public string GetSupplierDetails(string ID)
        {
            try
            {
                SuppliersViewModel SupplierObj = Mapper.Map<Supplier, SuppliersViewModel>(_supplierBusiness.GetSupplierDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = SupplierObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "R")]
        public string GetDueDate(string Code, string InvDate = "")
        {
            try
            {
                Common com = new Common();
                DateTime Datenow = com.GetCurrentDateTime();
                PaymentTermsViewModel payTermsObj = Mapper.Map<PaymentTerms, PaymentTermsViewModel>(_paymentTermsBusiness.GetPayTermDetails(Code));
                string DuePaymentDueDateFormatted;
                if (InvDate == "")
                {
                    DuePaymentDueDateFormatted = Datenow.AddDays(payTermsObj.NoOfDays).ToString("dd-MMM-yyyy");
                }
                else
                {
                    DuePaymentDueDateFormatted = Convert.ToDateTime(InvDate).AddDays(payTermsObj.NoOfDays).ToString("dd-MMM-yyyy");
                }
                return JsonConvert.SerializeObject(new { Result = "OK", Records = DuePaymentDueDateFormatted });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #region DeleteSupplierInvoice
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "D")]
        public string DeleteSupplierInvoice(string ID)
        {
            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                result = _supplierInvoicesBusiness.DeleteSupplierInvoice(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty,_appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteSupplierInvoice

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "R")]
        public string GetTaxRate(string Code)
        {
            try
            {
                TaxTypesViewModel taxTypesObj = Mapper.Map<TaxTypes, TaxTypesViewModel>(_taxTypesBusiness.GetTaxTypeDetailsByCode(Code));
                decimal Rate = taxTypesObj.Rate;
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Rate });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #region GetSupplierAdvancesByID
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "D")]
        [HttpGet]
        public string GetSupplierAdvances(string ID)
        {
            SupplierInvoicesViewModel Advlist =Mapper.Map<SupplierInvoices, SupplierInvoicesViewModel>(_supplierInvoicesBusiness.GetSupplierAdvances(ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = Advlist });
        }
        #endregion GetSupplierAdvancesByID

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierInvoices", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew();";

                    break;
                case "Edit":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "saveInvoices();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    break;                
                case "Add":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "saveInvoices();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteInvoices();";

                    break;
                case "AddSub":

                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}