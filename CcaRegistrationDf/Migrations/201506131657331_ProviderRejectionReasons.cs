namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProviderRejectionReasons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RejectionReasons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CCAs", "PrimaryRejectionReasonsID", c => c.Int(nullable: false));
            AddColumn("dbo.CCAs", "PrimaryRejectionReasons_ID", c => c.Int());
            CreateIndex("dbo.CCAs", "PrimaryRejectionReasons_ID");
            AddForeignKey("dbo.CCAs", "PrimaryRejectionReasons_ID", "dbo.RejectionReasons", "ID");
            DropColumn("dbo.CCAs", "PrimaryLEAReasonRejectingCCA");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CCAs", "PrimaryLEAReasonRejectingCCA", c => c.String());
            DropForeignKey("dbo.CCAs", "PrimaryRejectionReasons_ID", "dbo.RejectionReasons");
            DropIndex("dbo.CCAs", new[] { "PrimaryRejectionReasons_ID" });
            DropColumn("dbo.CCAs", "PrimaryRejectionReasons_ID");
            DropColumn("dbo.CCAs", "PrimaryRejectionReasonsID");
            DropTable("dbo.RejectionReasons");
        }
    }
}
