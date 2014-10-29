namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreditCardColumnToRefundEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Refunds", "RefundNo", c => c.String());
            AddColumn("dbo.Refunds", "CreditCardNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Refunds", "CreditCardNo");
            DropColumn("dbo.Refunds", "RefundNo");
        }
    }
}
