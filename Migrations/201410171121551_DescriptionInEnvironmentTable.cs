namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescriptionInEnvironmentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EnvironmentParameters", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EnvironmentParameters", "Description");
        }
    }
}
