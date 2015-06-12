namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class June12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrimaryUsers", "Email", c => c.String());
            AddColumn("dbo.ProviderUsers", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProviderUsers", "Email");
            DropColumn("dbo.PrimaryUsers", "Email");
        }
    }
}
