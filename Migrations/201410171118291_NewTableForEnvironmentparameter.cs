namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableForEnvironmentparameter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EnvironmentParameters",
                c => new
                    {
                        ParameterId = c.Int(nullable: false, identity: true),
                        KeyParameter = c.String(),
                        Active = c.Boolean(nullable: false),
                        ServerUrl = c.String(),
                    })
                .PrimaryKey(t => t.ParameterId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EnvironmentParameters");
        }
    }
}
