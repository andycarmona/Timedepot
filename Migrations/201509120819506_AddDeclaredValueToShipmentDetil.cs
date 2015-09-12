namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclaredValueToShipmentDetil : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentDetails", "DeclaredValue", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentDetails", "DeclaredValue");
        }
    }
}
