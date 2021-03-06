﻿using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class DashboardBusiness : IDashboardBusiness
    {
        private IDashboardRepository _dashboardRepository;
        private ICommonBusiness _commonBusiness;
        public DashboardBusiness(IDashboardRepository dashboardRepository, ICommonBusiness commonBusiness)
        {
            _dashboardRepository = dashboardRepository;
            _commonBusiness = commonBusiness;
        }
        public MonthlyRecap GetMonthlyRecap(MonthlyRecap data)
        {
            MonthlyRecap Result = _dashboardRepository.GetMonthlyRecap(data);
            if (Result != null)
            {
                foreach (MonthlyRecapItem m in Result.MonthlyRecapItemList)
                {
                    Result.TotalIncome = Result.TotalIncome + m.INAmount;
                    Result.TotalExpense = Result.TotalExpense + m.ExAmount;
                }

                Result.TotalProfit = Result.TotalIncome - Result.TotalExpense;
                //decimal n = (Result.MonthlyRecapItemList[11].INAmount - Result.MonthlyRecapItemList[0].INAmount);
                //decimal d = Result.MonthlyRecapItemList[0].INAmount;
                //if (d == 0) { d = 1; }
                //Result.IncomePercentage = n /d * 100;

                //n = (Result.MonthlyRecapItemList[11].ExAmount - Result.MonthlyRecapItemList[0].ExAmount);
                //d = Result.MonthlyRecapItemList[0].ExAmount;
                //if (d == 0) { d = 1; }
                //Result.ExpensePercentage = n/ d * 100;

                //n = ((Result.MonthlyRecapItemList[11].INAmount - Result.MonthlyRecapItemList[11].ExAmount) - (Result.MonthlyRecapItemList[0].INAmount - Result.MonthlyRecapItemList[0].ExAmount));
                //d = (Result.MonthlyRecapItemList[0].INAmount - Result.MonthlyRecapItemList[0].ExAmount);
                //if (d == 0) { d = 1; }

                //Result.ProfitPercentage =  n/d  * 100;
                Result.Caption = Result.MonthlyRecapItemList[0].Period + "-" + Result.MonthlyRecapItemList[11].Period;

                Result.FormattedTotalIncome = _commonBusiness.ConvertCurrency(Result.TotalIncome, 2);
                Result.FormattedTotalExpense = _commonBusiness.ConvertCurrency(Result.TotalExpense, 2);
                Result.FormattedTotalProfit = _commonBusiness.ConvertCurrency(Result.TotalProfit, 2);

            }

            return Result;

        }


        public MonthlySalesPurchase GetSalesPurchase(MonthlySalesPurchase data)
        {
            MonthlySalesPurchase Result = _dashboardRepository.GetSalesPurchase(data);
            if (Result != null)
            {
                foreach (MonthlySalesPurchaseItem m in Result.MonthlyItemList)
                {
                    Result.TotalIncome = Result.TotalIncome + m.Sales;
                    Result.TotalExpense = Result.TotalExpense + m.Purchase;
                }
                Result.Caption = Result.MonthlyItemList[0].Period + "-" + Result.MonthlyItemList[11].Period;
              
            }
                
            return Result;

        }

        public TopDocs GetTopDocs(string DocType, string Company, string BaseURL, bool IsInternal)
        {
            TopDocs Result = _dashboardRepository.GetTopDocs(DocType, Company,IsInternal);
            
            if (Result != null)
            {
                foreach (TopDocsItem m in Result.DocItems)
                {
                    m.ValueFormatted = _commonBusiness.ConvertCurrency(m.Value, 2);
                    m.URL = BaseURL+m.ID;
                } 
            }

            return Result;

        }
        public List<SalesSummary> GetSalesSummaryChart(SalesSummary duration)
        {
            return _dashboardRepository.GetSalesSummaryChart(duration);
        }

    }
}