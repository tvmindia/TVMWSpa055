﻿using System.Web.Optimization;

namespace UserInterface.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/boot").Include("~/Content/bootstrap.css", "~/Content/bootstrap-theme.css", "~/Content/font-awesome.min.css", "~/Content/Custom.css", "~/Content/sweetalert.css", "~/Content/Custom.css", "~/Content/sweetalert.css"));
            bundles.Add(new StyleBundle("~/Content/AdminLTE/css/plugins").Include("~/Content/AdminLTE/css/jvectormap/jquery-jvectormap-1.2.2.css", "~/Content/AdminLTE/css/AdminLTE.min.css", "~/Content/AdminLTE/css/skins/_all-skins.min.css"));
            //bundles.Add(new StyleBundle("~/AdminLTE/bootstrap/css/plugins").Include("~/AdminLTE/plugins/jvectormap/jquery-jvectormap-1.2.2.css", "~/AdminLTE/dist/css/AdminLTE.min.css", "~/AdminLTE/dist/css/skins/_all-skins.min.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrapdatepicker").Include("~/Content/bootstrap-datepicker3.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatable").Include("~/Content/DataTables/css/dataTables.bootstrap.min.css", "~/Content/DataTables/css/responsive.bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatablecheckbox").Include("~/Content/DataTables/css/dataTables.checkboxes.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableSelect").Include("~/Content/DataTables/css/select.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableButtons").Include("~/Content/DataTables/css/buttons.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableFixedColumns").Include("~/Content/DataTables/css/fixedColumns.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableFixedHeader").Include("~/Content/DataTables/css/fixedHeader.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/MvcDatalist/Datalist").Include("~/Content/MvcDatalist/mvc-datalist.css"));
            bundles.Add(new StyleBundle("~/Content/css/select2").Include("~/Content/css/select2.min.css"));


            //-------------------
            bundles.Add(new StyleBundle("~/Content/UserCSS/Login").Include("~/Content/UserCSS/Login.css"));


            //---------------------
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.1.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include("~/Scripts/jquery.form.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include("~/Scripts/AdminLTE/fastclick.min.js", "~/Scripts/AdminLTE/adminlte.min.js", "~/Scripts/AdminLTE/jquery.sparkline.min.js", "~/Scripts/AdminLTE/jquery.slimscroll.min.js", "~/Scripts/AdminLTE/Chart.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTEDash").Include("~/Scripts/AdminLTE/dashboard2.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusiveajaxvalidate").Include("~/Scripts/jquery.validate.min.js", "~/Scripts/jquery.validate.unobtrusive.min.js", "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include("~/Scripts/DataTables/jquery.dataTables.min.js", "~/Scripts/DataTables/dataTables.bootstrap.min.js", "~/Scripts/DataTables/dataTables.responsive.min.js", "~/Scripts/DataTables/responsive.bootstrap.min.js", "~/Scripts/DataTables/dataTables.fixedHeader.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableSelect").Include("~/Scripts/DataTables/dataTables.select.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatablecheckbox").Include("~/Scripts/DataTables/dataTables.checkboxes.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableButtons").Include("~/Scripts/DataTables/dataTables.buttons.min.js", "~/Scripts/DataTables/buttons.flash.min.js", "~/Scripts/DataTables/buttons.html5.min.js", "~/Scripts/DataTables/buttons.print.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableFixedColumns").Include("~/Scripts/DataTables/dataTables.fixedColumns.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jsZip").Include("~/Scripts/jszip.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/userpluginjs").Include("~/Scripts/jquery.noty.packaged.min.js", "~/Scripts/custom.js", "~/Scripts/Chart.js", "~/Scripts/sweetalert.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapdatepicker").Include("~/Scripts/bootstrap-datepicker.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/MvcDatalist/DataList").Include("~/Scripts/MvcDatalist/mvc-datalist.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include("~/Scripts/select2.min.js"));


            //----------------------
            bundles.Add(new ScriptBundle("~/bundles/ManageAccess").Include("~/Scripts/UserJS/ManageAccess.js"));
            bundles.Add(new ScriptBundle("~/bundles/ManageSubObjectAccess").Include("~/Scripts/UserJS/ManageSubObjectAccess.js"));
            bundles.Add(new ScriptBundle("~/bundles/Login").Include("~/Scripts/UserJS/Login.js"));
            bundles.Add(new ScriptBundle("~/bundles/User").Include("~/Scripts/UserJS/User.js"));
            bundles.Add(new ScriptBundle("~/bundles/Privileges").Include("~/Scripts/UserJS/Privileges.js"));
            bundles.Add(new ScriptBundle("~/bundles/PrivilegesView").Include("~/Scripts/UserJS/PrivilegesView.js"));
            bundles.Add(new ScriptBundle("~/bundles/Application").Include("~/Scripts/UserJS/Application.js"));
            bundles.Add(new ScriptBundle("~/bundles/AppObject").Include("~/Scripts/UserJS/AppObject.js"));
            bundles.Add(new ScriptBundle("~/bundles/AppSubobject").Include("~/Scripts/UserJS/AppSubobject.js"));
            bundles.Add(new ScriptBundle("~/bundles/Roles").Include("~/Scripts/UserJS/Roles.js"));

            //----------------------
            bundles.Add(new ScriptBundle("~/bundles/CustomerInvoices").Include("~/Scripts/UserJS/CustomerInvoices.js"));
            bundles.Add(new ScriptBundle("~/bundles/Customers").Include("~/Scripts/UserJS/Customers.js"));
            bundles.Add(new ScriptBundle("~/bundles/Suppliers").Include("~/Scripts/UserJS/Suppliers.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerCreditNote").Include("~/Scripts/UserJS/CustomerCreditNote.js"));
            bundles.Add(new ScriptBundle("~/bundles/Bank").Include("~/Scripts/UserJS/Bank.js"));
            bundles.Add(new ScriptBundle("~/bundles/Company").Include("~/Scripts/UserJS/Company.js"));
            bundles.Add(new ScriptBundle("~/bundles/TaxTypes").Include("~/Scripts/UserJS/TaxTypes.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerPayments").Include("~/Scripts/UserJS/CustomerPayments.js"));

            bundles.Add(new ScriptBundle("~/bundles/SpecialInvPayments").Include("~/Scripts/UserJS/SpecialInvPayments.js"));

            bundles.Add(new ScriptBundle("~/bundles/SupplierPayments").Include("~/Scripts/UserJS/SupplierPayments.js"));
            bundles.Add(new ScriptBundle("~/bundles/SupplierInvoices").Include("~/Scripts/UserJS/SupplierInvoices.js"));
            bundles.Add(new ScriptBundle("~/bundles/SupplierCreditNotes").Include("~/Scripts/UserJS/SupplierCreditNotes.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherIncome").Include("~/Scripts/UserJS/OtherIncome.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherExpense").Include("~/Scripts/UserJS/OtherExpense.js"));
            bundles.Add(new ScriptBundle("~/bundles/MonthlyRecap").Include("~/Scripts/UserJS/MonthlyRecap.js"));
            bundles.Add(new ScriptBundle("~/bundles/ExpenseSummary").Include("~/Scripts/UserJS/ExpenseSummary.js"));
            bundles.Add(new ScriptBundle("~/bundles/Outstanding").Include("~/Scripts/UserJS/Outstanding.js"));
            bundles.Add(new ScriptBundle("~/bundles/TopCustomers").Include("~/Scripts/UserJS/TopCustomers.js"));
            bundles.Add(new ScriptBundle("~/bundles/TopSuppliers").Include("~/Scripts/UserJS/TopSuppliers.js"));
            bundles.Add(new ScriptBundle("~/bundles/Employee").Include("~/Scripts/UserJS/Employee.js"));
            bundles.Add(new ScriptBundle("~/bundles/BankBalance").Include("~/Scripts/UserJS/BankBalance.js"));
            bundles.Add(new ScriptBundle("~/bundles/MonthlySalesPurchase").Include("~/Scripts/UserJS/MonthlySalesPurchase.js"));


            bundles.Add(new ScriptBundle("~/bundles/AdminDash").Include("~/Scripts/UserJS/MonthlyRecap.js", "~/Scripts/UserJS/MonthlySalesPurchase.js", "~/Scripts/UserJS/ExpenseSummary.js",  "~/Scripts/UserJS/Outstanding.js", "~/Scripts/UserJS/TopCustomers.js", "~/Scripts/UserJS/TopSuppliers.js", "~/Scripts/UserJS/Dashboard.js"));
            bundles.Add(new ScriptBundle("~/bundles/SaleSummaryReport").Include("~/Scripts/UserJS/SaleSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/SalesDetailReport").Include("~/Scripts/UserJS/SalesDetailReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherExpenseSummaryReport").Include("~/Scripts/UserJS/OtherExpenseSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherExpenseDetailsReport").Include("~/Scripts/UserJS/OtherExpenseDetailsReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/DepositAndWithdrawalDetailReport").Include("~/Scripts/UserJS/DepositAndWithdrawalDetailReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerContactDetailsReport").Include("~/Scripts/UserJS/CustomerContactDetailsReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/SalesTransactionLogReport").Include("~/Scripts/UserJS/SalesTransactionLogReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/PurchaseSummaryReport").Include("~/Scripts/UserJS/PurchaseSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/PurchaseDetailReport").Include("~/Scripts/UserJS/PurchaseDetailReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/PurchaseTransactionLogReport").Include("~/Scripts/UserJS/PurchaseTransactionLogReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/SupplierContactDetailsReport").Include("~/Scripts/UserJS/SupplierContactDetailsReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/AccountReceivableAgeingReport").Include("~/Scripts/UserJS/AccountReceivableAgeingReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/AccountReceivableAgeingSummaryReport").Include("~/Scripts/UserJS/AccountReceivableAgeingSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/AccountPayableAgeingReport").Include("~/Scripts/UserJS/AccountPayableAgeingReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/AccountPayableAgeingSummaryReport").Include("~/Scripts/UserJS/AccountPayableAgeingSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherIncomeSummaryReport").Include("~/Scripts/UserJS/OtherIncomeSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherIncomeDetailsReport").Include("~/Scripts/UserJS/OtherIncomeDetailsReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/DailyLedgerReport").Include("~/Scripts/UserJS/DailyLedgerReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/TrialBalanceReport").Include("~/Scripts/UserJS/TrialBalanceReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/BankLedgerReport").Include("~/Scripts/UserJS/BankLedgerReport.js"));

            bundles.Add(new ScriptBundle("~/bundles/UndepositedCheque").Include("~/Scripts/UserJS/UndepositedCheque.js"));
            bundles.Add(new ScriptBundle("~/bundles/DepositAndWithdrawals").Include("~/Scripts/UserJS/DepositAndWithdrawals.js"));
            bundles.Add(new ScriptBundle("~/bundles/ChartOfAccounts").Include("~/Scripts/UserJS/ChartOfAccounts.js"));
            bundles.Add(new ScriptBundle("~/bundles/SubType").Include("~/Scripts/UserJS/SubType.js"));
            bundles.Add(new ScriptBundle("~/bundles/Department").Include("~/Scripts/UserJS/Department.js"));
            bundles.Add(new ScriptBundle("~/bundles/EmployeeCategory").Include("~/Scripts/UserJS/EmployeeCategory.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerPaymentLedger").Include("~/Scripts/UserJS/CustomerPaymentLedgerReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/SupplierPaymentLedger").Include("~/Scripts/UserJS/SupplierPaymentLedgerReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerExpeditingReport").Include("~/Scripts/UserJS/CustomerExpeditingDetailsReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/SupplierExpeditingReport").Include("~/Scripts/UserJS/SupplierExpeditingDetailsReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/OutgoingCheque").Include("~/Scripts/UserJS/OutgoingCheque.js"));
            bundles.Add(new ScriptBundle("~/bundles/ImportOtherExpenses").Include("~/Scripts/UserJS/ImportOtherExpenses.js"));
            bundles.Add(new ScriptBundle("~/bundles/IncomingCheque").Include("~/Scripts/UserJS/IncomingCheque.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerInvoiceRegister").Include("~/Scripts/UserJS/CustomerInvoiceRegister.js"));
            bundles.Add(new ScriptBundle("~/bundles/SupplierInvoiceRegister").Include("~/Scripts/UserJS/SupplierInvoiceRegisterReport.js"));

            bundles.Add(new ScriptBundle("~/bundles/PaymentFollowup").Include("~/Scripts/UserJS/PaymentFollowup.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherExpenseLimitedDetailsReport").Include("~/Scripts/UserJS/OtherExpenseLimitedDetailsReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/PaymentFollowupsReport").Include("~/Scripts/UserJS/PaymentFollowupsReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/AccountHeadGroup").Include("~/Scripts/UserJS/AccountHeadGroup.js"));
            bundles.Add(new ScriptBundle("~/bundles/AccountHeadGroupSummaryReport").Include("~/Scripts/UserJS/AccountHeadGroupSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/AccountHeadGroupDetailReport").Include("~/Scripts/UserJS/AccountHeadGroupDetailReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/MonthWiseIncomeExpenseSummary").Include("~/Scripts/UserJS/MonthWiseIncomeExpenseSummary.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerOutStanding").Include("~/Scripts/UserJS/CustomerOutStandingReport.js"));

        }

    }
            
        }

    
