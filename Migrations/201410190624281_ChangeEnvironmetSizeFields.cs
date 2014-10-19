namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEnvironmetSizeFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EnvironmentParameters", "KeyParameter", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.EnvironmentParameters", "Description", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EnvironmentParameters", "Description", c => c.String());
            AlterColumn("dbo.EnvironmentParameters", "KeyParameter", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
