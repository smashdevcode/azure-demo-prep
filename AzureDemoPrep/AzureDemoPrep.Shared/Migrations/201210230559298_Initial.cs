namespace AzureDemoPrep.Shared.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Video",
                c => new
                    {
                        VideoID = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false, maxLength: 255),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        AddedOn = c.DateTime(nullable: false),
                        VideoStatus = c.Int(nullable: false),
                        MediaServicesJobID = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.VideoID);
            
            CreateTable(
                "dbo.VideoAsset",
                c => new
                    {
                        VideoAssetID = c.Int(nullable: false, identity: true),
                        VideoID = c.Int(nullable: false),
                        FileType = c.Int(nullable: false),
                        MediaServicesAssetID = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.VideoAssetID)
                .ForeignKey("dbo.Video", t => t.VideoID, cascadeDelete: true)
                .Index(t => t.VideoID);            
        }
        
        public override void Down()
        {
            DropIndex("dbo.VideoAsset", new[] { "VideoID" });
            DropForeignKey("dbo.VideoAsset", "VideoID", "dbo.Video");
            DropTable("dbo.VideoAsset");
            DropTable("dbo.Video");
        }
    }
}
