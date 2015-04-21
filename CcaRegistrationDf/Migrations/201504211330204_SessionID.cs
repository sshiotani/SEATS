namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Session_ID", "dbo.Sessions");
            DropIndex("dbo.Courses", new[] { "Session_ID" });
            RenameColumn(table: "dbo.Courses", name: "Session_ID", newName: "SessionID");
            AlterColumn("dbo.Courses", "SessionID", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "SessionID");
            AddForeignKey("dbo.Courses", "SessionID", "dbo.Sessions", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "SessionID", "dbo.Sessions");
            DropIndex("dbo.Courses", new[] { "SessionID" });
            AlterColumn("dbo.Courses", "SessionID", c => c.Int());
            RenameColumn(table: "dbo.Courses", name: "SessionID", newName: "Session_ID");
            CreateIndex("dbo.Courses", "Session_ID");
            AddForeignKey("dbo.Courses", "Session_ID", "dbo.Sessions", "ID");
        }
    }
}
