using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PCLwebb.Models
{
    public class ModelContext : IdentityDbContext<User>
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options) { }

        public DbSet<User> AppUsers { get; set; }
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Client_Has_Project> ClientHasProjects { get; set; }
        public DbSet<ListTask> ListTasks { get; set; }
        public DbSet<Project_Has_Checklist> ProjectHasChecklists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client_Has_Project>()
        .HasKey(chp => new { chp.ProjectID, chp.ClientID });

            modelBuilder.Entity<Client_Has_Project>()
                .HasOne(chp => chp.Client)
                .WithMany(c => c.ClientHasProjects)
                .HasForeignKey(chp => chp.ClientID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client_Has_Project>()
                .HasOne(chp => chp.Project)
                .WithMany(p => p.ClientHasProjects)
                .HasForeignKey(chp => chp.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);
           
            modelBuilder.Entity<Project_Has_Checklist>()
                .HasKey(phc => new { phc.ProjectID, phc.ChecklistID });

            modelBuilder.Entity<Project_Has_Checklist>()
                .HasOne(phc => phc.Project)
                .WithMany(p => p.ProjectHasChecklists)
                .HasForeignKey(phc => phc.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project_Has_Checklist>()
                .HasOne(phc => phc.Checklist)
                .WithMany(c => c.ProjectHasChecklists)
                .HasForeignKey(phc => phc.ChecklistID)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Creator)
                .WithMany(u => u.Clients)
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<ListTask>()
                .HasOne(lt => lt.Checklist)
                .WithMany(cl => cl.ListTasks)
                .HasForeignKey(lt => lt.ChecklistID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Checklist>()
                .HasOne(cl => cl.Creator)
                .WithMany(u => u.Checklists)
                .HasForeignKey(cl => cl.CreatorID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Checklist>()
                .HasOne(cl => cl.ParentChecklist)
                .WithMany()
                .HasForeignKey(cl => cl.ParentChecklistID)
                .OnDelete(DeleteBehavior.Restrict); // För att undvika oavsiktlig radering av mallar


            modelBuilder.Entity<Project>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Projects) 
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
