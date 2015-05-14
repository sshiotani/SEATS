namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeBudget : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "StudentBudgetID", "dbo.StudentBudgets");
            DropIndex("dbo.Students", new[] { "StudentBudgetID" });
            DropTable("dbo.StudentBudgets");
        }
        
        public override void Down()
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
            
            CreateIndex("dbo.Students", "StudentBudgetID");
            AddForeignKey("dbo.Students", "StudentBudgetID", "dbo.StudentBudgets", "ID", cascadeDelete: true);
        }
    }
}
