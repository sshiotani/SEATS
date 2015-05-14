namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CCA1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCAs", "ExcessiveFEDReasonID", c => c.Int());
            DropColumn("dbo.CCAs", "ExcessiveFEDReasonCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CCAs", "ExcessiveFEDReasonCode", c => c.Int());
            DropColumn("dbo.CCAs", "ExcessiveFEDReasonID");
        }
    }
}
