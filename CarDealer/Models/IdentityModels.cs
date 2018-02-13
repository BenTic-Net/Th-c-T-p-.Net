using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FullName { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Favorite> ToFavorite { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConection1", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
            base.OnModelCreating(modelBuilder);
        }
        // public System.Data.Entity.DbSet<CarDealer.Models.UserViewModel> UserViewModels { get; set; }
        public System.Data.Entity.DbSet<AspController> AspControllers { get; set; }
        public System.Data.Entity.DbSet<AspRoleController> AspRoleControllers { get; set; }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarModel> CarModels { get; set; }
        public virtual DbSet<CarSold> CarSolds { get; set; }
        public virtual DbSet<CarType> CarTypes { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<Footter> Footters { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<New> News { get; set; }
        public virtual DbSet<Manufacture> Manufactures { get; set; }
        public virtual DbSet<CarDetail> CarDetails { get; set; }
        public virtual DbSet<VerifyCode> VerifyCodes { get; set; }
        public virtual DbSet<SystemConfig> SystemConfigs { get; set; }
        public virtual DbSet<Slider> Slides { get; set; }
        public virtual DbSet<Favorite> UFavorite { get; set; }
        //  public System.Data.Entity.DbSet<CarDealer.Models.ApplicationUser> ApplicationUsers { get; set; }
        //public virtual DbSet<User> Users { get; set; }


    }
}