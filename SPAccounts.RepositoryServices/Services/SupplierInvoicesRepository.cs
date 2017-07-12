﻿using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Services
{
    public class SupplierInvoicesRepository: ISupplierInvoicesRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SupplierInvoicesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<SupplierInvoices> GetAllSupplierInvoices()
        {
            List<SupplierInvoices> SupplierInvoicesList = null;
            Settings settings = new Settings();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[GetAllSupplierInvoices]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoicesList = new List<SupplierInvoices>();
                                while (sdr.Read())
                                {
                                    SupplierInvoices SIList = new SupplierInvoices();
                                    {
                                        SIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : SIList.ID);
                                        SIList.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : SIList.InvoiceDate);
                                        SIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        SIList.suppliersObj = new Supplier();
                                        SIList.suppliersObj.ID = Guid.Parse(sdr["SupplierID"].ToString());
                                        SIList.suppliersObj.CompanyName = sdr["CompanyName"].ToString();
                                        SIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : SIList.PaymentDueDate);
                                        SIList.TotalInvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : SIList.TotalInvoiceAmount);
                                        SIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : SIList.BalanceDue);
                                        SIList.LastPaymentDate = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()) : SIList.LastPaymentDate);
                                        SIList.Status = sdr["Status"].ToString();

                                        //------------date formatting-----------------//
                                        SIList.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.dateformat) : SIList.InvoiceDateFormatted);
                                        SIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : SIList.PaymentDueDateFormatted);
                                        SIList.LastPaymentDateFormatted = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()).ToString(settings.dateformat) : SIList.LastPaymentDateFormatted);


                                    }
                                    SupplierInvoicesList.Add(SIList);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return SupplierInvoicesList;
        }
        public SupplierInvoiceSummary GetSupplierInvoicesSummary()
        {
            SupplierInvoiceSummary SupplierInvoiceSummaryObj = null;

            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[GetSupplierInvoiceSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoiceSummaryObj = new SupplierInvoiceSummary();
                                if (sdr.Read())
                                {

                                    {
                                        SupplierInvoiceSummaryObj.OverdueAmount = (sdr["Overdueamt"].ToString() != "" ? Decimal.Parse(sdr["Overdueamt"].ToString()) : SupplierInvoiceSummaryObj.OverdueAmount);
                                        SupplierInvoiceSummaryObj.OverdueInvoices = (sdr["OverdueInvoices"].ToString() != "" ? int.Parse(sdr["OverdueInvoices"].ToString()) : SupplierInvoiceSummaryObj.OverdueInvoices);

                                        SupplierInvoiceSummaryObj.OpenAmount = (sdr["Openamt"].ToString() != "" ? Decimal.Parse(sdr["Openamt"].ToString()) : SupplierInvoiceSummaryObj.OpenAmount);
                                        SupplierInvoiceSummaryObj.OpenInvoices = (sdr["OpenInvoices"].ToString() != "" ? int.Parse(sdr["OpenInvoices"].ToString()) : SupplierInvoiceSummaryObj.OpenInvoices);

                                        SupplierInvoiceSummaryObj.PaidAmount = (sdr["Paidamt"].ToString() != "" ? Decimal.Parse(sdr["Paidamt"].ToString()) : SupplierInvoiceSummaryObj.PaidAmount);
                                        SupplierInvoiceSummaryObj.PaidInvoices = (sdr["PaidInvoices"].ToString() != "" ? int.Parse(sdr["PaidInvoices"].ToString()) : SupplierInvoiceSummaryObj.PaidInvoices);

                                    }

                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return SupplierInvoiceSummaryObj;
        }
        public SupplierInvoices GetSupplierInvoiceDetails(Guid ID)
        {
            SupplierInvoices SIList = null;
            Settings settings = new Settings();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[GetSupplierInvoiceDetails]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    SIList = new SupplierInvoices();
                                    SIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : SIList.ID);
                                    SIList.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : SIList.InvoiceDate);
                                    SIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                    SIList.companiesObj = new Companies();
                                    SIList.companiesObj.Code = (sdr["InvoiceToComanyCode"].ToString());
                                    SIList.paymentTermsObj = new PaymentTerms();
                                    SIList.paymentTermsObj.Code = (sdr["PaymentTerm"].ToString());
                                    SIList.suppliersObj = new Supplier();
                                    SIList.suppliersObj.ID = Guid.Parse(sdr["SupplierID"].ToString());
                                    SIList.suppliersObj.CompanyName = sdr["CompanyName"].ToString();
                                    SIList.BillingAddress = (sdr["BillingAddress"].ToString());
                                    SIList.GrossAmount = (sdr["GrossAmount"].ToString() != "" ? Decimal.Parse(sdr["GrossAmount"].ToString()) : SIList.GrossAmount);
                                    SIList.Discount = (sdr["Discount"].ToString() != "" ? Decimal.Parse(sdr["Discount"].ToString()) : SIList.Discount);
                                    SIList.TaxAmount = (sdr["TaxAmount"].ToString() != "" ? Decimal.Parse(sdr["TaxAmount"].ToString()) : SIList.TaxAmount);
                                    SIList.TaxPercApplied = (sdr["TaxPercApplied"].ToString() != "" ? Decimal.Parse(sdr["TaxPercApplied"].ToString()) : SIList.TaxPercApplied);
                                    SIList.Notes = (sdr["GeneralNotes"].ToString() != "" ? (sdr["GeneralNotes"].ToString()) : SIList.Notes);
                                    SIList.TaxTypeObj = new TaxTypes();
                                    SIList.TaxTypeObj.Code = sdr["TaxTypeCode"].ToString();
                                    SIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : SIList.PaymentDueDate);
                                    SIList.TotalInvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : SIList.TotalInvoiceAmount);
                                    SIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : SIList.BalanceDue);
                                    SIList.LastPaymentDate = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()) : SIList.LastPaymentDate);

                                    //------------date formatting-----------------//
                                    SIList.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.dateformat) : SIList.InvoiceDateFormatted);
                                    SIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : SIList.PaymentDueDateFormatted);
                                    SIList.LastPaymentDateFormatted = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()).ToString(settings.dateformat) : SIList.LastPaymentDateFormatted);

                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return SIList;
        }
        public SupplierInvoices InsertInvoice(SupplierInvoices _supplierInvoicesObj)
        {
            try
            {
                SqlParameter outputStatus, outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[InsertSupplierInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.CompanyCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.InvoiceNo;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierInvoicesObj.SupplierID;
                        cmd.Parameters.Add("@PaymentTerm", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.PayCode;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.InvoiceDateFormatted;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = _supplierInvoicesObj.BillingAddress;
                        cmd.Parameters.Add("@GrossAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.GrossAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = _supplierInvoicesObj.Discount;
                        cmd.Parameters.Add("@TaxTypeCode", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.TaxCode != "" ? _supplierInvoicesObj.TaxCode : null;
                        cmd.Parameters.Add("@TaxPreApplied", SqlDbType.Decimal).Value = _supplierInvoicesObj.TaxPercApplied;
                        cmd.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.TaxAmount;
                        cmd.Parameters.Add("@TotalInvoiceAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.TotalInvoiceAmount;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierInvoicesObj.Notes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _supplierInvoicesObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.commonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        _supplierInvoicesObj.ID = new Guid(outputID.Value.ToString());
                        break;
                    case "2":
                        AppConst Cobj1 = new AppConst();
                        throw new Exception(Cobj1.Duplicate);
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierInvoicesObj;
        }
        public SupplierInvoices UpdateInvoice(SupplierInvoices _supplierInvoicesObj)
        {
            try
            {
                SqlParameter outputStatus = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[UpdateSupplierInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _supplierInvoicesObj.ID;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.CompanyCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.InvoiceNo;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierInvoicesObj.SupplierID;
                        cmd.Parameters.Add("@PaymentTerm", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.PayCode;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.InvoiceDateFormatted;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = _supplierInvoicesObj.BillingAddress;
                        cmd.Parameters.Add("@GrossAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.GrossAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = _supplierInvoicesObj.Discount;
                        cmd.Parameters.Add("@TaxTypeCode", SqlDbType.VarChar, 10).Value = _supplierInvoicesObj.TaxCode != "" ? _supplierInvoicesObj.TaxCode : null;
                        cmd.Parameters.Add("@TaxPreApplied", SqlDbType.Decimal).Value = _supplierInvoicesObj.TaxPercApplied;
                        cmd.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.TaxAmount;
                        cmd.Parameters.Add("@TotalInvoiceAmount", SqlDbType.Decimal).Value = _supplierInvoicesObj.TotalInvoiceAmount;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierInvoicesObj.Notes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _supplierInvoicesObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _supplierInvoicesObj.commonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        //outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        //outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        // _customerInvoicesObj.ID = new Guid(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierInvoicesObj;
        }
    }
}