﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ISupplierInvoicesBusiness
    {
        List<SupplierInvoices> GetAllSupplierInvoices();
        SupplierInvoices GetSupplierInvoiceDetails(Guid ID);
        SupplierInvoiceSummary GetSupplierInvoicesSummary();
        SupplierInvoices InsertUpdateInvoice(SupplierInvoices _supplierInvoicesObj);
        SupplierSummaryforMobile GetOutstandingSupplierInvoices(SupplierInvoices SupObj);
        List<SupplierInvoices> GetOutStandingInvoicesBySupplier(Guid PaymentID,Guid supplierID);
        SupplierSummaryforMobile GetOpeningSupplierInvoices();
        object DeleteSupplierInvoice(Guid ID, string userName);
    }
}