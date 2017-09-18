﻿using Newtonsoft.Json;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UserInterface.Models;
using AutoMapper;
using System.Web;
using System.IO;
using SPAccounts.UserInterface.SecurityFilter;
using System.Linq;

namespace UserInterface.Controllers
{
    public class CustomerInvoicesController : Controller
    {
        #region Constructor_Injection

        ICustomerInvoicesBusiness _customerInvoicesBusiness;
        AppConst c = new AppConst();
        ICustomerBusiness _customerBusiness;
        ITaxTypesBusiness _taxTypesBusiness;
        ICompaniesBusiness _companiesBusiness;
        IPaymentTermsBusiness _paymentTermsBusiness;
        ICommonBusiness _commonBusiness;
        public CustomerInvoicesController(ICommonBusiness commonBusiness,IPaymentTermsBusiness paymentTermsBusiness,ICompaniesBusiness companiesBusiness, ICustomerInvoicesBusiness customerInvoicesBusiness,ICustomerBusiness customerBusiness,ITaxTypesBusiness taxTypesBusiness)
        {
            _customerInvoicesBusiness = customerInvoicesBusiness;
            _customerBusiness = customerBusiness;
            _taxTypesBusiness = taxTypesBusiness;
            _companiesBusiness = companiesBusiness;
            _paymentTermsBusiness = paymentTermsBusiness;
            _commonBusiness = commonBusiness;
        }
        #endregion Constructor_Injection

        #region Index
        // GET: Invoices
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.value = id;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            CustomerInvoicesViewModel CI = new CustomerInvoicesViewModel();

            CI.customerObj = new CustomerViewModel();
            CI.paymentTermsObj = new PaymentTermsViewModel();
            CI.companiesObj = new CompaniesViewModel();
            CI.TaxTypeObj = new TaxTypesViewModel();

