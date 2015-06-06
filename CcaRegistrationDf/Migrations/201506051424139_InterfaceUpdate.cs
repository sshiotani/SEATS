namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterfaceUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCAs", "BusinessAdministratorSignature", c => c.String());
            AddColumn("dbo.CCAs", "PrimaryNotes", c => c.String());
            AddColumn("dbo.CCAs", "ProviderSignature", c => c.String());
            AddColumn("dbo.CCAs", "ProviderNotes", c => c.String());
            CreateIndex("dbo.Students", "ParentID");
            AddForeignKey("dbo.Students", "ParentID", "dbo.Parents", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "ParentID", "dbo.Parents");
            DropIndex("dbo.Students", new[] { "ParentID" });
            DropColumn("dbo.CCAs", "ProviderNotes");
            DropColumn("dbo.CCAs", "ProviderSignature");
            DropColumn("dbo.CCAs", "PrimaryNotes");
            DropColumn("dbo.CCAs", "BusinessAdministratorSignature");
        }
    }
}
