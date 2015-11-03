namespace SEATS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ccaUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCAs", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CCAs", "Discriminator");
        }
    }
}
