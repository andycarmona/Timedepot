namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropTransacionCodeTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.TransactionsCodes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TransactionsCodes",
                c => new
                    {
                        TransactionCodeId = c.Int(nullable: false, identity: true),
                        CodeDescription = c.String(),
                    })
                .PrimaryKey(t => t.TransactionCodeId);
            
        }
    }
}
