namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDeclaredValueToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShipmentDetails", "DeclaredValue", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentDetails", "DeclaredValue", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
