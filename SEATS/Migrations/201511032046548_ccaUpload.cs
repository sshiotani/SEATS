namespace SEATS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ccaUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCAs", "IsUpload", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CCAs", "IsUpload");
        }
    }
}
