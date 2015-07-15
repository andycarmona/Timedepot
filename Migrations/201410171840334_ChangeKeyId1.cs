namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeKeyId1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EnvironmentParameters", "KeyValue", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EnvironmentParameters", "KeyValue", c => c.Int(nullable: false));
        }
    }
}
