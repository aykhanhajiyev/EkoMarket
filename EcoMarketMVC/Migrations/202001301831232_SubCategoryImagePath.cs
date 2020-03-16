namespace EcoMarketMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubCategoryImagePath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subcategories", "ImagePath", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subcategories", "ImagePath");
        }
    }
}
