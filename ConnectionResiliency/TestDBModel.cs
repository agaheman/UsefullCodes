namespace ConnectionResiliency
{
    using System.Data.Entity;

    public partial class TestDbModel : DbContext
    {
        public TestDbModel()
            : base("name=TestDBModel")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TestDbModel,MigrationConfiguration>());
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
