﻿using Memoyed.Domain.Cards.Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memoyed.Application.DataModel.Mappings
{
    public class CardEntityMap : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey("DbId");

            builder.OwnsOne(c => c.Id,
                a => a.Property(c => c.Value).HasColumnName("Id"));
            builder.OwnsOne(c => c.TargetLanguageWord,
                a => a.Property(c => c.Value).HasColumnName("TargetLanguageWord"));
            builder.OwnsOne(c => c.NativeLanguageWord,
                a => a.Property(c => c.Value).HasColumnName("NativeLanguageWord"));
            builder.OwnsOne(c => c.Comment,
                a => a.Property(c => c.Value).HasColumnName("Comment"));
            builder.OwnsOne(c => c.CardBoxId,
                a => a.Property(c => c.Value).HasColumnName("CardBoxId"));
            builder.OwnsOne(c => c.CardBoxChangedDate,
                a => a.Property(c => c.Value).HasColumnName("CardBoxChangeDate"));
        }
    }
}