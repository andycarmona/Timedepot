namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesForTransactionAndRefunds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Refunds", "SalesOrderNo", c => c.String());
            AddColumn("dbo.Refunds", "CustomerNo", c => c.String());
            AlterColumn("dbo.Refunds", "RefundAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Refunds", "RefundAmount", c => c.Double(nullable: false));
            DropColumn("dbo.Refunds", "CustomerNo");
            DropColumn("dbo.Refunds", "SalesOrderNo");
        }
    }
}
