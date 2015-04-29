namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class providerteacher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MainTables", "TeacherFirstName", c => c.String());
            AddColumn("dbo.MainTables", "TeacherLastName", c => c.String());
            AddColumn("dbo.MainTables", "TeacherCactusID", c => c.Int());
            DropColumn("dbo.MainTables", "ProviderCactusID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MainTables", "ProviderCactusID", c => c.Int());
            DropColumn("dbo.MainTables", "TeacherCactusID");
            DropColumn("dbo.MainTables", "TeacherLastName");
            DropColumn("dbo.MainTables", "TeacherFirstName");
        }
    }
}
