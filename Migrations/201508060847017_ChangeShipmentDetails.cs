namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeShipmentDetails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShipmentDetails", "ShipmentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentDetails", "ShipmentId", c => c.Int());
        }
    }
}
