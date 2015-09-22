namespace SEATS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CounselorRejection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CounselorRejectionReasons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CCAs", "IsCounselorRejecting", c => c.Boolean(nullable: false));
            AddColumn("dbo.CCAs", "CounselorRejectionReasonsID", c => c.Int());
            AddColumn("dbo.CCAs", "CounselorRejectionExplantion", c => c.String());
            CreateIndex("dbo.CCAs", "CounselorRejectionReasonsID");
            AddForeignKey("dbo.CCAs", "CounselorRejectionReasonsID", "dbo.CounselorRejectionReasons", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CCAs", "CounselorRejectionReasonsID", "dbo.CounselorRejectionReasons");
            DropIndex("dbo.CCAs", new[] { "CounselorRejectionReasonsID" });
            DropColumn("dbo.CCAs", "CounselorRejectionExplantion");
            DropColumn("dbo.CCAs", "CounselorRejectionReasonsID");
            DropColumn("dbo.CCAs", "IsCounselorRejecting");
            DropTable("dbo.CounselorRejectionReasons");
        }
    }
}
