namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cactus : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.EnrollmentLocations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EnrollmentLocations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
