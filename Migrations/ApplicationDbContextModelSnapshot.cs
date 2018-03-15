﻿// <auto-generated />
using ArtMapApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ArtMapApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("ArtMapApi.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("PostId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.HasKey("CommentId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("ArtMapApi.Models.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("PostId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.HasKey("LikeId");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("ArtMapApi.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<int>("CommentsCount");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

                    b.Property<decimal>("Lat");

                    b.Property<int>("LikesCount");

                    b.Property<decimal>("Long");

                    b.Property<string>("PhotoId")
                        .IsRequired();

                    b.Property<string>("PhotoURI")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("PostId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("ArtMapApi.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Avatar");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<long>("FacebookId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}
