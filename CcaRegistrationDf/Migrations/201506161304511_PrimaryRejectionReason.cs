namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimaryRejectionReason : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CCAs", name: "PrimaryRejectionReasons_ID", newName: "PrimaryRejectionReasonsID");
            RenameIndex(table: "dbo.CCAs", name: "IX_PrimaryRejectionReasons_ID", newName: "IX_PrimaryRejectionReasonsID");
            DropColumn("dbo.CCAs", "PrimaryRejectionReasonID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CCAs", "PrimaryRejectionReasonID", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.CCAs", name: "IX_PrimaryRejectionReasonsID", newName: "IX_PrimaryRejectionReasons_ID");
            RenameColumn(table: "dbo.CCAs", name: "PrimaryRejectionReasonsID", newName: "PrimaryRejectionReasons_ID");
        }
    }
}
