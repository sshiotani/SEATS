namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creditchange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "Credit", c => c.String());
            DropColumn("dbo.Courses", "CreditOptions");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "CreditOptions", c => c.String());
            AlterColumn("dbo.Courses", "Credit", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
