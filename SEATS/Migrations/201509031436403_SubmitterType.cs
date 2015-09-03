namespace SEATS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubmitterType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCAs", "SubmitterTypeID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CCAs", "SubmitterTypeID");
        }
    }
}
