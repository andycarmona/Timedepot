namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeQuantityFormat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceDetail", "Quantity", c => c.Int());
            AlterColumn("dbo.ShipmentDetails", "Quantity", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShipmentDetails", "Quantity", c => c.Double());
            AlterColumn("dbo.InvoiceDetail", "Quantity", c => c.Double());
        }
    }
}
