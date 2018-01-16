namespace MyCompanyName.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppSupplier", "UserName", c => c.String());
            DropColumn("dbo.AppSupplier", "Date");
            DropColumn("dbo.AppSupplier", "IsCancelled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppSupplier", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppSupplier", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.AppSupplier", "UserName");
        }
    }
}
