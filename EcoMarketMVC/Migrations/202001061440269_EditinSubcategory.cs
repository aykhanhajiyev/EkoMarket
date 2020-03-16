namespace EcoMarketMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditinSubcategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subcategories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Subcategories", new[] { "Category_Id" });
            RenameColumn(table: "dbo.Subcategories", name: "Category_Id", newName: "CategoryId");
            AlterColumn("dbo.Subcategories", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Subcategories", "CategoryId");
            AddForeignKey("dbo.Subcategories", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subcategories", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Subcategories", new[] { "CategoryId" });
            AlterColumn("dbo.Subcategories", "CategoryId", c => c.Int());
            RenameColumn(table: "dbo.Subcategories", name: "CategoryId", newName: "Category_Id");
            CreateIndex("dbo.Subcategories", "Category_Id");
            AddForeignKey("dbo.Subcategories", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
