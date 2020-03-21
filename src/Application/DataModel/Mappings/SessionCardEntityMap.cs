﻿using Memoyed.Domain.Cards.RevisionSessions.SessionCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class SessionCardEntityMap : IEntityTypeConfiguration<SessionCard>
    {
        public void Configure(EntityTypeBuilder<SessionCard> builder)
        {
            builder.HasKey("DbId");
            builder.OwnsOne(c => c.SessionId,
                a => a.Property(c => c.Value).HasColumnName("SessionId"));
            builder.OwnsOne(c => c.CardId,
                a => a.Property(c => c.Value).HasColumnName("CardId"));
            builder.OwnsOne(c => c.NativeLanguageWord,
                a => a.Property(c => c.Value).HasColumnName("NativeLanguageWord"));
            builder.OwnsOne(c => c.TargetLanguageWord,
                a => a.Property(c => c.Value).HasColumnName("TargetLanguageWord"));
        }
    }
}