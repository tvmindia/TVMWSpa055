﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
   public interface ISupplierBusiness
    {
        List<Supplier> GetAllSuppliers();
        Supplier GetSupplierDetails(Guid ID);
        object InsertUpdateSupplier(Supplier _supplierObj, AppUA ua);
        object DeleteSupplier(Guid ID);
    }
}
