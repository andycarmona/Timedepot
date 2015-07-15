namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTrnasactionListAndAddTransactioncodeinpaymentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "TransactionCode", c => c.Int(nullable: false));
            DropTable("dbo.CustomerTransactions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomerTransactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        TransactionCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionId);
            
            DropColumn("dbo.Payments", "TransactionCode");
        }
    }
}
