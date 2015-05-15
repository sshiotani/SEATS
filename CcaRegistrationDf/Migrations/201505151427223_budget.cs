namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class budget : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentBudgets",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        OffSet = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Distribution = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StudentBudgets");
        }
    }
}
