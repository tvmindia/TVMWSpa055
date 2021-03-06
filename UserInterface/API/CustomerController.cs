﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SPAccounts.BusinessService.Contracts;
using Newtonsoft.Json;
using AutoMapper;
using SPAccounts.DataAccessObject.DTO;
using SAMTool.DataAccessObject.DTO;
using UserInterface.Models;

namespace UserInterface.API
{
   
    public class CustomerController : ApiController
    {
        #region Constructor_Injection

        ICustomerBusiness _customerBusiness;
        

        public CustomerController(ICustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
            
        }
        #endregion Constructor_Injection

        Const messages = new Const();

        [HttpPost]
        public object GetCustomerDetailsMobile(Customer cusObj)
        {
            try
            {
                List<CustomerViewModel> CustomerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomersForMobile(cusObj));
                if (CustomerList.Count == 0) throw new Exception(messages.NoItems);
                return JsonConvert.SerializeObject(new { Result = true, Records = CustomerList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }  

        #region GetCustomerDetailsByID
        [HttpPost]
        public string GetCustomerDetailsByIDForMobile(Customer cust)
        {
            try
            {

                if (cust== null) throw new Exception(messages.NoItems);
                CustomerViewModel customerObj = Mapper.Map<Customer, CustomerViewModel>(_customerBusiness.GetCustomerDetailsForMobile(cust.ID != null && cust.ID.ToString() != "" ? Guid.Parse(cust.ID.ToString()) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = true, Records = customerObj });
            }
            catch (Exception ex)
            {
                
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion  GetCustomerDetailsByID
    }
}



       