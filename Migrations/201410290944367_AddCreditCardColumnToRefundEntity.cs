namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreditCardColumnToRefundEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
        "dbo.CustomerTransactions",
        c => new
        {
            TransactionId = c.Int(nullable: false, identity: true),
            CustomerId = c.Int(nullable: false),
            RefundId = c.Int(nullable: false),
            PaymentId = c.Int(nullable: false),
            TransactionCode = c.String(),
        })
        .PrimaryKey(t => t.TransactionId);

            CreateTable(
               "dbo.Refunds",
               c => new
               {
                   RefundId = c.Int(nullable: false, identity: true),
                   CustomerId = c.Int(nullable: false),
                   RefundAmount = c.Double(nullable: false),
                   Refunddate = c.DateTime(nullable: false),
               })
               .PrimaryKey(t => t.RefundId);
            AddColumn("dbo.Refunds", "RefundNo", c => c.String());
            AddColumn("dbo.Refunds", "CreditCardNo", c => c.String());
            AddColumn("dbo.Refunds", "PayLog", c => c.String());
            AddColumn("dbo.Refunds", "ReferenceNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Refunds", "CreditCardNo");
            DropColumn("dbo.Refunds", "RefundNo");
        }
    }
}
