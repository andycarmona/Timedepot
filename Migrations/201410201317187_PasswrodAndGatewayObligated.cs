namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswrodAndGatewayObligated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EnvironmentParameters", "GatewayId", c => c.String(nullable: false));
            AlterColumn("dbo.EnvironmentParameters", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EnvironmentParameters", "Password", c => c.String());
            AlterColumn("dbo.EnvironmentParameters", "GatewayId", c => c.String());
        }
    }
}
