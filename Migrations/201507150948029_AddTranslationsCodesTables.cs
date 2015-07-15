namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTranslationsCodesTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.TransactionsCodes",
               c => new
               {
                   TransactionCodeId = c.Int(nullable: false, identity: true),
                   TransactionCode = c.Int(nullable: false),
                   CodeDescription = c.String(),
               })
               .PrimaryKey(t => t.TransactionCodeId);
        }
        
        public override void Down()
        {
        }
    }
}