            CI.customerObj.CustomerList= new List<SelectListItem>();            
            selectListItem = new List<SelectListItem>();
            List<CustomerViewModel> CustList= Mapper.Map<List<Customer>, List< CustomerViewModel >>(_customerBusiness.GetAllCustomers());
            foreach (CustomerViewModel Cust in CustList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Cust.CompanyName,
                    Value = Cust.ID.ToString(),
                    Selected = false
                });
            }
            CI.customerObj.CustomerList = selectListItem;

            CI.paymentTermsObj.PaymentTermsList = new List<SelectListItem>();
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
            CI.paymentTermsObj.PaymentTermsList = selectListItem;

            CI.companiesObj.CompanyList = new List<SelectListItem>();
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
            CI.companiesObj.CompanyList = selectListItem;

            CI.TaxTypeObj.TaxTypesList = new List<SelectListItem>();
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
            CI.TaxTypeObj.TaxTypesList = selectListItem;
            return View(CI);
        }
        #endregion Index

        #region GetCustomerInvoiceDetails
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
        [HttpGet]
        public string GetCustomerInvoiceDetails(string ID)
        {
            try
            {
                CustomerInvoicesViewModel CustomerInvoiceObj = Mapper.Map<CustomerInvoice, CustomerInvoicesViewModel>(_customerInvoicesBusiness.GetCustomerInvoiceDetails(Guid.Parse(ID)));
                if (CustomerInvoiceObj != null)
                {
                    CustomerInvoiceObj.TotalInvoiceAmountstring = _commonBusiness.ConvertCurrency(CustomerInvoiceObj.TotalInvoiceAmount, 0);
                    CustomerInvoiceObj.BalanceDuestring = _commonBusiness.ConvertCurrency(CustomerInvoiceObj.BalanceDue, 0);
                    CustomerInvoiceObj.PaidAmountstring = _commonBusiness.ConvertCurrency((CustomerInvoiceObj.TotalInvoiceAmount-CustomerInvoiceObj.BalanceDue), 0);

                }
                return JsonConvert.SerializeObject(new { Result = "OK", Records = CustomerInvoiceObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetCustomerInvoiceDetails

        #region GetAllInvoices
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
        [HttpGet]    
        public string GetInvoicesAndSummary(string filter)
        {
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                CustomerInvoiceBundleViewModel Result = new CustomerInvoiceBundleViewModel();

                string[] arr = _appUA.RolesCSV.Split(',');
                if (arr.Contains("SAdmin")|| arr.Contains("CEO"))
                {
                    Result.CustomerInvoiceSummary = Mapper.Map<CustomerInvoiceSummary, CustomerInvoiceSummaryViewModel>(_customerInvoicesBusiness.GetCustomerInvoicesSummaryForSA());
                    Result.CustomerInvoices = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetAllCustomerInvoicesForSA());
                } 
                else
                {
                    Result.CustomerInvoiceSummary = Mapper.Map<CustomerInvoiceSummary, CustomerInvoiceSummaryViewModel>(_customerInvoicesBusiness.GetCustomerInvoicesSummary());
                    Result.CustomerInvoices = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetAllCustomerInvoices());
                }
                if (filter != null && filter=="OD")
                {
                    Result.CustomerInvoices = Result.CustomerInvoices.Where(m => m.PaymentDueDate < _appUA.DateTime && m.BalanceDue > 0).ToList();
                }
                else if (filter != null && filter == "OI")
                {
                    Result.CustomerInvoices = Result.CustomerInvoices.Where(m => m.PaymentDueDate >= _appUA.DateTime && m.BalanceDue > 0).ToList();
                }
                else if (filter != null && filter == "FP")
                {
                    Result.CustomerInvoices = Result.CustomerInvoices.Where(m => m.BalanceDue <= 0).ToList();
                }

                return JsonConvert.SerializeObject(new { Result = "OK", Records = Result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllInvoices
        
        #region GetCustomerDetails
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
        [HttpGet]
        public string GetCustomerDetails(string ID)
        {
            try
            {
                CustomerViewModel CustomerObj = Mapper.Map<Customer, CustomerViewModel>(_customerBusiness.GetCustomerDetails(ID!=null&&ID!=""?Guid.Parse(ID):Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = CustomerObj });
            }
            catch(Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetCustomerDetails

        #region GetDueDate
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
        [HttpGet]
        public string GetDueDate(string Code,string InvDate="")
        {
            try
            {
                string DuePaymentDueDateFormatted;
                Common com = new Common();
                DateTime Datenow = com.GetCurrentDateTime();
                PaymentTermsViewModel payTermsObj= Mapper.Map<PaymentTerms, PaymentTermsViewModel>(_paymentTermsBusiness.GetPayTermDetails(Code));
                if (InvDate == "") {
                    DuePaymentDueDateFormatted = Datenow.AddDays(payTermsObj.NoOfDays).ToString("dd-MMM-yyyy");
                }
                else {
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
        #endregion GetDueDate

        #region GetTaxRate
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
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
        #endregion GetTaxRate
        
        #region InsertUpdateInvoice
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "W")]
        [HttpPost]
        public string InsertUpdateInvoice(CustomerInvoicesViewModel _customerInvoicesObj)
        {
            try
            {
                if(_customerInvoicesObj.TotalInvoiceAmount==0)
                {
                    throw new Exception("Please Enter Amount");
                }
                if (_customerInvoicesObj.RefInvoice == null && _customerInvoicesObj.InvoiceType == "PB")
                {
                    throw new Exception("Please Enter Reference Invoice");
                }
                AppUA _appUA = Session["AppUA"] as AppUA;
                _customerInvoicesObj.commonObj = new CommonViewModel();
                _customerInvoicesObj.commonObj.CreatedBy = _appUA.UserName;
                _customerInvoicesObj.commonObj.CreatedDate = DateTime.Now;
                _customerInvoicesObj.commonObj.UpdatedBy= _appUA.UserName;
                _customerInvoicesObj.commonObj.UpdatedDate= DateTime.Now;
                CustomerInvoicesViewModel CIVM = Mapper.Map<CustomerInvoice, CustomerInvoicesViewModel>(_customerInvoicesBusiness.InsertUpdateInvoice(Mapper.Map<CustomerInvoicesViewModel, CustomerInvoice>(_customerInvoicesObj), _appUA));
                if (_customerInvoicesObj.ID != null && _customerInvoicesObj.ID != Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = CIVM });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CIVM });
                }
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateInvoice

        #region DeleteInvoices
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "D")]
        [HttpPost]
        public string DeleteInvoices(CustomerInvoicesViewModel _customerinvObj)
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            object result = null;
            try
            {
                result = _customerInvoicesBusiness.DeleteInvoices(_customerinvObj.ID, _appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess, Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion DeleteInvoices

        #region GetCustomerAdvancesByID
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "D")]
        [HttpGet]
        public string GetCustomerAdvances(string ID)
        {
            CustomerInvoicesViewModel custAdvlist = Mapper.Map<CustomerInvoice, CustomerInvoicesViewModel>(_customerInvoicesBusiness.GetCustomerAdvances(ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = custAdvlist });
        }
        #endregion GetCustomerAdvancesByID

        #region GetAllCustomerInvociesByCustomerID
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
        [HttpGet]
        public string GetAllCustomerInvociesByCustomerID(string CustomerID)
        {
            try
            {
                List < CustomerInvoicesViewModel > CustomerInvoicesList = new List<CustomerInvoicesViewModel>();
                CustomerInvoicesList = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetAllCustomerInvociesByCustomerID(Guid.Parse(CustomerID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = CustomerInvoicesList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllCustomerInvociesByCustomerID


        #region SpecialPayments
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "W")]
        [HttpPost]
        public string SpecialPayments(CustomerInvoicesViewModel _customerInvoicesObj)
        {
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                _customerInvoicesObj.commonObj = new CommonViewModel();
                _customerInvoicesObj.commonObj.CreatedBy = _appUA.UserName;
                _customerInvoicesObj.commonObj.CreatedDate = DateTime.Now;
                _customerInvoicesObj.commonObj.UpdatedBy = _appUA.UserName;
                _customerInvoicesObj.commonObj.UpdatedDate = DateTime.Now;
                CustomerInvoicesViewModel CIVM = Mapper.Map<CustomerInvoice, CustomerInvoicesViewModel>(_customerInvoicesBusiness.InsertUpdateSpecialPayments(Mapper.Map<CustomerInvoicesViewModel, CustomerInvoice>(_customerInvoicesObj), _appUA));
                if ((_customerInvoicesObj.SpecialPayObj.ID != null )&& (_customerInvoicesObj.SpecialPayObj.ID != Guid.Empty))
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = CIVM });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CIVM });
                }
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
        [HttpGet]
        public string GetAllSpecialPayments(string InvoiceID)
        {
            try
            {
                List<CustomerInvoicesViewModel> CustomerInvoicesList = new List<CustomerInvoicesViewModel>();
                 CustomerInvoicesList = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoicesViewModel>>(_customerInvoicesBusiness.GetAllSpecialPayments(InvoiceID != null && InvoiceID != "" ? Guid.Parse(InvoiceID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = CustomerInvoicesList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        
        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
        [HttpGet]
        public string GetSpecialPaymentsDetails(string ID)
        {
            try
            {
                CustomerInvoicesViewModel InvoiceObj = Mapper.Map<CustomerInvoice, CustomerInvoicesViewModel>(_customerInvoicesBusiness.GetSpecialPaymentsDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = InvoiceObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "D")]
        [HttpGet]
        public string DeleteSpecialPayments(CustomerInvoicesViewModel _customerinvObj)
        {
            object result = null;
            try
            {
                result = _customerInvoicesBusiness.DeleteSpecialPayments(_customerinvObj.SpecialPayObj.ID);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess, Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        [AuthSecurityFilter(ProjectObject = "CustomerInvoices", Mode = "R")]
        [HttpGet]
        public string SpecialPaymentSummary(string InvoiceID)
        {
            try
            {
                CustomerInvoicesViewModel CustomerInv = new CustomerInvoicesViewModel();
                CustomerInv = Mapper.Map<CustomerInvoice,CustomerInvoicesViewModel>(_customerInvoicesBusiness.SpecialPaymentSummary(InvoiceID != null && InvoiceID != "" ? Guid.Parse(InvoiceID) : Guid.Empty));
                CustomerInv.TotalInvoiceAmountstring = _commonBusiness.ConvertCurrency(CustomerInv.TotalInvoiceAmount, 0);
                CustomerInv.BalanceDuestring = _commonBusiness.ConvertCurrency(CustomerInv.BalanceDue, 0);
                CustomerInv.PaidAmountstring = _commonBusiness.ConvertCurrency((CustomerInv.PaidAmount), 0);

                return JsonConvert.SerializeObject(new { Result = "OK", Records = CustomerInv });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }





        #endregion SpecialPayments

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

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "List();";

                    //----added for export button--------------

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    //---------------------------------------

                    break;
                case "Edit": 
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteInvoices();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "saveInvoices();";

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
                    ToolboxViewModelObj.savebtn.Event = " saveInvoices();";

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
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}