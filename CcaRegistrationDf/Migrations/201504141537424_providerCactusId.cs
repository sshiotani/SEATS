namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class providerCactusId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MainTables", "TeacherCactusID", c => c.Int());
            DropColumn("dbo.MainTables", "TeacherCactusID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MainTables", "TeacherCactusID", c => c.Int());
            DropColumn("dbo.MainTables", "TeacherCactusID");
        }
    }
}
