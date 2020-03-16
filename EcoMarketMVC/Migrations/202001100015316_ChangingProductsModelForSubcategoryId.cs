namespace EcoMarketMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingProductsModelForSubcategoryId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Subcategory_Id", "dbo.Subcategories");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Products", new[] { "Subcategory_Id" });
            RenameColumn(table: "dbo.Products", name: "Subcategory_Id", newName: "SubcategoryId");
            RenameColumn(table: "dbo.Products", name: "CategoryId", newName: "Category_Id");
            AlterColumn("dbo.Products", "Category_Id", c => c.Int());
            AlterColumn("dbo.Products", "SubcategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "SubcategoryId");
            CreateIndex("dbo.Products", "Category_Id");
            AddForeignKey("dbo.Products", "SubcategoryId", "dbo.Subcategories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "Category_Id", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Products", "SubcategoryId", "dbo.Subcategories");
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropIndex("dbo.Products", new[] { "SubcategoryId" });
            AlterColumn("dbo.Products", "SubcategoryId", c => c.Int());
            AlterColumn("dbo.Products", "Category_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Products", name: "Category_Id", newName: "CategoryId");
            RenameColumn(table: "dbo.Products", name: "SubcategoryId", newName: "Subcategory_Id");
            CreateIndex("dbo.Products", "Subcategory_Id");
            CreateIndex("dbo.Products", "CategoryId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "Subcategory_Id", "dbo.Subcategories", "Id");
        }
    }
}
