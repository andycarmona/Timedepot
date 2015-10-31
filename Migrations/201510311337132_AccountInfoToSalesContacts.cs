namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountInfoToSalesContacts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomersSalesContact", "AccountInformation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomersSalesContact", "AccountInformation");
        }
    }
}
