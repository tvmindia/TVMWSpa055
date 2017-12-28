﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IReportBusiness
    {
        List<SystemReport> GetAllSysReports(AppUA appUA);
        List<CustomerPaymentLedger> GetCustomerPaymentLedger(DateTime? FromDate, DateTime? ToDate, string CustomerIDs,string Company);
        List<SupplierPaymentLedger>GetSupplierPaymentLedger(DateTime? FromDate, DateTime? ToDate, string Suppliercode, string Company);
        SaleSummary GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Boolean IsTax);
        SaleDetailReport GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Boolean IsTax, Guid Customer,string InvoiceType);
        SaleDetailReport GetRPTViewCustomerDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, Guid Customer);
        List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string ReportType, string OrderBy,string accounthead, string subtype, string employeeorother,string employeecompany,string search, string ExpenseType);
        List<EmployeeExpenseSummaryReport> GetEmployeeExpenseSummary(DateTime? FDate, DateTime? TDate, string EmployeeCode, string OrderBy);
        List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string OrderBy, string accounthead, string subtype, string employeeorother, string employeecompany, string search,string ExpenseType);
        List<CustomerContactDetailsReport> GetCustomerContactDetailsReport(string search);
        List<SalesTransactionLogReport> GetSalesTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        PurchaseSummaryReport GetPurchaseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal);
        PurchaseDetailReport GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Guid Supplier,string InvoiceType,Guid EmpID,string AccountCode);
        PurchaseDetailReport GetRPTViewPurchaseDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, Guid SupplierID);
        List<SupplierContactDetailsReport> GetSupplierContactDetailsReport(string search);
        List<PurchaseTransactionLogReport> GetPurchaseTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search);
        List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string Customerids);
        List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string Customerids);
        List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string Customerids);
        List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string Customerids);
        List<AccountsPayableAgeingReport> GetAccountsPayableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string SupplierIDs);
        List<AccountsPayableAgeingSummaryReport> GetAccountsPayableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string SupplierIDs);
        List<DepositsAndWithdrawalsDetailsReport> GetDepositAndWithdrawalDetail(DateTime? FromDate, DateTime? ToDate, string BankCode, string search);
        List<OtherIncomeSummaryReport> GetOtherIncomeSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode,string accounthead, string subtype, string employeeorother, string search);
        List<OtherIncomeDetailsReport> GetOtherIncomeDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string search);
        List<DailyLedgerReport> GetDailyLedgerDetails(DateTime? FromDate, DateTime? ToDate, DateTime? Date, string MainHead, string search, string Bank);
        List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? ToDate,string Filter);
        List<SupplierExpeditingReport> GetSupplierExpeditingDetail(DateTime? ToDate, string Filter);
        List<TrialBalance> GetTrialBalanceReport(DateTime? Date);
    }
}
