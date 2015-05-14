namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exessiveFED : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExcessiveFEDReasons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExcessiveFEDReasons");
        }
    }
}
