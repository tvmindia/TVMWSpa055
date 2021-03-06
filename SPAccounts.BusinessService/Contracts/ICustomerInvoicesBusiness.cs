﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ICustomerInvoicesBusiness
    {
        List<CustomerInvoice> GetAllCustomerInvociesByCustomerID(Guid CustomerID);
        CustomerInvoice GetCustomerInvoiceDetails(Guid ID);
        CustomerInvoice InsertUpdateInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua);
        List<CustomerInvoice> GetOutStandingInvoices(Guid PaymentID,Guid CustID);
        CustomerInvoice GetCustomerAdvances(string ID);
        object DeleteInvoices(Guid ID, string UserName);
        CustomerInvoicesSummaryForMobile GetOutstandingCustomerInvoices(CustomerInvoice CusObj);
        CustomerInvoicesSummaryForMobile GetOpeningCustomerInvoices(CustomerInvoice CusObj);
        CustomerInvoicesSummaryForMobile GetCustomerInvoicesByDateWise(CustomerInvoice CusmObj);

        //special Permission SAdmin and CEO case
        List<CustomerInvoice> GetAllCustomerInvoicesForSA(DateTime? FromDate, DateTime? ToDate, string Customer, string InvoiceType, string Company, string Status, string Search,bool PBAccess);
        CustomerInvoiceSummary GetCustomerInvoicesSummaryForSA();
        //SpecialPayments
        
        CustomerInvoice SpecialPaymentSummary(Guid InvoiceID); 
        List<CustomerInvoice> GetAllSpecialPayments(Guid InvoiceID);
        CustomerInvoice GetSpecialPaymentsDetails(Guid ID);
        CustomerInvoice InsertUpdateSpecialPayments(CustomerInvoice _customerInvoicesObj, AppUA ua);
        object DeleteSpecialPayments(Guid ID);
        CustomerInvoiceAgeingSummary GetCustomerInvoicesAgeingSummary();



    }
}
