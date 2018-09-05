namespace AgahClassLibrary
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AgahTestModel : DbContext
    {
        public AgahTestModel()
            : base("name=AgahTestModel")
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(e => e.ISBN)
                .IsFixedLength();

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Person)
                .HasForeignKey(e => e.AuthorPersonId)
                .WillCascadeOnDelete(false);
        }
    }
}
