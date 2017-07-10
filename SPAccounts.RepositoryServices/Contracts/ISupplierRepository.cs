﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
   public interface ISupplierRepository
    {
        List<Supplier> GetAllSuppliers();
        Supplier GetSupplierDetails(Guid ID);
        Supplier InsertSupplier(Supplier _supplierObj, AppUA ua);
        object UpdateSupplier(Supplier _supplierObj, AppUA ua);
        object DeleteSupplier(Guid ID);
    }
}
