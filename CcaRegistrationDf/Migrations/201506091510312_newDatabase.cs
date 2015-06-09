namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Providers", "Email", c => c.String());
            AddColumn("dbo.Students", "StudentNumber", c => c.Int());
            AlterColumn("dbo.Students", "SSID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "SSID", c => c.Int());
            DropColumn("dbo.Students", "StudentNumber");
            DropColumn("dbo.Providers", "Email");
        }
    }
}
