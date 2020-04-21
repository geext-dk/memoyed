using Memoyed.Application.DataModel.Mappings;
using Memoyed.Domain.Cards.CardBoxes;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;
using Memoyed.Domain.Users.Users;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.Application.DataModel
{
    public class CardsContext : DbContext
    {
        public CardsContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CardBoxSet> CardBoxSets { get; set; }
        public DbSet<CardBox> CardBoxes { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<RevisionSession> RevisionSessions { get; set; }
        public DbSet<SessionCard> SessionCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CardBoxSetEntityMap());
            modelBuilder.ApplyConfiguration(new CardBoxEntityMap());
            modelBuilder.ApplyConfiguration(new CardEntityMap());

            modelBuilder.ApplyConfiguration(new RevisionSessionEntityMap());
            modelBuilder.ApplyConfiguration(new SessionCardEntityMap());

            modelBuilder.ApplyConfiguration(new UserEntityMap());
        }
    }
}