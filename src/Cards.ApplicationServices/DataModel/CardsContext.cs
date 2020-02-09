using Dapper;
using Memoyed.Cards.ApplicationServices.DataModel.Mappings;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.RevisionSessions;
using Memoyed.Cards.Domain.RevisionSessions.SessionCards;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.Cards.ApplicationServices.DataModel
{
    public class CardsContext : DbContext
    {
        public DbSet<CardBoxSet> CardBoxSets { get; set; }
        public DbSet<CardBox> CardBoxes { get; set; }
        public DbSet<LearningCard> LearningCards { get; set; }
        
        public DbSet<RevisionSession> RevisionSessions { get; set; }
        public DbSet<SessionCard> SessionCards { get; set; }

        public CardsContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new CardBoxSetEntityMap());
            modelBuilder.ApplyConfiguration(new CardBoxEntityMap());
            modelBuilder.ApplyConfiguration(new LearningCardEntityMap());
            
            modelBuilder.ApplyConfiguration(new RevisionSessionEntityMap());
            modelBuilder.ApplyConfiguration(new SessionCardEntityMap());
        }
    }
}