namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shippedFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentDetails", "Shipped", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentDetails", "Shipped");
        }
    }
}
