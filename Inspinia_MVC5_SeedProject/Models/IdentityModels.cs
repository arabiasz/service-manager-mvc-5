using Microsoft.AspNet.Identity.EntityFramework;

namespace Inspinia_MVC5_SeedProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Group> Groups { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Producer> Producers { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.DevicesFolder> DevicesFolders { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Device> Devices { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Module> Modules { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Dealer> Dealers { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.TaxOffice> TaxOffices { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.Fiscalization> Fiscalizations { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.FiscalizationToModules> FiscalizationsToModules { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.InterimReview> InterimReviews { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.ServiceIntervention> ServiceInterventions { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5_SeedProject.Models.ReadingOfFiscalMemory> ReadingOfFiscalMemories { get; set; }
    }
}