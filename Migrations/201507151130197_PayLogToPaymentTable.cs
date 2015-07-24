namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayLogToPaymentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "PayLog", c => c.String());
        }
        
        public override void Down()
        {
        }
    }
}
