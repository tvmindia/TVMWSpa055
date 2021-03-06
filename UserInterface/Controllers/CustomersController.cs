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
    public class CustomersController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        ICustomerBusiness _customerBusiness;
        ICustomerPaymentsBusiness _CustPaymentBusiness;
        IPaymentTermsBusiness _paymentTermsBusiness;

        public CustomersController(ICustomerPaymentsBusiness custPaymentBusiness, ICustomerBusiness customerBusiness, IPaymentTermsBusiness paymentTermsBusiness)
        {
            _CustPaymentBusiness = custPaymentBusiness;
            _customerBusiness = customerBusiness;
            _paymentTermsBusiness = paymentTermsBusiness;
        }
        #endregion Constructor_Injection 
        // GET: Customers
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customers", Mode = "R")]
        public ActionResult Index(string id)
        {
            CustomerViewModel customerViewModel = null;
            ViewBag.value = id;
            try
            {
                customerViewModel = new CustomerViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                //Technician Drop down bind
                List<TitlesViewModel> titlesList = Mapper.Map<List<Titles>, List<TitlesViewModel>>(_customerBusiness.GetAllTitles());
                titlesList = titlesList == null ? null : titlesList.OrderBy(attset => attset.Title).ToList();
                foreach (TitlesViewModel tvm in titlesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = tvm.Title,
                        Value = tvm.Title,
                        Selected = false
                    });
                }
                customerViewModel.TitlesList = selectListItem;

                customerViewModel.DefaultPaymentTermList = new List<SelectListItem>();
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
                customerViewModel.DefaultPaymentTermList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            return View(customerViewModel);

            //CustomerViewModel cv = new CustomerViewModel();
            //cv.DefaultPaymentTermList = new List<SelectListItem>();
            //return View(cv);
        }

        #region GetAllCustomers
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customers", Mode = "R")]
        public string GetAllCustomers()
        {
            try
            {

                List<CustomerViewModel> customersList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = customersList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllCustomers

        #region GetCustomerDetailsByID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customers", Mode = "R")]
        public string GetCustomerDetailsByID(string ID)
        {
            try
            {

                CustomerViewModel customerObj = Mapper.Map<Customer, CustomerViewModel>(_customerBusiness.GetCustomerDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = customerObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetCustomerDetailsByID

        #region InsertUpdateCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Customers", Mode = "W")]
        public string InsertUpdateCustomer(CustomerViewModel _customersObj)
        {
            object result = null;
           
                try
                {
                    AppUA _appUA = Session["AppUA"] as AppUA;
                    _customersObj.commonObj = new CommonViewModel();
                    _customersObj.commonObj.CreatedBy = _appUA.UserName;
                    _customersObj.commonObj.CreatedDate = _appUA.DateTime;
                    _customersObj.commonObj.UpdatedBy = _appUA.UserName;
                    _customersObj.commonObj.UpdatedDate = _appUA.DateTime;

                    result = _customerBusiness.InsertUpdateCustomer(Mapper.Map<CustomerViewModel, Customer>(_customersObj));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
                catch (Exception ex)
                {

                    AppConstMessage cm = c.GetMessage(ex.Message);
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
                }
            
           
        }
        #endregion InsertUpdateCustomer

        #region DeleteCustomer
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customers", Mode = "D")]
        public string DeleteCustomer(string ID)
        {

            try
            {
                object result = null;

                result = _customerBusiness.DeleteCustomer(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteCustomer

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customers", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    break;
                case "Edit":
                   
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Customer";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Customer";
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

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

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