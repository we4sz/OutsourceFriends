namespace OutsourceFriends.Migrations
{
    using CsvHelper;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OutsourceFriends.Context.DomainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }



        protected override void Seed(OutsourceFriends.Context.DomainContext context)
        {
           
        }
    }
}
