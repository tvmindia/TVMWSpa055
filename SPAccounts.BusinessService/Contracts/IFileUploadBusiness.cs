﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IFileUploadBusiness
    {
        FileUpload InsertAttachment(FileUpload fileUploadObj);
        List<FileUpload> GetAttachments(Guid ID);
        object DeleteFile(Guid ID);
    }
}