namespace SEATS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GradeLevel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CCAs", "StudentGradeLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CCAs", "StudentGradeLevel", c => c.Int());
        }
    }
}
