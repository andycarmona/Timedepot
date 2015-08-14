namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLabelColumnToShipmentDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentDetails", "ShipmentLabel", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentDetails", "ShipmentLabel");
        }
    }
}
