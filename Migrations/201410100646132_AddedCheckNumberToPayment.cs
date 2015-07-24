namespace TimelyDepotMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCheckNumberToPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "CheckNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "CheckNo");
        }
    }
}
