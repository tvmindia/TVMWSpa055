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
    public class SupplierPaymentsController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        ISupplierPaymentsBusiness _supplierPaymentsBusiness;
        ISupplierBusiness _supplierBusiness;
        IBankBusiness _bankBusiness;
        ICompaniesBusiness _companiesBusiness;
        IApprovalStatusBusiness _approvalStatusBusiness;
        ISupplierCreditNotesBusiness _supplierCreditNotesBusiness;
        IPaymentModesBusiness _paymentmodesBusiness;
        ISupplierInvoicesBusiness _supplierInvoicesBusiness;

        public SupplierPaymentsController(ISupplierPaymentsBusiness supplierPaymentsBusiness,
            IPaymentModesBusiness paymentmodeBusiness,
            ISupplierBusiness supplierBusiness, IBankBusiness bankBusiness, ICompaniesBusiness companiesBusiness,
            ISupplierInvoicesBusiness supplierInvoicesBusiness,
            IApprovalStatusBusiness approvalStatusBusiness, ISupplierCreditNotesBusiness supplierCreditNotesBusiness)
        {
            _supplierPaymentsBusiness = supplierPaymentsBusiness;
            _paymentmodesBusiness = paymentmodeBusiness;
            _supplierInvoicesBusiness = supplierInvoicesBusiness;
            _supplierBusiness = supplierBusiness;
            _bankBusiness = bankBusiness;
            _supplierCreditNotesBusiness = supplierCreditNotesBusiness;
            _companiesBusiness = companiesBusiness;
            _approvalStatusBusiness = approvalStatusBusiness;
        }
        #endregion Constructor_Injection

        #region Index 
        // GET: SupplierPayments
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public ActionResult Index(string id)
        {
            ViewBag.value = id;
            AppUA _appUA = Session["AppUA"] as AppUA;
            ViewBag.Currentdate = _appUA.DateTime.ToString("dd-MMM-yyyy");

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            SupplierPaymentsViewModel SP = new SupplierPaymentsViewModel();
            //-------------1.Supplier List-------------------//
            SP.supplierObj = new SuppliersViewModel();
            SP.supplierObj.SupplierList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<SuppliersViewModel> supplierList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_supplierBusiness.GetAllSuppliers());
            foreach (SuppliersViewModel supplier in supplierList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = supplier.CompanyName,
                    Value = supplier.ID.ToString(),
                    Selected = false
                });
            }
            SP.supplierObj.SupplierList = selectListItem;
            //-------------2.PaymentModes-------------------//
            SP.PaymentModesObj = new PaymentModesViewModel();
            SP.PaymentModesObj.PaymentModesList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<PaymentModesViewModel> PaymentModeList = Mapper.Map<List<PaymentModes>, List<PaymentModesViewModel>>(_paymentmodesBusiness.GetAllPaymentModes());
            foreach (PaymentModesViewModel PMVM in PaymentModeList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = PMVM.Description,
                    Value = PMVM.Code,
                    Selected = false
                });
            }
            SP.PaymentModesObj.PaymentModesList = selectListItem;
            //-------------3.BanksList-------------------//
            SP.bankObj = new BankViewModel();
            SP.bankObj.BanksList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<BankViewModel> BankList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
            foreach (BankViewModel BL in BankList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = BL.Name,
                    Value = BL.Code,
                    Selected = false
                });
            }
            SP.bankObj.BanksList = selectListItem;
            //-------------4.CompanyList-------------------//
            SP.CompanyObj = new CompaniesViewModel();
            SP.CompanyObj.CompanyList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<CompaniesViewModel> CompaniesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
            foreach (CompaniesViewModel BL in CompaniesList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = BL.Name,
                    Value = BL.Code,
                    Selected = false
                });
            }
            SP.CompanyObj.CompanyList = selectListItem;

            //-------------4.Approval Status-------------------//
            SP.ApprovalStatusObj = new ApprovalStatusViewModel();
            SP.ApprovalStatusObj.ApprovalStatusList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<ApprovalStatusViewModel> ApprovalStatus = Mapper.Map<List<ApprovalStatus>, List<ApprovalStatusViewModel>>(_approvalStatusBusiness.GetAllApprovalStatus());
            foreach (ApprovalStatusViewModel BL in ApprovalStatus)
            {
                if (BL.Description!="Paid")
                selectListItem.Add(new SelectListItem
                {
                    Text = BL.Description,
                    Value = BL.Code,
                    Selected = false
                });
                else
                    selectListItem.Add(new SelectListItem
                {
                    Text = BL.Description,
                    Value = BL.Code,
                    Disabled = true
                });
            }
            SP.ApprovalStatusObj.ApprovalStatusList = selectListItem;
            return View(SP);
        }
        #endregion Index 

        #region GetAllSupplierPayments
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetAllSupplierPayments(string filter)
        {
            List<SupplierPaymentsViewModel> supplierPayList = Mapper.Map<List<SupplierPayments>, List<SupplierPaymentsViewModel>>(_supplierPaymentsBusiness.GetAllSupplierPayments());

            if (filter != null && filter == "NA")
            {
                supplierPayList = supplierPayList.Where(m => m.ApprovalStatus==1).ToList();
            }
            else if (filter != null && filter == "AP")
            {
                supplierPayList = supplierPayList.Where(m => m.ApprovalStatus == 2).ToList();
            } 

            return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierPayList });
        }
        #endregion GetAllSupplierPayments

        #region GetAllSupplierPaymentsByID
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetSupplierPaymentsByID(string ID)
        {
            SupplierPaymentsViewModel supplierpaylist = Mapper.Map<SupplierPayments, SupplierPaymentsViewModel>(_supplierPaymentsBusiness.GetSupplierPaymentsByID(ID));
           // if (supplierpaylist.ApprovalStatus != 1)
           // {
                AppUA _appUA = Session["AppUA"] as AppUA;
                if(_appUA.RolesCSV.Contains("CEO") || _appUA.RolesCSV.Contains("SAdmin"))
                {
                    supplierpaylist.HasAccess = true;
                }
            //}
            return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierpaylist });

        }
        #endregion GetAllSupplierPaymentsByID

        #region  GetOutStandingInvoices
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetOutStandingInvoices(string PaymentID, string supplierID)
        {
            List<SupplierInvoicesViewModel> List = Mapper.Map<List<SupplierInvoices>, List<SupplierInvoicesViewModel>>(_supplierInvoicesBusiness.GetOutStandingInvoicesBySupplier(Guid.Parse(PaymentID), Guid.Parse(supplierID)));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = List });

        }
        #endregion  GetOutStandingInvoices

        #region InsertUpdatePayments

        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "W")]
        [HttpPost]
        public string InsertUpdatePayments(SupplierPaymentsViewModel _supplierObj)
        {
            try
            {

                if (_supplierObj.TotalPaidAmt == 0)
                {
                    _supplierObj.TotalPaidAmt = Decimal.Parse(_supplierObj.hdfCreditAmount);
                }

                if (_supplierObj.TotalPaidAmt == 0 && _supplierObj.Type == "C" || _supplierObj.hdfType == "C")
                {
                    _supplierObj.TotalPaidAmt = Decimal.Parse(_supplierObj.hdfCreditAmount);
                    _supplierObj.AdvanceAmount = 0;
                    if (_supplierObj.TotalPaidAmt == 0)
                    { 
                        throw new Exception("Please Check Credit Notes");
                    }
                }
                else if (_supplierObj.TotalPaidAmt == 0)
                { 
                    throw new Exception("Please Enter Amount");
                }
                AppUA _appUA = Session["AppUA"] as AppUA;
                if (_supplierObj.paymentDetailhdf != null)
                    _supplierObj.supplierPaymentsDetail = JsonConvert.DeserializeObject<List<SupplierPaymentsDetailViewModel>>(_supplierObj.paymentDetailhdf);
                _supplierObj.CommonObj = new CommonViewModel();
                _supplierObj.CommonObj.CreatedBy = _appUA.UserName;
                _supplierObj.CommonObj.CreatedDate = _appUA.DateTime;
                _supplierObj.CommonObj.UpdatedBy = _appUA.UserName;
                _supplierObj.CommonObj.UpdatedDate = _appUA.DateTime;
                SupplierPaymentsViewModel CPVM = Mapper.Map<SupplierPayments, SupplierPaymentsViewModel>(_supplierPaymentsBusiness.InsertUpdatePayments(Mapper.Map<SupplierPaymentsViewModel, SupplierPayments>(_supplierObj)));
                if (_supplierObj.ID != null && _supplierObj.ID != Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = CPVM });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CPVM });
                }
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion InsertUpdatePayments

        #region DeletePayments 
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "D")]
        [HttpPost]
        public string DeletePayments(SupplierPaymentsViewModel _supplierpayObj)
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            object result = null;
            try
            {
                result = _supplierPaymentsBusiness.DeletePayments(_supplierpayObj.ID, _appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess, Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion DeletePayments

        #region InsertPaymentAdjustment
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "W")]
        [HttpPost]
        public string InsertPaymentAdjustment(SupplierPaymentsViewModel _supplierpayObj)
        {
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                _supplierpayObj.CommonObj = new CommonViewModel();
                _supplierpayObj.CommonObj.CreatedBy = _appUA.UserName;
                _supplierpayObj.CommonObj.CreatedDate = _appUA.DateTime;
                SupplierPaymentsViewModel CPVM = Mapper.Map<SupplierPayments, SupplierPaymentsViewModel>(_supplierPaymentsBusiness.InsertPaymentAdjustment(Mapper.Map<SupplierPaymentsViewModel, SupplierPayments>(_supplierpayObj)));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = CPVM });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdatePayments

        #region GetCreditNoteBySupplier
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetCreditNoteBySupplier(string ID)
        {
            List<SupplierCreditNoteViewModel> CreditList = Mapper.Map<List<SupplierCreditNote>, List<SupplierCreditNoteViewModel>>(_supplierCreditNotesBusiness.GetCreditNoteBySupplier(Guid.Parse(ID)));

            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditList });
        }
        #endregion GetCreditNoteBySupplier


        #region GetCreditNoteByPaymentID
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetCreditNoteByPaymentID(string ID, string PaymentID)
        {
            List<CustomerCreditNoteViewModel> CreditList = Mapper.Map<List<CustomerCreditNotes>, List<CustomerCreditNoteViewModel>>(_supplierCreditNotesBusiness.GetCreditNoteByPaymentID(Guid.Parse(ID), Guid.Parse(PaymentID)));

            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditList });
        }
        #endregion GetCreditNoteByPaymentID

        #region GetCreditNoteAmount
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetCreditNoteAmount(string CreditID, string SupplierID)
        {
            SupplierCreditNoteViewModel CreditNote = Mapper.Map<SupplierCreditNote, SupplierCreditNoteViewModel>(_supplierCreditNotesBusiness.GetCreditNoteAmount(Guid.Parse(CreditID), Guid.Parse(SupplierID)));

            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNote });

        }
        #endregion GetCreditNoteAmount

        #region GetOutstandingAmountBySupplier
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "R")]
        [HttpGet]
        public string GetOutstandingAmountBySupplier(string CreditID, string SupplierID)
        {
            SupplierPaymentsViewModel Cus_pay = Mapper.Map<SupplierPayments, SupplierPaymentsViewModel>(_supplierPaymentsBusiness.GetOutstandingAmountBySupplier(SupplierID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = Cus_pay });
        }
        #endregion GetOutstandingAmountBySupplier


        #region ApprovedPayment
        [AuthSecurityFilter(ProjectObject = "SupplierPayments", Mode = "W")]
        [HttpPost]
        public string ApprovedPayment(SupplierPaymentsViewModel supobj)
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            object result = null;
            try
            {
                result = _supplierPaymentsBusiness.ApprovedPayment(supobj.ID, _appUA.UserName, _appUA.DateTime);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = result });
             
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion ApprovedPayment


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
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    break;
                case "Edit":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeletePayments();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "savePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.NotyBtn.Visible = true;
                    ToolboxViewModelObj.NotyBtn.Text = "Notify";
                    ToolboxViewModelObj.NotyBtn.Title = "Send Notification";
                    ToolboxViewModelObj.NotyBtn.Event = "SendNotification();";

                    ToolboxViewModelObj.PayBtn.Visible = true;
                    ToolboxViewModelObj.PayBtn.Disable = true;
                    ToolboxViewModelObj.PayBtn.Text = "Pay";
                    ToolboxViewModelObj.PayBtn.Title = "Pay";
                    ToolboxViewModelObj.PayBtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.PayBtn.Event = "ApprovedPayment();"; 

                    break;

                case "Approve":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeletePayments();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "savePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.NotyBtn.Visible = true;
                    ToolboxViewModelObj.NotyBtn.Text = "Notify";
                    ToolboxViewModelObj.NotyBtn.Title = "Send Notification";
                    ToolboxViewModelObj.NotyBtn.Event = "SendNotification();";

                    ToolboxViewModelObj.PayBtn.Visible = true;
                    ToolboxViewModelObj.PayBtn.Text = "Pay";
                    ToolboxViewModelObj.PayBtn.Title = "Pay";
                    ToolboxViewModelObj.PayBtn.Event = "ApprovedPayment();";
                    break;
                case "PaidApproved": 
                    ToolboxViewModelObj.addbtn.Visible = true; 
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeletePayments();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "savePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.NotyBtn.Visible = true;
                    ToolboxViewModelObj.NotyBtn.Text = "Notify";
                    ToolboxViewModelObj.NotyBtn.Title = "Send Notification";
                    ToolboxViewModelObj.NotyBtn.Event = "SendNotification();";

                    ToolboxViewModelObj.PayBtn.Visible = true;
                    ToolboxViewModelObj.PayBtn.Text = "Pay";
                    ToolboxViewModelObj.PayBtn.Title = "Pay";
                    ToolboxViewModelObj.PayBtn.Event = "ApprovedPayment();";
                    break;
                case "PaidManager":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeletePayments();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "savePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.NotyBtn.Visible = true;
                    ToolboxViewModelObj.NotyBtn.Text = "Notify";
                    ToolboxViewModelObj.NotyBtn.Title = "Send Notification";
                    ToolboxViewModelObj.NotyBtn.Event = "SendNotification();";

                    ToolboxViewModelObj.PayBtn.Visible = true;
                    ToolboxViewModelObj.PayBtn.Disable = true;
                    ToolboxViewModelObj.PayBtn.Text = "Pay";
                    ToolboxViewModelObj.PayBtn.Title = "Pay";
                    ToolboxViewModelObj.PayBtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.PayBtn.Event = "ApprovedPayment();";
                    break;
                case "Add":

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNavClick();";

                    ToolboxViewModelObj.NotyBtn.Visible = true;
                    ToolboxViewModelObj.NotyBtn.Disable = true;
                    ToolboxViewModelObj.NotyBtn.Text = "Notify";
                    ToolboxViewModelObj.NotyBtn.Title = "Send Notification";
                    ToolboxViewModelObj.NotyBtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.NotyBtn.Event = "SendNotification();";

                    ToolboxViewModelObj.PayBtn.Visible = true;
                    ToolboxViewModelObj.PayBtn.Disable = true;
                    ToolboxViewModelObj.PayBtn.Text = "Pay";
                    ToolboxViewModelObj.PayBtn.Title = "Pay";
                    ToolboxViewModelObj.PayBtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.PayBtn.Event = "ApprovedPayment();";


                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.deletebtn.Event = "DeletePayments();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "savePayments();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}