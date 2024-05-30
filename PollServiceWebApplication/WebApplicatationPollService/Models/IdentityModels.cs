using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplicatationPollService.Models.Entities;

namespace WebApplicatationPollService.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<PollAnswersEntity> VotedPoll { get; set; }
        public virtual ICollection<PollEntity> CreatedPoll { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) {

        }
        public virtual DbSet<PollEntity> Polls { get; set; }
        public virtual DbSet<PollAnswersEntity> PollAnswers { get; set; }
        public virtual DbSet<SessionUserPrivatePollEntity> SessionPrivatePoll { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            //deleteting from table two field which will be not used
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.PhoneNumber)
                .Ignore(x => x.PhoneNumberConfirmed);
        }
        
    }

}