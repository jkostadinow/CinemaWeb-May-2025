

using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Data.Models
{
    [Comment("Movie in the system")]
    public class Movie
    {
        [Comment("Movie identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Movie title")]
        public string Title { get; set; } = null!;

        [Comment("Movie genre")]
        public string Genre { get; set; } = null!;

        [Comment("Movie release date")]
        public DateOnly ReleaseDate { get; set; }

        [Comment("Movie director")]
        public string Director { get; set; } = null!;

        [Comment("Movie duration in minutes")]
        public int Duration { get; set; }

        [Comment("Movie description")]
        public string Description { get; set; } = null!;

        [Comment("Movie image URL")]
        public string? ImageUrl { get; set; }

        [Comment("Indicates if the movie is deleted")]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<ApplicationUserMovie> UserWatchlists { get; set; } = new HashSet<ApplicationUserMovie>();

    }
}
