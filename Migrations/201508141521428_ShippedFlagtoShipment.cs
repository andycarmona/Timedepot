namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippedFlagtoShipment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipment", "Shipped", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipment", "Shipped");
        }
    }
}
