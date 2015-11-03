namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConverValueBoolToString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoice", "AddressValidatedResult", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoice", "AddressValidatedResult");
        }
    }
}
