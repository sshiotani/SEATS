namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnrollmentLocation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CCAs", "PrimaryID", "dbo.Primaries");
            DropIndex("dbo.CCAs", new[] { "PrimaryID" });
            AddColumn("dbo.CCAs", "EnrollmentLocationID", c => c.Int());
            AlterColumn("dbo.CCAs", "PrimaryID", c => c.Int());
            CreateIndex("dbo.CCAs", "PrimaryID");
            AddForeignKey("dbo.CCAs", "PrimaryID", "dbo.Primaries", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CCAs", "PrimaryID", "dbo.Primaries");
            DropIndex("dbo.CCAs", new[] { "PrimaryID" });
            AlterColumn("dbo.CCAs", "PrimaryID", c => c.Int(nullable: false));
            DropColumn("dbo.CCAs", "EnrollmentLocationID");
            CreateIndex("dbo.CCAs", "PrimaryID");
            AddForeignKey("dbo.CCAs", "PrimaryID", "dbo.Primaries", "ID", cascadeDelete: true);
        }
    }
}
