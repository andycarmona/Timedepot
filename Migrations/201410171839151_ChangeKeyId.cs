namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeKeyId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EnvironmentParameters", "KeyValue", c => c.String());
         
        }
        
        public override void Down()
        {
            AddColumn("dbo.EnvironmentParameters", "KeyId", c => c.String());
            DropColumn("dbo.EnvironmentParameters", "KeyValue");
        }
    }
}
