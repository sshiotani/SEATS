namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class primaryEnrollment1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PrimaryUsers", "IsSigned");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrimaryUsers", "IsSigned", c => c.Boolean(nullable: false));
        }
    }
}
