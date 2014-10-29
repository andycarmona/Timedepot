namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToRefundEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Refunds", "PayLog", c => c.String());
            AddColumn("dbo.Refunds", "ReferenceNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Refunds", "ReferenceNo");
            DropColumn("dbo.Refunds", "PayLog");
        }
    }
}
