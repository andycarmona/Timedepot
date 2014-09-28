using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity.ModelConfiguration.Conventions;
using System.Configuration;
using System.Data.Entity;
using TimelyDepotMVC.Models;
using TimelyDepotMVC.Models.Admin;

namespace TimelyDepotMVC.DAL
{
    /////////////////////////////////////////////////////////////////////
    // Name: Class/Function/Propertyname
    // Version: 1.1.0
    // Summary: 
    // If the exisitng database was not created by Code First, then it will not have the EdmMetadata table that is used to let Code First know whether or not the 
    // underlying database has changed. If you delete the exisiting database, Code First should be able to successfully recreate the database based on your new model. 
    // Please note, that any data that you currently have in the existing database will be lost. Once Code First has successfully generated the database, 
    // it will contain an EdmMetadata table, so Code First will be able to drop and recreate the database in the future without the need for any manual intervention.
    // See: http://forums.asp.net/p/1711900/4559368.aspx/1?Entity+framework+code+first+approach+recreate+update+database+on+model+change
    // Date: dd/mm/2011
    // Author: Mario G Vernaza
    // Prerequisites: System.Data.Entity; System.Data.Entity.ModelConfiguration.Conventions; System.Configuration; ViosMails.Models.Admin.Mails;
    // In global.asax.cs requiere to have the System.Data.Entity.Database.SetInitializer ... instruccion added to the  Application_Start function
    // Change History:
    // Date of change (dd/mm/yyyy) [MGV] – Description of change
    /////////////////////////////////////////////////////////////////////
    public class TimelyDepotContext : DbContext 
    {
        public string szConn = ConfigurationManager.ConnectionStrings["TimelyDepotContext"].ToString();
        //public string szConn = ConfigurationManager.ConnectionStrings["con"].ToString();

        public TimelyDepotContext()
        {
            this.Database.Connection.ConnectionString = szConn;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Do not use plurals for the table's names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //The foregin keys

        }

        //public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<webpages_Roles> webpages_Roles { get; set; }

        public DbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }

