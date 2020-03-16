namespace EcoMarketMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteSubCategoryIdformproducts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "SubcategoryId", "dbo.Subcategories");
            DropIndex("dbo.Products", new[] { "SubcategoryId" });
            RenameColumn(table: "dbo.Products", name: "SubcategoryId", newName: "Subcategory_Id");
            AddColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "Subcategory_Id", c => c.Int());
            CreateIndex("dbo.Products", "CategoryId");
            CreateIndex("dbo.Products", "Subcategory_Id");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "Subcategory_Id", "dbo.Subcategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Subcategory_Id", "dbo.Subcategories");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "Subcategory_Id" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            AlterColumn("dbo.Products", "Subcategory_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "CategoryId");
            RenameColumn(table: "dbo.Products", name: "Subcategory_Id", newName: "SubcategoryId");
            CreateIndex("dbo.Products", "SubcategoryId");
            AddForeignKey("dbo.Products", "SubcategoryId", "dbo.Subcategories", "Id", cascadeDelete: true);
        }
    }
}
