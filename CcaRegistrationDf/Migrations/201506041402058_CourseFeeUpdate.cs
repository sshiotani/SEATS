namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseFeeUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCAs", "CourseFee", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CourseFees", "ValidYear", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourseFees", "ValidYear");
            DropColumn("dbo.CCAs", "CourseFee");
        }
    }
}
