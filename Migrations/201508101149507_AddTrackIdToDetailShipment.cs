namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrackIdToDetailShipment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentDetails", "trackId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShipmentDetails", "trackId");
        }
    }
}
