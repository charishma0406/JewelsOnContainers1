using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Data
{
    //this class is for the catalog data is storing in the data base, db context is coming from entity framework

    public class CatalogContext : DbContext
    {
        //creating a constructor for to know the data "where" we want to deploy  
        //in the parameters we gave dependency dbcontextoptions for where the sql db information it is depending on the db context
        //in the parameter it will ask which type of db is that (its just db its gono come and talk to it)
        //they are getting injected  
        //for data base connection we are giving the parameters in the constructor 
        //it is inheriting the base class constructor also
        public CatalogContext(DbContextOptions options) :base(options)
        {

        }

        //what to convert into database tables(the three models are converted to db table)
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }



        /*how to do this means overriding the parent proprties and building 
        the table onmodel creating is the inbuilt method to build tables in db*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogBrand>(e =>
            {
                e.ToTable("CatalogBrands");
                e.Property(b => b.Id)
                .IsRequired()
                .UseHiLo("Catalog_Brand_hilo");

                e.Property(b => b.Brand)
                .IsRequired()
                .HasMaxLength(100);

            });

            //model builder for ccatalog type model
            modelBuilder.Entity<CatalogType>(e =>
            {
                e.Property(e => e.Id)
                .IsRequired()
                .UseHiLo("Catalog_type_hilo");

                e.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(100);
            });

            //model builder for catalog item model
            modelBuilder.Entity<CatalogItem>(e =>
            {
                e.Property(c => c.Id)
                .IsRequired()
                .UseHiLo("Catalog_hilo");

                e.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

                e.Property(c => c.Price)
                .IsRequired();

                //it has one to many relation ship for example one ring can be many types
                //the relationship is mainted with catalag type id
                //it has one relation ship with catalog type table through the foreign key
                e.HasOne(c => c.CatalogType)
                .WithMany()
                .HasForeignKey(c => c.CatalogTypeId);

                //it has one and many relationship for example one necklace can be many brands
                //the relationship is maintained with catalog brand id.
                e.HasOne(c => c.CatalogBrand)
                .WithMany()
                .HasForeignKey(c => c.CatalogBrandId);
            });

        }
    }
}
