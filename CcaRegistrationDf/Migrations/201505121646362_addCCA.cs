namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCCA : DbMigration
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
            
            AlterColumn("dbo.CCAs", "ApplicationSubmissionDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Counselors", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Counselors", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Counselors", "LastName", c => c.String(nullable: false));
            CreateIndex("dbo.Students", "StudentBudgetID");
            AddForeignKey("dbo.Students", "StudentBudgetID", "dbo.StudentBudgets", "ID", cascadeDelete: true);
            DropColumn("dbo.CCAs", "SubmitterTypeID");
            DropColumn("dbo.CCAs", "AssessedProficiency");
            DropColumn("dbo.CCAs", "TeacherCactusID");
            DropColumn("dbo.CCAs", "TeacherFirstName");
            DropColumn("dbo.CCAs", "TeacherLastName");
            DropColumn("dbo.CCAs", "CourseFee");
            DropColumn("dbo.Counselors", "School");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Counselors", "School", c => c.String());
            AddColumn("dbo.CCAs", "CourseFee", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CCAs", "TeacherLastName", c => c.String());
            AddColumn("dbo.CCAs", "TeacherFirstName", c => c.String());
            AddColumn("dbo.CCAs", "TeacherCactusID", c => c.Int());
            AddColumn("dbo.CCAs", "AssessedProficiency", c => c.String());
            AddColumn("dbo.CCAs", "SubmitterTypeID", c => c.Int());
            DropForeignKey("dbo.Students", "StudentBudgetID", "dbo.StudentBudgets");
            DropIndex("dbo.Students", new[] { "StudentBudgetID" });
            AlterColumn("dbo.Counselors", "LastName", c => c.String());
            AlterColumn("dbo.Counselors", "FirstName", c => c.String());
            AlterColumn("dbo.Counselors", "Email", c => c.String());
            AlterColumn("dbo.CCAs", "ApplicationSubmissionDate", c => c.DateTime());
            DropTable("dbo.StudentBudgets");
        }
    }
}
