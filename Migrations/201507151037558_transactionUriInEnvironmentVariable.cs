namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionUriInEnvironmentVariable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EnvironmentParameters", "TransactionUri", c => c.String());
        }
        
        public override void Down()
        {
        }
    }
}
