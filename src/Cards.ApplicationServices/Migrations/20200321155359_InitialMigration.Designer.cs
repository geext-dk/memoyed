﻿// <auto-generated />
using System;
using Memoyed.Cards.ApplicationServices.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Memoyed.Cards.ApplicationServices.Migrations
{
    [DbContext(typeof(CardsContext))]
    [Migration("20200321155359_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("Memoyed.Cards.Domain.CardBoxSets.CardBoxSet", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("DbId");

                    b.ToTable("CardBoxSets");
                });

            modelBuilder.Entity("Memoyed.Cards.Domain.CardBoxes.CardBox", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CardBoxSetDbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DbId");

                    b.HasIndex("CardBoxSetDbId");

                    b.ToTable("CardBoxes");
                });

            modelBuilder.Entity("Memoyed.Cards.Domain.Cards.Card", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CardBoxDbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DbId");

                    b.HasIndex("CardBoxDbId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Memoyed.Cards.Domain.RevisionSessions.RevisionSession", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("DbId");

                    b.ToTable("RevisionSessions");
                });

            modelBuilder.Entity("Memoyed.Cards.Domain.RevisionSessions.SessionCards.SessionCard", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RevisionSessionDbId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("DbId");

                    b.HasIndex("RevisionSessionDbId");

                    b.ToTable("SessionCards");
                });

            modelBuilder.Entity("Memoyed.Cards.Domain.CardBoxSets.CardBoxSet", b =>
                {
                    b.OwnsOne("Memoyed.Cards.Domain.CardBoxSets.CardBoxSetId", "Id", b1 =>
                        {
                            b1.Property<int>("CardBoxSetDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("Value")
                                .HasColumnName("Id")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardBoxSetDbId");

                            b1.ToTable("CardBoxSets");

                            b1.WithOwner()
                                .HasForeignKey("CardBoxSetDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.CardBoxSets.CardBoxSetLanguage", "NativeLanguage", b1 =>
                        {
                            b1.Property<int>("CardBoxSetDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("NativeLanguage")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardBoxSetDbId");

                            b1.ToTable("CardBoxSets");

                            b1.WithOwner()
                                .HasForeignKey("CardBoxSetDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.CardBoxSets.CardBoxSetLanguage", "TargetLanguage", b1 =>
                        {
                            b1.Property<int>("CardBoxSetDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("TargetLanguage")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardBoxSetDbId");

                            b1.ToTable("CardBoxSets");

                            b1.WithOwner()
                                .HasForeignKey("CardBoxSetDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.CardBoxSets.CardBoxSetName", "Name", b1 =>
                        {
                            b1.Property<int>("CardBoxSetDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("Name")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardBoxSetDbId");

                            b1.ToTable("CardBoxSets");

                            b1.WithOwner()
                                .HasForeignKey("CardBoxSetDbId");
                        });

                    b.OwnsMany("Memoyed.Cards.Domain.RevisionSessions.RevisionSessionId", "CompletedRevisionSessionIds", b1 =>
                        {
                            b1.Property<int>("CardBoxSetDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("Value")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardBoxSetDbId", "Id");

                            b1.ToTable("CardBoxSets_CompletedRevisionSessionIds");

                            b1.WithOwner()
                                .HasForeignKey("CardBoxSetDbId");
                        });
                });

            modelBuilder.Entity("Memoyed.Cards.Domain.CardBoxes.CardBox", b =>
                {
                    b.HasOne("Memoyed.Cards.Domain.CardBoxSets.CardBoxSet", null)
                        .WithMany("CardBoxes")
                        .HasForeignKey("CardBoxSetDbId");

                    b.OwnsOne("Memoyed.Cards.Domain.CardBoxSets.CardBoxSetId", "SetId", b1 =>
                        {
                            b1.Property<int>("CardBoxDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("Value")
                                .HasColumnName("SetId")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardBoxDbId");

                            b1.ToTable("CardBoxes");

                            b1.WithOwner()
                                .HasForeignKey("CardBoxDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.CardBoxes.CardBoxId", "Id", b1 =>
                        {
                            b1.Property<int>("CardBoxDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("Value")
                                .HasColumnName("Id")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardBoxDbId");

                            b1.ToTable("CardBoxes");

                            b1.WithOwner()
                                .HasForeignKey("CardBoxDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.CardBoxes.CardBoxLevel", "Level", b1 =>
                        {
                            b1.Property<int>("CardBoxDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Value")
                                .HasColumnName("Level")
                                .HasColumnType("INTEGER");

                            b1.HasKey("CardBoxDbId");

                            b1.ToTable("CardBoxes");

                            b1.WithOwner()
                                .HasForeignKey("CardBoxDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.CardBoxes.CardBoxRevisionDelay", "RevisionDelay", b1 =>
                        {
                            b1.Property<int>("CardBoxDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Value")
                                .HasColumnName("RevisionDelay")
                                .HasColumnType("INTEGER");

                            b1.HasKey("CardBoxDbId");

                            b1.ToTable("CardBoxes");

                            b1.WithOwner()
                                .HasForeignKey("CardBoxDbId");
                        });
                });

            modelBuilder.Entity("Memoyed.Cards.Domain.Cards.Card", b =>
                {
                    b.HasOne("Memoyed.Cards.Domain.CardBoxes.CardBox", null)
                        .WithMany("Cards")
                        .HasForeignKey("CardBoxDbId");

                    b.OwnsOne("Memoyed.Cards.Domain.CardBoxes.CardBoxId", "CardBoxId", b1 =>
                        {
                            b1.Property<int>("CardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("Value")
                                .HasColumnName("CardBoxId")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardDbId");

                            b1.ToTable("Cards");

                            b1.WithOwner()
                                .HasForeignKey("CardDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.Cards.CardComment", "Comment", b1 =>
                        {
                            b1.Property<int>("CardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("Comment")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardDbId");

                            b1.ToTable("Cards");

                            b1.WithOwner()
                                .HasForeignKey("CardDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.Cards.CardId", "Id", b1 =>
                        {
                            b1.Property<int>("CardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("Value")
                                .HasColumnName("Id")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardDbId");

                            b1.ToTable("Cards");

                            b1.WithOwner()
                                .HasForeignKey("CardDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.Cards.CardWord", "NativeLanguageWord", b1 =>
                        {
                            b1.Property<int>("CardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("NativeLanguageWord")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardDbId");

                            b1.ToTable("Cards");

                            b1.WithOwner()
                                .HasForeignKey("CardDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.Cards.CardWord", "TargetLanguageWord", b1 =>
                        {
                            b1.Property<int>("CardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("TargetLanguageWord")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardDbId");

                            b1.ToTable("Cards");

                            b1.WithOwner()
                                .HasForeignKey("CardDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.Shared.UtcTime", "CardBoxChangedDate", b1 =>
                        {
                            b1.Property<int>("CardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<DateTime>("Value")
                                .HasColumnName("CardBoxChangeDate")
                                .HasColumnType("TEXT");

                            b1.HasKey("CardDbId");

                            b1.ToTable("Cards");

                            b1.WithOwner()
                                .HasForeignKey("CardDbId");
                        });
                });

            modelBuilder.Entity("Memoyed.Cards.Domain.RevisionSessions.SessionCards.SessionCard", b =>
                {
                    b.HasOne("Memoyed.Cards.Domain.RevisionSessions.RevisionSession", null)
                        .WithMany("SessionCards")
                        .HasForeignKey("RevisionSessionDbId");

                    b.OwnsOne("Memoyed.Cards.Domain.Cards.CardId", "CardId", b1 =>
                        {
                            b1.Property<int>("SessionCardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("Value")
                                .HasColumnName("CardId")
                                .HasColumnType("TEXT");

                            b1.HasKey("SessionCardDbId");

                            b1.ToTable("SessionCards");

                            b1.WithOwner()
                                .HasForeignKey("SessionCardDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.Cards.CardWord", "NativeLanguageWord", b1 =>
                        {
                            b1.Property<int>("SessionCardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("NativeLanguageWord")
                                .HasColumnType("TEXT");

                            b1.HasKey("SessionCardDbId");

                            b1.ToTable("SessionCards");

                            b1.WithOwner()
                                .HasForeignKey("SessionCardDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.Cards.CardWord", "TargetLanguageWord", b1 =>
                        {
                            b1.Property<int>("SessionCardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("TargetLanguageWord")
                                .HasColumnType("TEXT");

                            b1.HasKey("SessionCardDbId");

                            b1.ToTable("SessionCards");

                            b1.WithOwner()
                                .HasForeignKey("SessionCardDbId");
                        });

                    b.OwnsOne("Memoyed.Cards.Domain.RevisionSessions.RevisionSessionId", "SessionId", b1 =>
                        {
                            b1.Property<int>("SessionCardDbId")
                                .HasColumnType("INTEGER");

                            b1.Property<Guid>("Value")
                                .HasColumnName("SessionId")
                                .HasColumnType("TEXT");

                            b1.HasKey("SessionCardDbId");

                            b1.ToTable("SessionCards");

                            b1.WithOwner()
                                .HasForeignKey("SessionCardDbId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}