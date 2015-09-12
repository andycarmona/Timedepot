namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpsNumberToShipmentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipment", "UpsNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipment", "UpsNumber");
        }
    }
}
