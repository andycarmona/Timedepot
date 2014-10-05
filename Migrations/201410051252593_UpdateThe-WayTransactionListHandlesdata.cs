namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTheWayTransactionListHandlesdata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Refunds", "TransactionId", c => c.Int(nullable: false));
            DropColumn("dbo.CustomerTransactions", "TransactionDate");
            DropColumn("dbo.CustomerTransactions", "CustomerNo");
            DropColumn("dbo.CustomerTransactions", "SalesorderNo");
            DropColumn("dbo.CustomerTransactions", "RefundId");
            DropColumn("dbo.CustomerTransactions", "PaymentId");
            DropColumn("dbo.Refunds", "CustomerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Refunds", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerTransactions", "PaymentId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerTransactions", "RefundId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerTransactions", "SalesorderNo", c => c.String());
            AddColumn("dbo.CustomerTransactions", "CustomerNo", c => c.String());
            AddColumn("dbo.CustomerTransactions", "TransactionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Refunds", "TransactionId");
        }
    }
}
