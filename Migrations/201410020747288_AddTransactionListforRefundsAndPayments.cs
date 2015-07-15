namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddTransactionListforRefundsAndPayments : DbMigration
    {
        public override void Up()
        {


            CreateTable(
               "dbo.CustomerTransactions",
               c => new
               {
                   TransactionId = c.Int(nullable: false, identity: true),
                   CustomerId = c.Int(nullable: false),
                   RefundId = c.Int(nullable: false),
                   PaymentId = c.Int(nullable: false),
                   TransactionCode = c.String(),
               })
               .PrimaryKey(t => t.TransactionId);


            CreateTable(
                "dbo.Refunds",
                c => new
                    {
                        RefundId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        RefundAmount = c.Double(nullable: false),
                        Refunddate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RefundId);

        }

        public override void Down()
        {
            DropTable("dbo.Refunds");
            DropTable("dbo.CustomerTransactions");
        }
    }
}
