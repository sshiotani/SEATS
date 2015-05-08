namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moveFEDsection : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Student", "HasExcessiveFED");
            DropColumn("dbo.Student", "ExcessiveFEDExplanation");
            DropColumn("dbo.Student", "ExcessiveFEDReasonCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Student", "ExcessiveFEDReasonCode", c => c.Int());
            AddColumn("dbo.Student", "ExcessiveFEDExplanation", c => c.String());
            AddColumn("dbo.Student", "HasExcessiveFED", c => c.Boolean(nullable: false));
        }
    }
}
