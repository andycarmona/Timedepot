namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoicePaymentToPaymentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "InvoicePayment", c => c.String());
        }
        
        public override void Down()
        {
        }
    }
}
