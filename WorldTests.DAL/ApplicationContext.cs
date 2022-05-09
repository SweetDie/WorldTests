using System.Configuration;
using Microsoft.EntityFrameworkCore;
using WorldTests.DAL.Entities;

namespace WorldTests.DAL
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connectionString =
               //@"data source=(localdb)\MSSqlLocalDb;initial catalog=WorldTests;integrated security=True;";
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString);
        }

        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Test
            var testBuilder = modelBuilder.Entity<Test>();
            testBuilder.HasKey(t => t.Id);
            testBuilder.Property(t => t.Name).IsRequired();

            // Question
            var questionBuilder = modelBuilder.Entity<Question>();
            questionBuilder.HasKey(x => x.Id);
            questionBuilder.Property(t => t.Name).IsRequired();

            questionBuilder.HasOne(q => q.Test).WithMany(t => t.Questions).HasForeignKey(q => q.TestId);

            // Answer
            var answerBuilder = modelBuilder.Entity<Answer>();
            answerBuilder.HasKey(a => a.Id);
            answerBuilder.Property(a => a.Name).IsRequired();
            answerBuilder.Property(a => a.IsCorrect).IsRequired();

            answerBuilder.HasOne(a => a.Question).WithMany(q => q.Answers).HasForeignKey(a => a.QuestionId);

            // User
            var userBuilder = modelBuilder.Entity<User>();
            userBuilder.HasKey(u => u.Id);
            userBuilder.Property(u => u.Email).IsRequired();
            userBuilder.HasIndex(u => u.Email).IsUnique();

            // Credential
            var credentialBuilder = modelBuilder.Entity<Credential>();
            credentialBuilder.HasKey(c => c.Id);
            credentialBuilder.Property(c => c.Password).IsRequired();
            credentialBuilder.Property(c => c.Username).IsRequired();
            credentialBuilder.HasIndex(c => c.Username).IsUnique();

            credentialBuilder.HasOne(c => c.User).WithOne(u => u.Credential).HasForeignKey<User>(u => u.CredentialId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