        public DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }

        public DbSet<webpages_Membership> webpages_Membership { get; set; }

        public DbSet<UserRegistration> UserRegistrations { get; set; }

        public DbSet<UserQuotation> UserQuotations { get; set; }

        public DbSet<UserQuotationDetail> UserQuotationDetails { get; set; }

        public DbSet<WorkerInfluentialPersons> WorkerInfluentialPersons { get; set; }

        public DbSet<timelydepot_log> timelydepot_log { get; set; }

        public DbSet<SUB_SPECIALITEM> SUB_SPECIALITEM { get; set; }

        public DbSet<SUB_ITEM> SUB_ITEM { get; set; }

        public DbSet<SPECIALITEM> SPECIALITEMs { get; set; }

        public DbSet<SetupChargeDetail> SetupChargeDetails { get; set; }

        public DbSet<Setup_for_Price> Setup_for_Price { get; set; }

        public DbSet<SETUP> SETUPs { get; set; }

        public DbSet<PRICE> PRICEs { get; set; }

        public DbSet<packingitemdetail> packingitemdetails { get; set; }

        public DbSet<PackagingDetail> PackagingDetails { get; set; }

        public DbSet<packagemaster> packagemasters { get; set; }

        public DbSet<PackageGroupDetail> PackageGroupDetails { get; set; }

        public DbSet<PACKAGE> PACKAGEs { get; set; }

        public DbSet<Operator> Operators { get; set; }

        public DbSet<New_Items> New_Items { get; set; }

        public DbSet<ITEM_PACKAGE> ITEM_PACKAGE { get; set; }

        public DbSet<ITEM> ITEMs { get; set; }

        public DbSet<InformationId> InformationIds { get; set; }

        public DbSet<InformationDetail> InformationDetails { get; set; }

        public DbSet<ImprintMaster> ImprintMasters { get; set; }

        public DbSet<Imprintitemdetail> Imprintitemdetails { get; set; }

        public DbSet<Group_Master> Group_Master { get; set; }

        public DbSet<EventLog> EventLogs { get; set; }

        public DbSet<DiscountManage> DiscountManages { get; set; }

        public DbSet<DIAL> DIALs { get; set; }

        public DbSet<DF> DFs { get; set; }

        public DbSet<DELIVERY> DELIVERies { get; set; }

        public DbSet<COLLECTION> COLLECTIONs { get; set; }

        public DbSet<CATEGORY> CATEGORies { get; set; }

        public DbSet<CastMaster> CastMasters { get; set; }

        public DbSet<Vendors> Vendors { get; set; }

        public DbSet<VendorsSpecialNotes> VendorsSpecialNotes { get; set; }

        public DbSet<VendorsContactAddress> VendorsContactAddresses { get; set; }

        public DbSet<VendorsBillingDept> VendorsBillingDepts { get; set; }

        public DbSet<VendorsSalesContact> VendorsSalesContacts { get; set; }

        public DbSet<VendorsHistory> VendorsHistories { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<CustomersSpecialNotes> CustomersSpecialNotes { get; set; }

        public DbSet<CustomersContactAddress> CustomersContactAddresses { get; set; }

        public DbSet<CustomersSubsidiaryAddress> CustomersSubsidiaryAddresses { get; set; }

        public DbSet<CustomersBillingDept> CustomersBillingDepts { get; set; }

        public DbSet<CustomersSalesContact> CustomersSalesContacts { get; set; }

        public DbSet<CustomersShipAddress> CustomersShipAddresses { get; set; }

        public DbSet<CustomersHistory> CustomersHistories { get; set; }

        public DbSet<CustomersCreditCardShipping> CustomersCreditCardShippings { get; set; }

        public DbSet<VendorItem> VendorItems { get; set; }

        public DbSet<Trade> Trades { get; set; }

        public DbSet<SalesOrder> SalesOrders { get; set; }

        public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }

        public DbSet<Origin> Origins { get; set; }

        public DbSet<Deptos> Deptos { get; set; }

        public DbSet<Terms> Terms { get; set; }

        public DbSet<Bussines> Bussines { get; set; }

        public DbSet<SalesOrderBlindShip> SalesOrderBlindShips { get; set; }

        public DbSet<CustomerDefaults> CustomerDefaults { get; set; }

        public DbSet<CustomersCardType> CustomersCardTypes { get; set; }

        public DbSet<VendorTypes> VendorTypes { get; set; }

        public DbSet<VendorDefaults> VendorDefaults { get; set; }

        public DbSet<InitialInfo> InitialInfoes { get; set; }

        public DbSet<Payments> Payments { get; set; }

        public DbSet<Warehouses> Warehouses { get; set; }

        public DbSet<PurchaseOrders> PurchaseOrders { get; set; }

        public DbSet<PurchasOrderDetail> PurchasOrderDetails { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public DbSet<PackageRateLog> PackageRateLogs { get; set; }

        public DbSet<PackageRateLogDetails> PackageRateLogDetails { get; set; }

        public DbSet<Parameters> Parameters { get; set; }

        public DbSet<PackageRateLogParameters> PackageRateLogParameters { get; set; }

        public DbSet<Applications> Applications { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<Memberships> Memberships { get; set; }

        public DbSet<Profiles> Profiles { get; set; }

        public DbSet<Roles> Roles { get; set; }

        public DbSet<UsersInRoles> UsersInRoles { get; set; }

        public DbSet<aspnet_Applications> aspnet_Applications { get; set; }

        public DbSet<aspnet_Users> aspnet_Users { get; set; }

        public DbSet<aspnet_Membership> aspnet_Membership { get; set; }

        public DbSet<aspnet_Roles> aspnet_Roles { get; set; }

        public DbSet<aspnet_UsersInRoles> aspnet_UsersInRoles { get; set; }

        public DbSet<SalesOrderTES> SalesOrderTES { get; set; }

        public DbSet<ClssNos> ClssNos { get; set; }

        public DbSet<YearProducts> YearProducts { get; set; }

        public DbSet<ImprintMethods> ImprintMethods { get; set; }

        public DbSet<ShipVia> ShipVias { get; set; }

        public DbSet<OrderBy> OrderBies { get; set; }

        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<ShipmentDetails> ShipmentDetails { get; set; }


    }
}