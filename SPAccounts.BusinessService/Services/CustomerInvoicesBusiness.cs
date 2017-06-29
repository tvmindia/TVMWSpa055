﻿using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class CustomerInvoicesBusiness : ICustomerInvoicesBusiness
    {
        private ICustomerInvoicesRepository _customerInvoicesRepository;
        private ICommonBusiness _commonBusiness;
        public CustomerInvoicesBusiness(ICustomerInvoicesRepository customerInvoicesRepository, ICommonBusiness commonBusiness)
        {
            _customerInvoicesRepository = customerInvoicesRepository;
            _commonBusiness = commonBusiness;

        }

        public List<CustomerInvoice> GetAllCustomerInvoices()
        {
            try
            {
                return _customerInvoicesRepository.GetAllCustomerInvoices();
            }
            catch (Exception)
            {

                throw;
            }

        }


        public CustomerInvoiceSummary GetCustomerInvoicesSummary() {
            try
            {
                CustomerInvoiceSummary result= new CustomerInvoiceSummary();
                result= _customerInvoicesRepository.GetCustomerInvoicesSummary();
                if (result != null) {

                    result.OpenAmountFormatted = _commonBusiness.ConvertCurrency(result.OpenAmount, 2);
                    result.PaidAmountFormatted = _commonBusiness.ConvertCurrency(result.PaidAmount, 2);
                    result.OverdueAmountFormatted = _commonBusiness.ConvertCurrency(result.OverdueAmount, 2);

                }

                return result;

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}