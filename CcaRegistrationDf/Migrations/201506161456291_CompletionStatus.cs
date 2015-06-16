namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompletionStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseCompletionStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CCAs", "CourseCompletionStatusID", c => c.Int());
            CreateIndex("dbo.CCAs", "CourseCompletionStatusID");
            AddForeignKey("dbo.CCAs", "CourseCompletionStatusID", "dbo.CourseCompletionStatus", "ID");
            DropColumn("dbo.CCAs", "CompletionStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CCAs", "CompletionStatus", c => c.String());
            DropForeignKey("dbo.CCAs", "CourseCompletionStatusID", "dbo.CourseCompletionStatus");
            DropIndex("dbo.CCAs", new[] { "CourseCompletionStatusID" });
            DropColumn("dbo.CCAs", "CourseCompletionStatusID");
            DropTable("dbo.CourseCompletionStatus");
        }
    }
}
