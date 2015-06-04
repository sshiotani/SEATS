namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePrimary : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CCAs", "PrimaryID", "dbo.Primaries");
            DropIndex("dbo.CCAs", new[] { "PrimaryID" });
            DropColumn("dbo.CCAs", "PrimaryID");
            DropTable("dbo.Primaries");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Primaries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EnrollmentLocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CCAs", "PrimaryID", c => c.Int());
            CreateIndex("dbo.CCAs", "PrimaryID");
            AddForeignKey("dbo.CCAs", "PrimaryID", "dbo.Primaries", "ID");
        }
    }
}
