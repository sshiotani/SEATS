namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessonToCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Session_ID", c => c.Int());
            CreateIndex("dbo.Courses", "Session_ID");
            AddForeignKey("dbo.Courses", "Session_ID", "dbo.Sessions", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "Session_ID", "dbo.Sessions");
            DropIndex("dbo.Courses", new[] { "Session_ID" });
            DropColumn("dbo.Courses", "Session_ID");
        }
    }
}
