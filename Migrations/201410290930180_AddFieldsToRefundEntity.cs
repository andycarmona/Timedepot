namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToRefundEntity : DbMigration
    {
        public override void Up()
        {
    
        }
        
        public override void Down()
        {
            DropColumn("dbo.Refunds", "ReferenceNo");
            DropColumn("dbo.Refunds", "PayLog");
        }
    }
}
