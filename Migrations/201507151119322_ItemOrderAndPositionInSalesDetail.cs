namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemOrderAndPositionInSalesDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesOrderDetail", "ItemPosition", c => c.Int(nullable: true));
            AddColumn("dbo.SalesOrderDetail", "ItemOrder", c => c.Double(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
