namespace CarService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "FreeFrom", c => c.String());
            AlterColumn("dbo.Customers", "FreeTo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "FreeTo", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "FreeFrom", c => c.DateTime(nullable: false));
        }
    }
}
