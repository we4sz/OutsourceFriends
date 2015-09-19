namespace OutsourceFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Guides",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 256),
                        CurrentTitle = c.String(maxLength: 256),
                        Removed = c.Boolean(nullable: false),
                        UpdatedTime = c.DateTime(),
                        LastActive = c.DateTime(),
                        Age = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 256),
                        CreatedTime = c.DateTime(nullable: false),
                        Location = c.Geography(),
                        Description = c.String(maxLength: 256),
                        CreatedInLanguage = c.String(maxLength: 2),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId, unique: true)
                .Index(t => t.Name)
                .Index(t => t.Removed)
                .Index(t => t.UpdatedTime)
                .Index(t => t.LastActive)
                .Index(t => t.Age)
                .Index(t => t.ImageUrl);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserLanguage = c.String(maxLength: 2),
                        Emails = c.Boolean(nullable: false),
                        Removed = c.Boolean(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        CreatedTime = c.DateTime(),
                        LastActive = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserLanguage)
                .Index(t => t.Removed)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Travelers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 256),
                        CurrentTitle = c.String(maxLength: 256),
                        Removed = c.Boolean(nullable: false),
                        UpdatedTime = c.DateTime(),
                        LastActive = c.DateTime(),
                        Age = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 256),
                        CreatedTime = c.DateTime(nullable: false),
                        Location = c.Geography(),
                        Description = c.String(maxLength: 256),
                        CreatedInLanguage = c.String(maxLength: 2),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId, unique: true)
                .Index(t => t.Name)
                .Index(t => t.Removed)
                .Index(t => t.UpdatedTime)
                .Index(t => t.LastActive)
                .Index(t => t.Age)
                .Index(t => t.ImageUrl);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Guides", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Travelers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Travelers", new[] { "ImageUrl" });
            DropIndex("dbo.Travelers", new[] { "Age" });
            DropIndex("dbo.Travelers", new[] { "LastActive" });
            DropIndex("dbo.Travelers", new[] { "UpdatedTime" });
            DropIndex("dbo.Travelers", new[] { "Removed" });
            DropIndex("dbo.Travelers", new[] { "Name" });
            DropIndex("dbo.Travelers", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "Removed" });
            DropIndex("dbo.AspNetUsers", new[] { "UserLanguage" });
            DropIndex("dbo.Guides", new[] { "ImageUrl" });
            DropIndex("dbo.Guides", new[] { "Age" });
            DropIndex("dbo.Guides", new[] { "LastActive" });
            DropIndex("dbo.Guides", new[] { "UpdatedTime" });
            DropIndex("dbo.Guides", new[] { "Removed" });
            DropIndex("dbo.Guides", new[] { "Name" });
            DropIndex("dbo.Guides", new[] { "UserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Travelers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Guides");
        }
    }
}
