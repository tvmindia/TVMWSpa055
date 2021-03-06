﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IPaymentTermsBusiness
    {
        List<PaymentTerms> GetAllPayTerms();
        PaymentTerms GetPayTermDetails(string Code);
    }
}