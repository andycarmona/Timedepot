namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShipmentTables : DbMigration
    {
        public override void Up()
        {
       //     CreateTable(
       //"dbo.Shipment",
       //c => new
       //{
       //    ShipmentId = c.Int(nullable: false, identity: true),
       //    ShipmentDate = c.DateTime(nullable: false),
       //    InvoiceId = c.Int(nullable: true),
       //    InvoiceNo = c.String(),
       //    RateResults = c.String(),
       //})
       //.PrimaryKey(t => t.ShipmentId);

       //     CreateTable(
       //   "dbo.ShipmentDetails",
       //   c => new
       //   {
       //       ShipmentId = c.Int(nullable: true),
       //       ShipmentDetailId = c.Int(nullable: false, identity: true),
       //       DetailId = c.Int(nullable: true),
       //       BoxNo = c.String(),
       //       Sub_ItemID = c.String(),
       //       Quantity = c.String(),
       //       UnitPrice = c.Decimal(nullable: true),
       //       UnitWeight = c.Int(nullable: true),
       //       DimensionH = c.Int(nullable: true),
       //       DimensionL = c.Int(nullable: true),
       //       DimensionD = c.Int(nullable: true),
       //       Reference1 = c.String(),
       //       Reference2 = c.String()


       //   })
       //   .PrimaryKey(t => t.ShipmentId);
        }
        
        public override void Down()
        {
        }
    }
}
