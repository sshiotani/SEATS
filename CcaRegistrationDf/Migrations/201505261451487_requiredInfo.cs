namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredInfo : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.PrimaryUsers", "FirstName", c => c.String(nullable: false));
            //AlterColumn("dbo.PrimaryUsers", "LastName", c => c.String(nullable: false));
            //AlterColumn("dbo.PrimaryUsers", "Phone", c => c.String(nullable: false));
            //AlterColumn("dbo.ProviderUsers", "FirstName", c => c.String(nullable: false));
            //AlterColumn("dbo.ProviderUsers", "LastName", c => c.String(nullable: false));
            //AlterColumn("dbo.ProviderUsers", "Phone", c => c.String(nullable: false));
            //AlterColumn("dbo.ProviderUsers", "Fax", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.ProviderUsers", "Fax", c => c.String());
            //AlterColumn("dbo.ProviderUsers", "Phone", c => c.String());
            //AlterColumn("dbo.ProviderUsers", "LastName", c => c.String());
            //AlterColumn("dbo.ProviderUsers", "FirstName", c => c.String());
            //AlterColumn("dbo.PrimaryUsers", "Phone", c => c.String());
            //AlterColumn("dbo.PrimaryUsers", "LastName", c => c.String());
            //AlterColumn("dbo.PrimaryUsers", "FirstName", c => c.String());
        }
    }
}
