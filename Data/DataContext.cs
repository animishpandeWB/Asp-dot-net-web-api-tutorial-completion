using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    //DATACONTEXT BRINGS ALL THE DATA AND ALLOWS US TO MANIPULATE IT
    //DATACONTEXT IS GOING TO TIE ALL THE DATABASE TABLES TOGETHER
    //THIS WILL GIVE US A CONTEXT THROUGH WHICH WE CAN ACCESS ALL THE DATA QUICKLY
    public class DataContext : DbContext
    {
        //BASE IS GOING TO PUSH ALL THE DATA THAT IS BEING RECIEVED FROM OUTSIDE CLASS
        //UP INTO THE DBCONTEXT, BASICALLY PERFORMING INHERITANCE
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
            
        }

        //TELLING THE DBCONTEXT ABOUT ALL THE MODELS IN OUR APPLICATION
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Country> Countries{ get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PokemonOwner> PokemonOwners{ get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ONMODELCREATING IS USED TO MANIPULATE THE DATABASE TABLES WITHOUT ALTERING THE ACTUAL TABLE
            //WE ARE TELLING ENTITY FRAMEWORK THAT WE NEED TO LINK THESE TWO IDS TOGETHER
            //OTHERWISE ENTITY FRAMEWORK IS NOT GOING TO KNOW YOU WANT TO LINK THESE 2 IDS TOGETHER
            //ELSE THE RELATIONSHIP IS NOT GOING TO EXIST
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId });
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<PokemonOwner>()
                .HasKey(po => new { po.PokemonId, po.OwnerId });
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Owner)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(o => o.OwnerId);

        }
    }
}
