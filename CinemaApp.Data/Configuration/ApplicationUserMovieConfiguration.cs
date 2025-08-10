

using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
    public class ApplicationUserMovieConfiguration : IEntityTypeConfiguration<ApplicationUserMovie>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserMovie> entity)
        {
            entity
                .HasKey(aum => new { aum.ApplicationUserId, aum.MovieId });

            entity
                .Property(aum => aum.ApplicationUserId)
                .IsRequired();

            entity
                .Property(aum => aum.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(aum => aum.ApplicationUser)
                .WithMany()
                .HasForeignKey(aum => aum.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
                

            entity
                .HasOne(aum => aum.Movie)
                .WithMany(m => m.UserWatchlists)
                .HasForeignKey(aum => aum.MovieId)
                .OnDelete(DeleteBehavior.Restrict);
                

            entity.HasQueryFilter(aum => !aum.IsDeleted && !aum.Movie.IsDeleted);


        }


    }
}
