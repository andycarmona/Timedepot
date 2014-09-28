namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalStageFroLocalDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationId = c.Guid(nullable: false),
                        ApplicationName = c.String(maxLength: 235),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ApplicationId);
            
            CreateTable(
                "dbo.aspnet_Applications",
                c => new
                    {
                        ApplicationId = c.Guid(nullable: false),
                        ApplicationName = c.String(maxLength: 256),
                        LoweredApplicationName = c.String(maxLength: 256),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ApplicationId);
            
            CreateTable(
                "dbo.aspnet_Membership",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        Password = c.String(maxLength: 128),
                        PasswordFormat = c.Int(nullable: false),
                        PasswordSalt = c.String(maxLength: 128),
                        MobilePIN = c.String(maxLength: 16),
                        Email = c.String(maxLength: 256),
                        LoweredEmail = c.String(maxLength: 256),
                        PasswordQuestion = c.String(maxLength: 256),
                        PasswordAnswer = c.String(maxLength: 128),
                        IsApproved = c.Boolean(nullable: false),
                        IsLockedOut = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                        LastPasswordChangedDate = c.DateTime(nullable: false),
                        LastLockoutDate = c.DateTime(nullable: false),
                        FailedPasswordAttemptCount = c.Int(nullable: false),
                        FailedPasswordAttemptWindowStart = c.DateTime(nullable: false),
                        FailedPasswordAnswerAttemptCount = c.Int(nullable: false),
                        FailedPasswordAnswerAttemptWindowStart = c.DateTime(nullable: false),
                        Comment = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.aspnet_Roles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        RoleName = c.String(maxLength: 256),
                        LoweredRoleName = c.String(maxLength: 256),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.aspnet_Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        UserName = c.String(maxLength: 50),
                        LoweredUserName = c.String(maxLength: 50),
                        MobileAlias = c.String(maxLength: 50),
                        IsAnonymous = c.Boolean(nullable: false),
                        LastActivityDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.aspnet_UsersInRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId });
            
            CreateTable(
                "dbo.Bussines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BussinesType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CastMaster",
                c => new
                    {
                        CastId = c.Int(nullable: false, identity: true),
                        CastName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CastId);
            
            CreateTable(
                "dbo.CATEGORY",
                c => new
                    {
                        CatID = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CatID);
            
            CreateTable(
                "dbo.ClssNos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClssNo = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.COLLECTION",
                c => new
                    {
                        ColID = c.String(nullable: false, maxLength: 50),
                        CatID = c.String(maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ColID);
            
            CreateTable(
                "dbo.CustomerDefaults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        SalesContactId = c.Int(),
                        SalesName = c.String(),
                        SubsidiaryId = c.Int(),
                        SubsidiaryName = c.String(),
                        ShiptoAddressId = c.Int(),
                        ShiptoName = c.String(),
                        NoteId = c.Int(),
                        NoteName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerNo = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        BussinesType = c.String(),
                        SalesPerson = c.String(),
                        DeptoNo = c.String(),
                        SellerPermintNo = c.String(),
                        ASINo = c.String(),
                        PPAINo = c.String(),
                        SageNo = c.String(),
                        Origin = c.String(),
                        CreditLimit = c.Decimal(precision: 18, scale: 2),
                        PaymentTerms = c.String(),
                        BussinesSice = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomersBillingDept",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomersCardType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomersContactAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        CompanyName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Website = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomersCreditCardShipping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        Name = c.String(),
                        CreditNumber = c.String(),
                        CardType = c.String(),
                        SecureCode = c.String(),
                        ExpirationDate = c.DateTime(),
                        Tel = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomersHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        OutstandingBalance = c.Decimal(precision: 18, scale: 2),
                        OpenPurchaseOrder = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomersSalesContact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomersShipAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Address1 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Email = c.String(),
                        Address2 = c.String(),
                        ShipperAccount = c.String(),
                        ShippingPreference = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomersSpecialNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        SpecialNote = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomersSubsidiaryAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        CompanyName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Website = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DELIVERY",
                c => new
                    {
                        DeliveryID = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.DeliveryID);
            
            CreateTable(
                "dbo.Deptos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeptoNo = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DF",
                c => new
                    {
                        ItemID = c.String(nullable: false, maxLength: 255),
                        DialType = c.String(maxLength: 50),
                        DescA = c.String(maxLength: 255),
                        CollectionID = c.String(maxLength: 50),
                        Start_Qty = c.String(maxLength: 50),
                        Field11 = c.String(maxLength: 255),
                        Field12 = c.String(maxLength: 255),
                        Field13 = c.String(maxLength: 255),
                        Field14 = c.String(maxLength: 255),
                        Field15 = c.String(maxLength: 255),
                        Field16 = c.Decimal(precision: 18, scale: 2),
                        Field17 = c.Decimal(precision: 18, scale: 2),
                        Field18 = c.Decimal(precision: 18, scale: 2),
                        Field19 = c.Decimal(precision: 18, scale: 2),
                        Field20 = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ItemID);
            
            CreateTable(
                "dbo.DIAL",
                c => new
                    {
                        DialID = c.String(nullable: false, maxLength: 50),
                        DialPic = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DialID);
            
            CreateTable(
                "dbo.DiscountManage",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        DiscountName = c.String(maxLength: 60),
                        DiscountPercentage = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.EventLog",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EventLog1 = c.String(maxLength: 50),
                        IP = c.String(maxLength: 50),
                        User = c.String(maxLength: 50),
                        EventTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Group_Master",
                c => new
                    {
                        Group_Id = c.Int(nullable: false, identity: true),
                        Group_Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Group_Id);
            
            CreateTable(
                "dbo.Imprintitemdetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        printId = c.Int(),
                        itemId = c.String(maxLength: 30),
                        RowNo = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ImprintMaster",
                c => new
                    {
                        ImprintId = c.Int(nullable: false, identity: true),
                        ImprintName = c.String(maxLength: 100),
                        SetUpCharge = c.Decimal(precision: 18, scale: 2),
                        SetUpCharge2 = c.Decimal(precision: 18, scale: 2),
                        RunCharge = c.Decimal(precision: 18, scale: 2),
                        ColorCharge = c.Decimal(precision: 18, scale: 2),
                        Displayname = c.String(maxLength: 100),
                        DiscountCode = c.String(maxLength: 100),
                        ImagePath = c.String(maxLength: 100),
                        NumberColor = c.Int(),
                        Information = c.String(maxLength: 500),
                        RunChargeInclude = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.ImprintId);
            
            CreateTable(
                "dbo.ImprintMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InformationDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InformationId = c.Int(),
                        ItemId = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InformationId",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InformationId1 = c.String(maxLength: 20),
                        Description = c.String(maxLength: 800),
                        Priceinformation = c.String(maxLength: 200),
                        ProductionTime = c.String(),
                        Artwork = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InitialInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SalesOrderNo = c.Int(nullable: false),
                        InvoiceNo = c.Int(nullable: false),
                        PaymentNo = c.Int(nullable: false),
                        PurchaseOrderNo = c.Int(nullable: false),
                        TaxRate = c.Double(nullable: false),
                        PaymentAccount = c.String(),
                        ShipperInfo = c.String(),
                        TrackingNo = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvoiceDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(),
                        Quantity = c.Double(),
                        ShipQuantity = c.Double(),
                        BackOrderQuantity = c.Double(),
                        ItemID = c.String(),
                        Sub_ItemID = c.String(),
                        Description = c.String(),
                        Tax = c.Double(),
                        UnitPrice = c.Decimal(precision: 18, scale: 2),
                        ItemPosition = c.Int(),
                        ItemOrder = c.Double(),
                        Logo = c.String(),
                        ImprintMethod = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        InvoiceNo = c.String(),
                        SalesOrderId = c.Int(),
                        SalesOrderNo = c.String(),
                        InvoiceDate = c.DateTime(),
                        ShipVia = c.String(),
                        ShipDate = c.DateTime(),
                        TradeId = c.Int(),
                        CustomerId = c.Int(nullable: false),
                        CustomerShiptoId = c.Int(),
                        CustomerShipLocation = c.String(),
                        VendorId = c.Int(),
                        BussinesType = c.String(),
                        VendorAddress = c.String(),
                        PurchaseOrderNo = c.String(),
                        PaymentTerms = c.String(),
                        PaymentDate = c.DateTime(),
                        PaymentAmount = c.Decimal(precision: 18, scale: 2),
                        ShippingHandling = c.Decimal(precision: 18, scale: 2),
                        CreaditCardNo = c.String(),
                        Note = c.String(),
                        IsBlindShip = c.Boolean(nullable: false),
                        SalesRep = c.String(),
                        TrackingNo = c.String(),
                        Tax_rate = c.Decimal(precision: 18, scale: 2),
                        Invs_Tax = c.Decimal(precision: 18, scale: 2),
                        Warehouse = c.String(),
                        FromName = c.String(),
                        FromTitle = c.String(),
                        FromCompany = c.String(),
                        FromAddress1 = c.String(),
                        FromAddress2 = c.String(),
                        FromCity = c.String(),
                        FromState = c.String(),
                        FromZip = c.String(),
                        FromCountry = c.String(),
                        FromEmail = c.String(),
                        FromTel = c.String(),
                        FromFax = c.String(),
                        ToName = c.String(),
                        ToTitle = c.String(),
                        ToCompany = c.String(),
                        ToAddress1 = c.String(),
                        ToAddress2 = c.String(),
                        ToCity = c.String(),
                        ToState = c.String(),
                        ToZip = c.String(),
                        ToCountry = c.String(),
                        ToEmail = c.String(),
                        ToTel = c.String(),
                        ToFax = c.String(),
                    })
                .PrimaryKey(t => t.InvoiceId);
            
            CreateTable(
                "dbo.ITEM_PACKAGE",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item = c.String(maxLength: 50),
                        Package = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ITEM",
                c => new
                    {
                        ItemID = c.String(nullable: false, maxLength: 50),
                        PicID = c.String(maxLength: 50),
                        DialType = c.String(maxLength: 50),
                        DescA = c.String(),
                        DescB = c.String(),
                        CollectionID = c.String(maxLength: 50),
                        Price_ID = c.String(maxLength: 50),
                        Misc_ID = c.String(maxLength: 50),
                        Inactive = c.String(maxLength: 1),
                        Keywords = c.String(),
                        Special = c.String(maxLength: 1),
                        New = c.String(maxLength: 1),
                        title = c.String(maxLength: 600),
                        UnitPerCase = c.String(),
                        UnitWeight = c.String(),
                        CaseWeight = c.String(),
                        DimensionH = c.Int(),
                        DimensionL = c.Int(),
                        DimensionD = c.Int(),
                        Status = c.Boolean(nullable: false),
                        Pic2ID = c.String(),
                        Pic3ID = c.String(),
                        DeptoNo = c.String(),
                        YearProduct = c.String(),
                        ClassNo = c.String(),
                        UPCCode = c.String(),
                        CaseDimensionL = c.String(),
                        CaseDimensionW = c.String(),
                        CaseDimensionH = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.ItemID);
            
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        Password = c.String(maxLength: 128),
                        PasswordFormat = c.Int(nullable: false),
                        PasswordSalt = c.String(maxLength: 128),
                        Email = c.String(maxLength: 256),
                        PasswordQuestion = c.String(maxLength: 256),
                        PasswordAnswer = c.String(maxLength: 128),
                        IsApproved = c.Boolean(nullable: false),
                        IsLockedOut = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                        LastPasswordChangedDate = c.DateTime(nullable: false),
                        LastLockoutDate = c.DateTime(nullable: false),
                        FailedPasswordAttemptCount = c.Int(nullable: false),
                        FailedPasswordAttemptWindowStart = c.DateTime(nullable: false),
                        FailedPasswordAnswerAttemptCount = c.Int(nullable: false),
                        FailedPasswordAnswerAttemptWindowsStart = c.DateTime(nullable: false),
                        Comment = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.New_Items",
                c => new
                    {
                        ITEMID = c.String(nullable: false, maxLength: 255),
                        SUB_ITEMID = c.String(maxLength: 255),
                        Desca = c.String(maxLength: 255),
                        DescB = c.String(maxLength: 255),
                        Colors_available = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ITEMID);
            
            CreateTable(
                "dbo.Operator",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 50),
                        Password = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.OrderBy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Origin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PackageGroupDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(maxLength: 100),
                        PAckageId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.packagemaster",
                c => new
                    {
                        PackId = c.Int(nullable: false, identity: true),
                        PackName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PackId);
            
            CreateTable(
                "dbo.PackageRateLogDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRateLog = c.Int(),
                        BoxNo = c.String(),
                        Quantity = c.String(),
                        Dimensions = c.String(),
                        DimensionsUnits = c.String(),
                        WeigthUnits = c.String(),
                        DeclaredValue = c.String(),
                        RequestCode = c.String(),
                        PackageTypeCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PackageRateLogParameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRateLog = c.Int(),
                        Parameter = c.String(),
                        ParameterValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PackageRateLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateSubmit = c.DateTime(),
                        ItemId = c.String(),
                        Quantity = c.String(),
                        PostalZipCode = c.String(),
                        Boxes = c.String(),
                        ItemsLastBox = c.String(),
                        FullBoxWeight = c.String(),
                        PartialBoxWeight = c.String(),
                        ValueperFullBox = c.String(),
                        ValueperPartialBox = c.String(),
                        CaseHeight = c.String(),
                        CaseLenght = c.String(),
                        CaseWidth = c.String(),
                        CaseWeight = c.String(),
                        UnitPrice = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PACKAGE",
                c => new
                    {
                        PackageID = c.String(nullable: false, maxLength: 50),
                        Weight = c.String(maxLength: 50),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(),
                        ImagePath = c.String(maxLength: 500),
                        DiscountCode = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.PackageID);
            
            CreateTable(
                "dbo.PackagingDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackId = c.Int(),
                        ItemNo = c.String(maxLength: 100),
                        DisplayName = c.String(maxLength: 100),
                        Price = c.Decimal(precision: 18, scale: 2),
                        ImagePath = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.packingitemdetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackId = c.String(maxLength: 100),
                        itemId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Parameter = c.String(),
                        ParameterValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentNo = c.String(),
                        CustomerNo = c.String(),
                        SalesOrderNo = c.String(),
                        PaymentType = c.String(),
                        CreditCardNumber = c.String(),
                        ReferenceNo = c.String(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        PaymentDate = c.DateTime(),
                        PayLog = c.String(),
                        InvoicePayment = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PRICE",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item = c.String(maxLength: 50),
                        Qty = c.Short(nullable: false),
                        PriceType = c.String(maxLength: 50),
                        thePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount_Code = c.String(maxLength: 50),
                        Description = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        PropertyNames = c.String(nullable: false, maxLength: 4000),
                        PropertyValueStrings = c.String(nullable: false, maxLength: 4000),
                        PropertyValueBinary = c.String(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        PurchaseOrderId = c.Int(nullable: false, identity: true),
                        PurchaseOrderNo = c.String(),
                        SalesOrderNo = c.String(),
                        TradeId = c.Int(),
                        PODate = c.DateTime(),
                        VendorId = c.String(),
                        ShipDate = c.DateTime(),
                        PaidBy = c.String(),
                        BlindDrop = c.String(),
                        Logo = c.String(),
                        ImprintColor = c.String(),
                        PurchaseOrderReference = c.String(),
                        IsBlindShip = c.Boolean(nullable: false),
                        FromName = c.String(),
                        FromTitle = c.String(),
                        FromCompany = c.String(),
                        FromAddress1 = c.String(),
                        FromAddress2 = c.String(),
                        FromCity = c.String(),
                        FromState = c.String(),
                        FromZip = c.String(),
                        FromCountry = c.String(),
                        FromEmail = c.String(),
                        FromTel = c.String(),
                        FromFax = c.String(),
                        ToName = c.String(),
                        ToTitle = c.String(),
                        ToCompany = c.String(),
                        ToAddress1 = c.String(),
                        ToAddress2 = c.String(),
                        ToCity = c.String(),
                        ToState = c.String(),
                        ToZip = c.String(),
                        ToCountry = c.String(),
                        ToEmail = c.String(),
                        ToTel = c.String(),
                        ToFax = c.String(),
                        ReceiveStatus = c.String(),
                        RequiredDate = c.DateTime(),
                        Invoice = c.String(),
                        TrackingNo = c.String(),
                        Terms = c.String(),
                        OrderBy = c.String(),
                        Warehouse = c.String(),
                        Billto = c.String(),
                        ShipVia = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseOrderId);
            
            CreateTable(
                "dbo.PurchasOrderDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(),
                        ItemID = c.String(),
                        Sub_ItemID = c.String(),
                        Description = c.String(),
                        Quantity = c.Double(),
                        Tax = c.Double(),
                        UnitPrice = c.Decimal(precision: 18, scale: 2),
                        VendorReference = c.String(),
                        ItemPosition = c.Int(),
                        ItemOrder = c.Double(),
                        Logo = c.String(),
                        ImprintMethod = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        RoleName = c.String(maxLength: 256),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.SalesOrderBlindShip",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SalesOrderId = c.Int(),
                        Title = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Tel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesOrderDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SalesOrderId = c.Int(),
                        Quantity = c.Double(),
                        ShipQuantity = c.Double(),
                        BackOrderQuantity = c.Double(),
                        ItemID = c.String(),
                        Sub_ItemID = c.String(),
                        Description = c.String(),
                        Tax = c.Double(),
                        UnitPrice = c.Decimal(precision: 18, scale: 2),
                        ItemPosition = c.Int(),
                        ItemOrder = c.Double(),
                        Logo = c.String(),
                        ImprintMethod = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesOrder",
                c => new
                    {
                        SalesOrderId = c.Int(nullable: false, identity: true),
                        SalesOrderNo = c.String(),
                        SODate = c.DateTime(),
                        ShipVia = c.String(),
                        ShipDate = c.DateTime(),
                        TradeId = c.Int(),
                        CustomerId = c.Int(nullable: false),
                        CustomerShiptoId = c.Int(),
                        CustomerShipLocation = c.String(),
                        VendorId = c.Int(),
                        BussinesType = c.String(),
                        VendorAddress = c.String(),
                        PurchaseOrderNo = c.String(),
                        PaymentTerms = c.String(),
                        PaymentDate = c.DateTime(),
                        PaymentAmount = c.Decimal(precision: 18, scale: 2),
                        ShippingHandling = c.Decimal(precision: 18, scale: 2),
                        CreaditCardNo = c.String(),
                        Note = c.String(),
                        IsBlindShip = c.Boolean(nullable: false),
                        SalesRep = c.String(),
                        Tax_rate = c.Double(),
                        Invs_Tax = c.Double(),
                        Approvedby = c.String(),
                        AprovedDate = c.DateTime(),
                        Requiredate = c.DateTime(),
                        QuoteId = c.Int(),
                        FromName = c.String(),
                        FromTitle = c.String(),
                        FromCompany = c.String(),
                        FromAddress1 = c.String(),
                        FromAddress2 = c.String(),
                        FromCity = c.String(),
                        FromState = c.String(),
                        FromZip = c.String(),
                        FromCountry = c.String(),
                        FromEmail = c.String(),
                        FromTel = c.String(),
                        FromFax = c.String(),
                        ToName = c.String(),
                        ToTitle = c.String(),
                        ToCompany = c.String(),
                        ToAddress1 = c.String(),
                        ToAddress2 = c.String(),
                        ToCity = c.String(),
                        ToState = c.String(),
                        ToZip = c.String(),
                        ToCountry = c.String(),
                        ToEmail = c.String(),
                        ToTel = c.String(),
                        ToFax = c.String(),
                    })
                .PrimaryKey(t => t.SalesOrderId);
            
            CreateTable(
                "dbo.SalesOrderTES",
                c => new
                    {
                        SalesOrderId = c.Int(nullable: false, identity: true),
                        SalesOrderNo = c.String(),
                    })
                .PrimaryKey(t => t.SalesOrderId);
            
            CreateTable(
                "dbo.Setup_for_Price",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item = c.String(maxLength: 50),
                        Qty = c.Short(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount_Code = c.String(maxLength: 50),
                        Description = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SetupChargeDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SetUpCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RunCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        itemid = c.String(maxLength: 40),
                        ReSetupCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReSetupChargeDiscountCode = c.String(),
                        RunChargeDiscountCode = c.String(),
                        SetupChargeDiscountCode = c.String(),
                        FirstSetupFree = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SETUP",
                c => new
                    {
                        SetupID = c.String(nullable: false, maxLength: 50),
                        Run_charge = c.Int(),
                        SetupCost = c.Int(),
                        Discount_Code = c.String(maxLength: 50),
                        Min = c.Int(),
                        Description = c.String(),
                        Dial = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SetupID);
            
            CreateTable(
                "dbo.ShipmentDetails",
                c => new
                    {
                        ShipmentDetailID = c.Int(nullable: false, identity: true),
                        ShipmentId = c.Int(),
                        DetailId = c.Int(),
                        BoxNo = c.String(),
                        Sub_ItemID = c.String(),
                        Quantity = c.Double(),
                        UnitPrice = c.Decimal(precision: 18, scale: 2),
                        UnitWeight = c.Int(),
                        DimensionH = c.Int(),
                        DimensionL = c.Int(),
                        DimensionD = c.Int(),
                        Reference1 = c.String(),
                        Reference2 = c.String(),
                    })
                .PrimaryKey(t => t.ShipmentDetailID);
            
            CreateTable(
                "dbo.Shipment",
                c => new
                    {
                        ShipmentId = c.Int(nullable: false, identity: true),
                        ShipmentDate = c.DateTime(),
                        InvoiceId = c.Int(),
                        InvoiceNo = c.String(),
                        RateResults = c.String(),
                    })
                .PrimaryKey(t => t.ShipmentId);
            
            CreateTable(
                "dbo.ShipVia",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SPECIALITEM",
                c => new
                    {
                        ItemID = c.String(nullable: false, maxLength: 50),
                        Price = c.String(maxLength: 50),
                        Option = c.String(maxLength: 50),
                        PicID = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ItemID);
            
            CreateTable(
                "dbo.SUB_ITEM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemID = c.String(maxLength: 50),
                        Description = c.String(maxLength: 50),
                        Field1 = c.String(maxLength: 50),
                        Sub_ItemID = c.String(maxLength: 50),
                        Sub_GroupID = c.String(maxLength: 50),
                        OnHand_Db = c.Double(),
                        OnHand_Cr = c.Double(),
                        OpenSO_Db = c.Double(),
                        OpenSO_Cr = c.Double(),
                        OpenPO_Db = c.Double(),
                        OpenPO_Cr = c.Double(),
                        Qty_Db = c.Double(),
                        Qty_Cr = c.Double(),
                        GrossPrice = c.Decimal(precision: 18, scale: 2),
                        NetPrice = c.Decimal(precision: 18, scale: 2),
                        PartNo = c.String(),
                        VendorStock = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SUB_SPECIALITEM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemID = c.String(maxLength: 50),
                        Option = c.String(maxLength: 50),
                        OptionCost = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Term = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.timelydepot_log",
                c => new
                    {
                        logID = c.Int(nullable: false, identity: true),
                        logTime = c.DateTime(),
                        eventdata = c.String(maxLength: 255),
                        note = c.String(),
                    })
                .PrimaryKey(t => t.logID);
            
            CreateTable(
                "dbo.Trade",
                c => new
                    {
                        TradeId = c.Int(nullable: false, identity: true),
                        TradeName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        PostCode = c.String(),
                        Country = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                        WebSite = c.String(),
                        Email = c.String(),
                        ASINo = c.String(),
                        SageNo = c.String(),
                        PPAINo = c.String(),
                    })
                .PrimaryKey(t => t.TradeId);
            
            CreateTable(
                "dbo.UserQuotationDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetailId = c.Int(),
                        ProductType = c.String(maxLength: 100),
                        Quantity = c.String(maxLength: 20),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemID = c.String(),
                        Status = c.Int(),
                        ShippedQuantity = c.Double(),
                        BOQuantity = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserQuotation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.String(maxLength: 20),
                        UserId = c.Int(),
                        Date = c.DateTime(),
                        PostCode = c.String(maxLength: 8),
                        PostalType = c.String(maxLength: 10),
                        ImprintType = c.String(maxLength: 12),
                        Status = c.Int(),
                        Invoicestatus = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRegistration",
                c => new
                    {
                        RId = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 200),
                        UserPassword = c.String(maxLength: 200),
                        Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.RId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        UserName = c.String(maxLength: 50),
                        IsAnonymous = c.Boolean(nullable: false),
                        LastActivityDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UsersInRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId });
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Warehouse = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                        ConfirmationToken = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(nullable: false),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(maxLength: 128),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(maxLength: 128),
                        PasswordVerificationToken = c.String(maxLength: 128),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_OAuthMembership",
                c => new
                    {
                        Provider = c.String(nullable: false, maxLength: 30),
                        ProviderUserId = c.String(nullable: false, maxLength: 100),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Provider, t.ProviderUserId });
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId });
            
            CreateTable(
                "dbo.VendorDefaults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VendorId = c.Int(),
                        SalesContactId = c.Int(),
                        SalesName = c.String(),
                        SubsidiaryId = c.Int(),
                        SubsidiaryName = c.String(),
                        ShiptoAddressId = c.Int(),
                        ShiptoName = c.String(),
                        NoteId = c.Int(),
                        NoteName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.String(),
                        VendorNo = c.String(),
                        VendorPartNo = c.String(),
                        Cost = c.Decimal(precision: 18, scale: 2),
                        CostBlind = c.Decimal(precision: 18, scale: 2),
                        RunCharge = c.Decimal(precision: 18, scale: 2),
                        SetupCharge = c.Decimal(precision: 18, scale: 2),
                        ReSetupCharge = c.Decimal(precision: 18, scale: 2),
                        LeadTimeHrs = c.Int(),
                        LeadTimeMin = c.Int(),
                        LeadTimeSec = c.Int(),
                        LeadTime = c.Time(precision: 7),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VendorNo = c.String(),
                        Status = c.Boolean(nullable: false),
                        BussinesType = c.String(),
                        Origin = c.String(),
                        CreditLimit = c.Decimal(precision: 18, scale: 2),
                        PaymentTerms = c.String(),
                        BussinesSice = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsBillingDept",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VendorId = c.Int(),
                        Beneficiary = c.String(),
                        BeneficiaryAccountNo = c.String(),
                        SWIFT = c.String(),
                        BankName = c.String(),
                        BankAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsContactAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VendorId = c.Int(),
                        CompanyName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Website = c.String(),
                        Note = c.String(),
                        Address3 = c.String(),
                        Tel1 = c.String(),
                        Tel2 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VendorId = c.Int(),
                        OutstandingBalance = c.Decimal(precision: 18, scale: 2),
                        OpenPurchaseOrder = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsSalesContact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VendorId = c.Int(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Tel = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsSpecialNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VendorId = c.Int(),
                        SpecialNote = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VendorType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkerInfluentialPersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        categoryID = c.Int(),
                        influentailPersonID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.YearProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YearofProducts = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.YearProducts");
            DropTable("dbo.WorkerInfluentialPersons");
            DropTable("dbo.VendorTypes");
            DropTable("dbo.VendorsSpecialNotes");
            DropTable("dbo.VendorsSalesContact");
            DropTable("dbo.VendorsHistory");
            DropTable("dbo.VendorsContactAddress");
            DropTable("dbo.VendorsBillingDept");
            DropTable("dbo.Vendors");
            DropTable("dbo.VendorItem");
            DropTable("dbo.VendorDefaults");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.Warehouses");
            DropTable("dbo.UsersInRoles");
            DropTable("dbo.Users");
            DropTable("dbo.UserRegistration");
            DropTable("dbo.UserQuotation");
            DropTable("dbo.UserQuotationDetail");
            DropTable("dbo.Trade");
            DropTable("dbo.timelydepot_log");
            DropTable("dbo.Terms");
            DropTable("dbo.SUB_SPECIALITEM");
            DropTable("dbo.SUB_ITEM");
            DropTable("dbo.SPECIALITEM");
            DropTable("dbo.ShipVia");
            DropTable("dbo.Shipment");
            DropTable("dbo.ShipmentDetails");
            DropTable("dbo.SETUP");
            DropTable("dbo.SetupChargeDetail");
            DropTable("dbo.Setup_for_Price");
            DropTable("dbo.SalesOrderTES");
            DropTable("dbo.SalesOrder");
            DropTable("dbo.SalesOrderDetail");
            DropTable("dbo.SalesOrderBlindShip");
            DropTable("dbo.Roles");
            DropTable("dbo.PurchasOrderDetail");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.Profiles");
            DropTable("dbo.PRICE");
            DropTable("dbo.Payments");
            DropTable("dbo.Parameters");
            DropTable("dbo.packingitemdetail");
            DropTable("dbo.PackagingDetail");
            DropTable("dbo.PACKAGE");
            DropTable("dbo.PackageRateLog");
            DropTable("dbo.PackageRateLogParameters");
            DropTable("dbo.PackageRateLogDetails");
            DropTable("dbo.packagemaster");
            DropTable("dbo.PackageGroupDetail");
            DropTable("dbo.Origin");
            DropTable("dbo.OrderBy");
            DropTable("dbo.Operator");
            DropTable("dbo.New_Items");
            DropTable("dbo.Memberships");
            DropTable("dbo.ITEM");
            DropTable("dbo.ITEM_PACKAGE");
            DropTable("dbo.Invoice");
            DropTable("dbo.InvoiceDetail");
            DropTable("dbo.InitialInfo");
            DropTable("dbo.InformationId");
            DropTable("dbo.InformationDetail");
            DropTable("dbo.ImprintMethods");
            DropTable("dbo.ImprintMaster");
            DropTable("dbo.Imprintitemdetail");
            DropTable("dbo.Group_Master");
            DropTable("dbo.EventLog");
            DropTable("dbo.DiscountManage");
            DropTable("dbo.DIAL");
            DropTable("dbo.DF");
            DropTable("dbo.Deptos");
            DropTable("dbo.DELIVERY");
            DropTable("dbo.CustomersSubsidiaryAddress");
            DropTable("dbo.CustomersSpecialNotes");
            DropTable("dbo.CustomersShipAddress");
            DropTable("dbo.CustomersSalesContact");
            DropTable("dbo.CustomersHistory");
            DropTable("dbo.CustomersCreditCardShipping");
            DropTable("dbo.CustomersContactAddress");
            DropTable("dbo.CustomersCardType");
            DropTable("dbo.CustomersBillingDept");
            DropTable("dbo.Customers");
            DropTable("dbo.CustomerDefaults");
            DropTable("dbo.COLLECTION");
            DropTable("dbo.ClssNos");
            DropTable("dbo.CATEGORY");
            DropTable("dbo.CastMaster");
            DropTable("dbo.Bussines");
            DropTable("dbo.aspnet_UsersInRoles");
            DropTable("dbo.aspnet_Users");
            DropTable("dbo.aspnet_Roles");
            DropTable("dbo.aspnet_Membership");
            DropTable("dbo.aspnet_Applications");
            DropTable("dbo.Applications");
        }
    }
}
