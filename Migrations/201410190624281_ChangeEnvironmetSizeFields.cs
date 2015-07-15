namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEnvironmetSizeFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
          "dbo.EnvironmentParameters",
          c => new
          {
              ParameterId = c.Int(nullable: false, identity: true),
              KeyParameter = c.String(nullable: false, maxLength: 50),
              Description = c.String(nullable: false, maxLength: 50),
              Active = c.Boolean(nullable: false),
              ServerUrl = c.String(),
          })
          .PrimaryKey(t => t.ParameterId);
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EnvironmentParameters", "Description", c => c.String());
            AlterColumn("dbo.EnvironmentParameters", "KeyParameter", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
