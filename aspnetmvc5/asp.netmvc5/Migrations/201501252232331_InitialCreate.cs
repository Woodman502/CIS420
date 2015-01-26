namespace asp.netmvc5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vaccines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Vendor_ = c.String(),
                        Dosage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date_Added = c.DateTime(nullable: false),
                        Date_Expire = c.DateTime(nullable: false),
                        Barcode = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vaccines");
        }
    }
}
