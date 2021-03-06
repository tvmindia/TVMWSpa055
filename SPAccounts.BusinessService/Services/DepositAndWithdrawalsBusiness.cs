﻿using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class DepositAndWithdrawalsBusiness : IDepositAndWithdrawalsBusiness
    {
        private ICommonBusiness _commonBusiness;

        private IDepositAndWithdrawalsRepository _depositAndWithdrawalsRepository;

        public DepositAndWithdrawalsBusiness(IDepositAndWithdrawalsRepository iDepositAndWithdrawalsRepository, ICommonBusiness commonBusiness)
        {
            _depositAndWithdrawalsRepository = iDepositAndWithdrawalsRepository;
            _commonBusiness = commonBusiness;
        }

        public List<DepositAndWithdrawals> GetAllDepositAndWithdrawals(string FromDate, string ToDate, string DepositOrWithdrawal,string chqclr)
        {
            return _depositAndWithdrawalsRepository.GetAllDepositAndWithdrawals(FromDate, ToDate,DepositOrWithdrawal, chqclr);
        }

        public List<DepositAndWithdrawals> GetUndepositedCheque(UndepositedChequeAdvanceSearch undepositedChequeAdvanceSearchObject)
        {
            return _depositAndWithdrawalsRepository.GetUndepositedCheque(undepositedChequeAdvanceSearchObject);
        }
        public List<OutGoingCheques> GetOutGoingCheques(OutgoingChequeAdvanceSearch advanceSearchObject)
        {
            return _depositAndWithdrawalsRepository.GetOutGoingCheques(advanceSearchObject);
        }
        public List<IncomingCheques> GetIncomingCheques(OutgoingChequeAdvanceSearch advanceSearchObject)
        {
            return _depositAndWithdrawalsRepository.GetIncomingCheques(advanceSearchObject);
        }
        public DepositAndWithdrawals GetDepositAndWithdrawalDetails(Guid ID)
        {
            DepositAndWithdrawals depositAndWithdrawalsObj = new DepositAndWithdrawals();
            depositAndWithdrawalsObj = _depositAndWithdrawalsRepository.GetDepositAndWithdrawalDetails(ID);
            if (depositAndWithdrawalsObj != null)
            {
                depositAndWithdrawalsObj.AmountFormatted = _commonBusiness.ConvertCurrency(depositAndWithdrawalsObj.Amount, 2);

            }
            return depositAndWithdrawalsObj;
        }

        public DepositAndWithdrawals GetTransferCashById(Guid TransferId)
        {
            DepositAndWithdrawals depositAndWithdrawalsObj = new DepositAndWithdrawals();
            depositAndWithdrawalsObj = _depositAndWithdrawalsRepository.GetTransferCashById(TransferId);
            if (depositAndWithdrawalsObj != null)
            {
                depositAndWithdrawalsObj.AmountFormatted = _commonBusiness.ConvertCurrency(depositAndWithdrawalsObj.Amount, 2);

            }
            return depositAndWithdrawalsObj;
           
        }

        public object InsertUpdateTransferAmount(DepositAndWithdrawals _depositAndWithdrwalObj)
        {
            object result = null;
         
            if (_depositAndWithdrwalObj.TransferID == Guid.Empty)
            {
                result = _depositAndWithdrawalsRepository.TransferAmount(_depositAndWithdrwalObj);
            }
            else
            {
                result = _depositAndWithdrawalsRepository.UpdateTransferAmount(_depositAndWithdrwalObj);
            }
            return result; ;
        }

        public object InsertUpdateDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj)
        {
            object result = null;
            try
            {
                if (_depositAndWithdrawalsObj.CheckedRows.Count>0)
                {
                    for (int i = 0; i < _depositAndWithdrawalsObj.CheckedRows.Count; i++)
                    {
                       
                       
                        result = _depositAndWithdrawalsRepository.InsertDepositAndWithdrawals(_depositAndWithdrawalsObj.CheckedRows[i]);
                    }

                }
                else
                {
                    if (_depositAndWithdrawalsObj.ID == Guid.Empty)
                    {
                        result = _depositAndWithdrawalsRepository.InsertDepositAndWithdrawals(_depositAndWithdrawalsObj);
                    }
                    else
                    {
                        result = _depositAndWithdrawalsRepository.UpdateDepositAndWithdrawals(_depositAndWithdrawalsObj);
                    }
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public object ClearCheque(string IDS,string date)
        {
            object result = null;
            try
            {

                string[] data = IDS.Split(',');
                int len = data.Length;

                for (int i = 0; i < len; i++)
                {
                    if(data[i].Trim()!=string.Empty)
                    result = _depositAndWithdrawalsRepository.ClearCheque(Guid.Parse(data[i]), date);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string GetUndepositedChequeCount(string Date)
        {
            return _depositAndWithdrawalsRepository.GetUndepositedChequeCount(Date);
        }

        public object DeleteDepositandwithdrawal(Guid ID, string UserName)
        {
            return _depositAndWithdrawalsRepository.DeleteDepositandwithdrawal(ID,UserName);
        }
        
        public object DeleteTransferAmount(Guid TransferID, string UserName)
        {
            return _depositAndWithdrawalsRepository.DeleteTransferAmount(TransferID, UserName);
        }


        public object InsertUpdateOutgoingCheque(OutGoingCheques outGoingChequeObj)
        {
            object result = null;

            if (outGoingChequeObj.ID == Guid.Empty)
            {
                result = _depositAndWithdrawalsRepository.InsertOutgoingCheques(outGoingChequeObj);
            }
            else
            {
                result = _depositAndWithdrawalsRepository.UpdateOutgoingCheques(outGoingChequeObj);
            }
            return result; ;
        }


        public object InsertUpdateIncomingCheque(IncomingCheques incomingChequeObj)
        {
            object result = null;

            if (incomingChequeObj.ID == Guid.Empty)
            {
                result = _depositAndWithdrawalsRepository.InsertIncomingCheques(incomingChequeObj);
            }
            else
            {
                result = _depositAndWithdrawalsRepository.UpdateIncomingCheques(incomingChequeObj);
            }
            return result; ;
        }

        public object DeleteOutgoingCheque(Guid ID)
        {
            return _depositAndWithdrawalsRepository.DeleteOutgoingCheque(ID);
        }

        public object DeleteIncomingCheque(Guid ID)
        {
            return _depositAndWithdrawalsRepository.DeleteIncomingCheque(ID);
        }

        public OutGoingCheques GetOutgoingChequeById(Guid ID)
        {
            OutGoingCheques outGoingChequesObj = new OutGoingCheques();
            outGoingChequesObj = _depositAndWithdrawalsRepository.GetOutgoingChequeById(ID);
            return outGoingChequesObj;

        }

        public IncomingCheques GetIncomingChequeById(Guid ID)
        {
            IncomingCheques incomingChequesObj = new IncomingCheques();
            incomingChequesObj = _depositAndWithdrawalsRepository.GetIncomingChequeById(ID);
            return incomingChequesObj;

        }

        public object ClearChequeOut(string ID, string Date)
        {
            object result = null;
            try
            {
                string[] data = ID.Split(',');
                int len = data.Length;
                for (int i = 0; i < len; i++)
                {
                    if (data[i].Trim() != string.Empty)
                        result = _depositAndWithdrawalsRepository.ClearChequeOut(Guid.Parse(data[i]), Date);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        
        public List<DepositAndWithdrawals> GetAllWithdrawals()
        {
            return _depositAndWithdrawalsRepository.GetAllWithdrawals();
        }

        public object ValidateChequeNo(OutGoingCheques outGoingChequeObj)
        {
            return _depositAndWithdrawalsRepository.ValidateChequeNo(outGoingChequeObj);
        }

        public object ValidateChequeNoIncomingCheque(IncomingCheques incomingChequeObj)
        {
            return _depositAndWithdrawalsRepository.ValidateChequeNoIncomingCheque(incomingChequeObj);
        }

    }
}