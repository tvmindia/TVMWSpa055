﻿using AutoMapper;
using Newtonsoft.Json;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
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
        public ActionResult Index()
        {

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
        public string InsertUpdateInvoice(SupplierInvoicesViewModel _supplierInvoicesObj)
        {
            try
            {
                AppUA ua = new AppUA();
                ua.UserName = "Thomson";
                _supplierInvoicesObj.commonObj = new CommonViewModel();
                _supplierInvoicesObj.commonObj.CreatedBy = ua.UserName;
                _supplierInvoicesObj.commonObj.CreatedDate = DateTime.Now;
                _supplierInvoicesObj.commonObj.UpdatedBy = ua.UserName;
                _supplierInvoicesObj.commonObj.UpdatedDate = DateTime.Now;
                SupplierInvoicesViewModel CIVM = Mapper.Map<SupplierInvoices, SupplierInvoicesViewModel>(_supplierInvoicesBusiness.InsertUpdateInvoice(Mapper.Map<SupplierInvoicesViewModel, SupplierInvoices>(_supplierInvoicesObj)));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CIVM });
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        [HttpGet]
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
        public string GetDueDate(string Code)
        {
            try
            {
                Common com = new Common();
                DateTime Datenow = com.GetCurrentDateTime();
                PaymentTermsViewModel payTermsObj = Mapper.Map<PaymentTerms, PaymentTermsViewModel>(_paymentTermsBusiness.GetPayTermDetails(Code));
                string DuePaymentDueDateFormatted = Datenow.AddDays(payTermsObj.NoOfDays).ToString("dd-MMM-yyyy");
                return JsonConvert.SerializeObject(new { Result = "OK", Records = DuePaymentDueDateFormatted });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        [HttpGet]
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
        #region ButtonStyling
        [HttpGet]
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



                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    break;
                case "Edit":


                    break;
                case "Add":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "$('#btnSave').click();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

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