namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.EnvironmentParameters",
               c => new
               {
                   ParameterId = c.Int(nullable: false, identity: true),
                   KeyValue = c.String(nullable: false, maxLength: 20),
                   KeyParameter = c.String(nullable: false, maxLength: 30),
                   Active = c.Boolean(nullable: false),
                   ServerUrl = c.String(nullable: false, maxLength: 50),
                   TransactionUri = c.String(nullable: false, maxLength: 20),
                   Description = c.String(),
               })
               .PrimaryKey(t => t.ParameterId);
            
        }
        
        public override void Down()
        {
      
            DropTable("dbo.EnvironmentParameters");
    
        }
    }
}
