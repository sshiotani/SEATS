namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CCA2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CCAs", "CourseCreditID", "dbo.CourseCredits");
            DropIndex("dbo.CCAs", new[] { "CourseCreditID" });
            RenameColumn(table: "dbo.CCAs", name: "CourseID", newName: "OnlineCourseID");
            RenameIndex(table: "dbo.CCAs", name: "IX_CourseID", newName: "IX_OnlineCourseID");
            AlterColumn("dbo.CCAs", "CourseCategoryID", c => c.Int(nullable: false));
            AlterColumn("dbo.CCAs", "CourseCreditID", c => c.Int(nullable: false));
            CreateIndex("dbo.CCAs", "CourseCategoryID");
            CreateIndex("dbo.CCAs", "CourseCreditID");
            CreateIndex("dbo.CCAs", "SessionID");
            AddForeignKey("dbo.CCAs", "CourseCategoryID", "dbo.CourseCategories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CCAs", "SessionID", "dbo.Sessions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CCAs", "CourseCreditID", "dbo.CourseCredits", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CCAs", "CourseCreditID", "dbo.CourseCredits");
            DropForeignKey("dbo.CCAs", "SessionID", "dbo.Sessions");
            DropForeignKey("dbo.CCAs", "CourseCategoryID", "dbo.CourseCategories");
            DropIndex("dbo.CCAs", new[] { "SessionID" });
            DropIndex("dbo.CCAs", new[] { "CourseCreditID" });
            DropIndex("dbo.CCAs", new[] { "CourseCategoryID" });
            AlterColumn("dbo.CCAs", "CourseCreditID", c => c.Int());
            AlterColumn("dbo.CCAs", "CourseCategoryID", c => c.Int());
            RenameIndex(table: "dbo.CCAs", name: "IX_OnlineCourseID", newName: "IX_CourseID");
            RenameColumn(table: "dbo.CCAs", name: "OnlineCourseID", newName: "CourseID");
            CreateIndex("dbo.CCAs", "CourseCreditID");
            AddForeignKey("dbo.CCAs", "CourseCreditID", "dbo.CourseCredits", "ID");
        }
    }
}
