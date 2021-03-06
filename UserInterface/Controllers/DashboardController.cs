﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using UserInterface.Models;
using AutoMapper;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using Microsoft.Practices.Unity;

namespace UserInterface.Controllers
{
    public class DashboardController : Controller
    {
        
        #region Constructor_Injection 

        AppConst c = new AppConst();
        IDashboardBusiness _dashboardBusiness;
        IOtherExpenseBusiness _otherExpenseBusiness;
        ICustomerInvoicesBusiness _customerInvoiceBusiness;
        ISupplierInvoicesBusiness _supplierInvoicesBusiness;
        ICommonBusiness _commonBusiness;
        private IDynamicUIBusiness _dynamicUIBusiness;
        [Dependency]
        public IUserBusiness _userBusiness { get; set; }
        public string LoggedUserName { get; set; }

        public DashboardController(IDashboardBusiness dashboardBusiness, IOtherExpenseBusiness otherExpenseBusiness, ICustomerInvoicesBusiness customerInvoiceBusiness, ISupplierInvoicesBusiness supplierInvoicesBusiness, ICommonBusiness commonBusiness, IDynamicUIBusiness dynamicUIBusiness)
        {
            _dashboardBusiness = dashboardBusiness;
            _otherExpenseBusiness = otherExpenseBusiness;
            _customerInvoiceBusiness = customerInvoiceBusiness;
            _supplierInvoicesBusiness = supplierInvoicesBusiness;
            _commonBusiness = commonBusiness;
            _dynamicUIBusiness = dynamicUIBusiness;
        }
        #endregion Constructor_Injection 


        // GET: Dashboard
        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult Index()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            
            if (("," +_appUA.RolesCSV+ ",").Contains(",SAdmin,") || _appUA.RolesCSV.Contains("CEO"))
            {
                return RedirectToAdminDashboard();
            }
            else {
                LoggedUserName = _appUA.UserName;
                DynamicUIViewModel dUIObj = new DynamicUIViewModel();
                List<Menu> menulist = _dynamicUIBusiness.GetAllMenues();
                dUIObj.MenuViewModelList = Mapper.Map<List<Menu>, List<MenuViewModel>>(menulist);
                foreach (MenuViewModel item in dUIObj.MenuViewModelList)
                {
                    if (item.SecurityObject != null)
                    {
                        Permission _permission = _userBusiness.GetSecurityCode(LoggedUserName, item.SecurityObject);

                        if (_permission.AccessCode.Contains('R'))
                        {
                            item.HasAccess = true;
                        }
                    }

                }
                    return View(dUIObj);
            }
            
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult Admin()
        {
            return View();
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult MonthlyRecap(MonthlyRecapViewModel data)
        {
            data = Mapper.Map<MonthlyRecap, MonthlyRecapViewModel>(_dashboardBusiness.GetMonthlyRecap(Mapper.Map<MonthlyRecapViewModel, MonthlyRecap>(data)));
            return PartialView("_MontlyRecap", data);
        }
        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult MonthlySalesPurchase(MonthlySalesPurchaseViewModel data)
        {
          //  MonthlySalesPurchaseViewModel data = new MonthlySalesPurchaseViewModel();
            data = Mapper.Map<MonthlySalesPurchase, MonthlySalesPurchaseViewModel>(_dashboardBusiness.GetSalesPurchase(Mapper.Map<MonthlySalesPurchaseViewModel,MonthlySalesPurchase>(data)));
            return PartialView("_MonthlySalesPurchase", data);
        }
        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult ExpenseSummary(ExpenseSummaryViewModel data)
        {
            if (data.month == 0) {
                data.month = DateTime.Today.Month;
                data.year = DateTime.Today.Year;
            }
            data.OtherExpSummary= Mapper.Map<OtherExpSummary, OtherExpSummaryViewModel>(_otherExpenseBusiness.GetOtherExpSummary(data.month, data.year, data.CompanyName));
            return PartialView("_ExpenseSummary", data);
        }

        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult OutstandingSummary(OutstandingSummaryViewModel data)
        {
            CustomerInvoiceSummaryViewModel CustomerInvoiceSummary = Mapper.Map<CustomerInvoiceSummary, CustomerInvoiceSummaryViewModel>(_customerInvoiceBusiness.GetCustomerInvoicesSummaryForSA());
            SupplierInvoiceSummaryViewModel SupplierInvoiceSummary = Mapper.Map<SupplierInvoiceSummary, SupplierInvoiceSummaryViewModel>(_supplierInvoicesBusiness.GetSupplierInvoicesSummary(data.IsInternal));
            data.OutstandingInv = CustomerInvoiceSummary.OpenAmount + CustomerInvoiceSummary.OverdueAmount;
            data.OuttandingPay = SupplierInvoiceSummary.OpenAmount + SupplierInvoiceSummary.OverdueAmount;

            data.OutstandingInvFormatted = _commonBusiness.ConvertCurrency(data.OutstandingInv, 2);
            data.OuttandingPayFormatted = _commonBusiness.ConvertCurrency(data.OuttandingPay, 2);

            data.invCount = CustomerInvoiceSummary.OpenInvoices + CustomerInvoiceSummary.OverdueInvoices;
            data.payCount = SupplierInvoiceSummary.OpenInvoices + SupplierInvoiceSummary.OverdueInvoices;

            return PartialView("_OutstandingSummary", data);
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult TopCustomers(TopCustomersViewModel data)
        {
            data.BaseURL = "../Customers/Index/";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs("CUST", "ALL", data.BaseURL,data.IsInternal));
            return PartialView("_TopCustomers", data);
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashboard", Mode = "R")]
        public ActionResult TopSuppliers(TopSuppliersViewModel data)
        {
            data.BaseURL = "../Suppliers/Index/";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs("SUPP", "ALL", data.BaseURL, data.IsInternal));
            return PartialView("_TopSuppliers", data);
        }


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentCustomerInvoice(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();

            data.DocType = "CINV";
            data.Title = "Recent Customer Invoices";
            data.Color = "bg-yellow";
            data.BaseURL =  "../CustomerInvoices/Index/";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType,"ALL", data.BaseURL,data.IsInternal));


