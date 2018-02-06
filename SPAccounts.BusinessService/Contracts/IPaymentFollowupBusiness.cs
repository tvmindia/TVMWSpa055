﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IPaymentFollowupBusiness
    {
        List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? toDate, string filter, string company, string customer, string outstanding);
        List<FollowUp> GetFollowUpDetails(Guid customerID);
        FollowUp InsertUpdateFollowUp(FollowUp followupObj);
        FollowUp GetFollowupDetailsByFollowUpID(Guid ID);
        object DeleteFollowUp(Guid ID);
    }
}