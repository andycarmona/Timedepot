namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThirdpartyinfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillerContactData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShipmentId = c.Int(nullable: false),
                        Contact = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        Country = c.String(),
                        AccountNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BillerContactData");
        }
    }
}