            return PartialView("_RecentDocs", data);
        }

         


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentCustomerPayments(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "CPAY";
            data.Title = "Recent Customer Payments";
            data.Color = "bg-yellow";
            data.BaseURL = "../CustomerPayments/Index/";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL, data.IsInternal));

            return PartialView("_RecentDocs", data);
        }
        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentSupplierInvoice(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "SINV";
            data.Title = "Recent Supplier Invoices";
            data.Color = "bg-green";
            data.BaseURL = "../SupplierInvoices/Index/";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL,data.IsInternal));
            return PartialView("_RecentDocs", data);
        }

        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentSupplierPayments(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "SPAY";
            data.Title = "Recent Supplier Payments";
            data.Color = "bg-green";
            data.BaseURL = "../SupplierPayments/Index/";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL,data.IsInternal));
            return PartialView("_RecentDocs", data);
        }


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentOtherIncome(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "OI";
            data.Title = "Recent Other Income";
            data.Color = "bg-yellow";
            data.BaseURL = "../OtherIncome/Index/";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL,data.IsInternal));
            return PartialView("_RecentDocs", data);
        }


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult RecentOtherExpense(string Company)
        {
            RecentDocumentsViewModel data = new RecentDocumentsViewModel();
            data.DocType = "OE";
            data.Title = "Recent Other Expense";
            data.Color = "bg-green";
            data.BaseURL = "../OtherExpenses/Index/";
            data.Docs = Mapper.Map<TopDocs, TopDocsVewModel>(_dashboardBusiness.GetTopDocs(data.DocType, "ALL", data.BaseURL,data.IsInternal));
            return PartialView("_RecentDocs", data);
        }



        private ActionResult RedirectToAdminDashboard()
        {
            return RedirectToAction("Admin", "DashBoard");
        }


        [AuthSecurityFilter(ProjectObject = "Dashboard", Mode = "R")]
        public ActionResult  InvAgeingSummary()
        {
            InvoiceAgeingSummary Result = new InvoiceAgeingSummary();

            Result.CustInvAgeSummary = Mapper.Map<CustomerInvoiceAgeingSummary, CustomerInvoiceAgeingSummaryViewModel>(_customerInvoiceBusiness.GetCustomerInvoicesAgeingSummary());
            Result.SuppInvAgeSummary = Mapper.Map<SupplierInvoiceAgeingSummary, SupplierInvoiceAgeingSummaryViewModel>(_supplierInvoicesBusiness.GetSupplierInvoicesAgeingSummary());

            return PartialView("_InvoiceRegisterSummary", Result);
            //  return PartialView("_InvAgeingSummary", Result);
        }

    }
}