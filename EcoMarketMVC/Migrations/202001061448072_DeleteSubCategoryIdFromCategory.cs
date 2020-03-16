namespace EcoMarketMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteSubCategoryIdFromCategory : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Categories", "Subcategoryid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Subcategoryid", c => c.Int(nullable: false));
        }
    }
}
