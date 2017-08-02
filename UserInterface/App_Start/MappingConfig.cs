﻿using UserInterface.Models;
using SPAccounts.DataAccessObject.DTO;
using SAMTool.DataAccessObject.DTO;

namespace UserInterface.App_Start
{
    public class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                //domain <===== viewmodel
                //viewmodel =====> domain
                //ReverseMap() makes it possible to map both ways.


                //*****SAMTOOL MODELS 
                config.CreateMap<LoginViewModel, SAMTool.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<UserViewModel, SAMTool.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<SysMenuViewModel, SAMTool.DataAccessObject.DTO.SysMenu>().ReverseMap();
                config.CreateMap<RolesViewModel, SAMTool.DataAccessObject.DTO.Roles>().ReverseMap();
                config.CreateMap<ApplicationViewModel, SAMTool.DataAccessObject.DTO.Application>().ReverseMap();
                config.CreateMap<AppObjectViewModel, SAMTool.DataAccessObject.DTO.AppObject>().ReverseMap();
                config.CreateMap<AppSubobjectViewmodel, SAMTool.DataAccessObject.DTO.AppSubobject>().ReverseMap();
                config.CreateMap<CommonViewModel, SAMTool.DataAccessObject.DTO.Common>().ReverseMap();
                config.CreateMap<ManageAccessViewModel, SAMTool.DataAccessObject.DTO.ManageAccess>().ReverseMap();
                config.CreateMap<ManageSubObjectAccessViewModel, SAMTool.DataAccessObject.DTO.ManageSubObjectAccess > ().ReverseMap();
                config.CreateMap<PrivilegesViewModel, SAMTool.DataAccessObject.DTO.Privileges>().ReverseMap();
                
                //****SAMTOOL MODELS 




                //****SPACCOUNTS MODELS 
                config.CreateMap<MenuViewModel, Menu>().ReverseMap();
                config.CreateMap<CustomerInvoiceSummaryViewModel, CustomerInvoiceSummary>().ReverseMap();
                config.CreateMap<CustomerInvoicesViewModel, CustomerInvoice>().ReverseMap();
                config.CreateMap<CustomerViewModel, Customer>().ReverseMap();
                config.CreateMap<CommonViewModel, SPAccounts.DataAccessObject.DTO.Common>().ReverseMap();
                config.CreateMap<PaymentTermsViewModel, PaymentTerms>().ReverseMap();
                config.CreateMap<CompaniesViewModel, Companies>().ReverseMap();
                config.CreateMap<TaxTypesViewModel, TaxTypes>().ReverseMap();
                config.CreateMap<SuppliersViewModel, Supplier>().ReverseMap();
                config.CreateMap<CustomerCreditNoteViewModel, CustomerCreditNotes>().ReverseMap();
                config.CreateMap<BankViewModel, Bank>().ReverseMap();
                config.CreateMap<TaxTypesViewModel, TaxTypes>().ReverseMap();
                config.CreateMap<PaymentModesViewModel, PaymentModes>().ReverseMap();
                config.CreateMap<CustomerPaymentsViewModel, CustomerPayments>().ReverseMap();
                config.CreateMap<CustomerPaymentsDetailViewModel, CustomerPaymentsDetail>().ReverseMap(); 
                config.CreateMap<TitlesViewModel, Titles>().ReverseMap();
                config.CreateMap<SupplierInvoicesViewModel, SupplierInvoices>().ReverseMap();
                config.CreateMap<SupplierInvoiceSummaryViewModel, SupplierInvoiceSummary>().ReverseMap();
                config.CreateMap<SupplierPaymentsViewModel, SupplierPayments>().ReverseMap();
                config.CreateMap<SupplierPaymentsDetailViewModel, SupplierPaymentsDetail>().ReverseMap();

                config.CreateMap<SystemReportViewModel, SystemReport>().ReverseMap();
                config.CreateMap<SupplierCreditNoteViewModel, SupplierCreditNote>().ReverseMap();
                config.CreateMap<OtherExpenseViewModel, OtherExpense>().ReverseMap();
                config.CreateMap<EmployeeViewModel, Employee>().ReverseMap();
                config.CreateMap<DepositAndWithdrwalViewModel, DepositAndWithdrawals>().ReverseMap();
                config.CreateMap<OtherIncomeViewModel, OtherIncome>().ReverseMap();
                config.CreateMap<DashBoardViewModel, ChartOfAccounts>().ReverseMap();
                config.CreateMap<EmployeeViewModel, Employee>().ReverseMap();
                config.CreateMap<EmployeeTypeViewModel, EmployeeType>().ReverseMap();

                config.CreateMap<MonthlyRecapViewModel, MonthlyRecap>().ReverseMap();
                config.CreateMap<MonthlyRecapItemViewModel, MonthlyRecapItem>().ReverseMap();
                config.CreateMap<CustomerInvoicesSummaryForMobileViewModel, CustomerInvoicesSummaryForMobile>().ReverseMap();
                config.CreateMap<InvoiceSummaryformobileViewModel, InvoiceSummaryformobile>().ReverseMap();
                config.CreateMap<SupplierSummaryforMobileViewModel, SupplierSummaryforMobile>().ReverseMap();
                config.CreateMap<SupplierInvoiceSummaryformobileViewModel, SupplierInvoiceSummaryformobile>().ReverseMap();

                config.CreateMap<OtherExpSummaryViewModel, OtherExpSummary>().ReverseMap();
                config.CreateMap<OtherExpSummaryItemViewModel, OtherExpSummaryItem>().ReverseMap();
             
                config.CreateMap<SalesSummaryViewModel, SalesSummary>().ReverseMap();
            });
        }
    }
}