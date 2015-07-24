namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTranslationCodesTable : DbMigration
    {
        public override void Up()
        {  
            AddColumn("dbo.CustomerTransactions", "TransactionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerTransactions", "CustomerNo", c => c.String());
            AddColumn("dbo.CustomerTransactions", "SalesorderNo", c => c.String());
            AlterColumn("dbo.CustomerTransactions", "TransactionCode", c => c.Int(nullable: false));
            DropColumn("dbo.CustomerTransactions", "CustomerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerTransactions", "CustomerId", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerTransactions", "TransactionCode", c => c.String());
            DropColumn("dbo.CustomerTransactions", "SalesorderNo");
            DropColumn("dbo.CustomerTransactions", "CustomerNo");
            DropColumn("dbo.CustomerTransactions", "TransactionDate");
            DropTable("dbo.TransactionsCodes");
        }
    }
}
