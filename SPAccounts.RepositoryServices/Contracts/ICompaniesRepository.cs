﻿using System;
using SPAccounts.DataAccessObject.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ICompaniesRepository
    {
        List<Companies> GetAllCompanies();
    }
}