namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moveOffset : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CourseCategories", "CourseFeeID");
            AddForeignKey("dbo.CourseCategories", "CourseFeeID", "dbo.CourseFees", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseCategories", "CourseFeeID", "dbo.CourseFees");
            DropIndex("dbo.CourseCategories", new[] { "CourseFeeID" });
        }
    }
}
