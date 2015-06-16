namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProviderRejectionReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCAs", "ProviderRejectionReasonsID", c => c.Int());
            CreateIndex("dbo.CCAs", "ProviderRejectionReasonsID");
            AddForeignKey("dbo.CCAs", "ProviderRejectionReasonsID", "dbo.ProviderRejectionReasons", "ID");
            DropColumn("dbo.CCAs", "ProviderReasonRejection");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CCAs", "ProviderReasonRejection", c => c.String());
            DropForeignKey("dbo.CCAs", "ProviderRejectionReasonsID", "dbo.ProviderRejectionReasons");
            DropIndex("dbo.CCAs", new[] { "ProviderRejectionReasonsID" });
            DropColumn("dbo.CCAs", "ProviderRejectionReasonsID");
        }
    }
}
