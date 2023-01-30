﻿// <auto-generated />
using System;
using FirstProject.ArticlesAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FirstProject.ArticlesAPI.Migrations
{
    [DbContext(typeof(ArticlesDBContext))]
    partial class ArticlesDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<bool?>("Checked")
                        .HasColumnType("bit");

                    b.Property<bool>("CommentsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("EditorVersion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Format")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasPinnedComments")
                        .HasColumnType("bit");

                    b.Property<string>("ImageLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCorporative")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEditorial")
                        .HasColumnType("bit");

                    b.Property<string>("Lang")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LeadDataId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PlannedPublishTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelatedData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RulesRemindEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextHtml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TimePublished")
                        .HasColumnType("datetime2");

                    b.Property<string>("TitleHtml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("VotesEnabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("LeadDataId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("RelatedData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Speciality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Flow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("FlowId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TitleHtml")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("FlowId");

                    b.ToTable("Flow");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Hub", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsLoading")
                        .HasColumnType("bit");

                    b.Property<bool>("IsProfiled")
                        .HasColumnType("bit");

                    b.Property<string>("RelatedData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TitleHtml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Hub");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.LeadData", b =>
                {
                    b.Property<int>("LeadDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeadDataId"));

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ButtonTextHtml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextHtml")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LeadDataId");

                    b.ToTable("LeadData");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid?>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TitleHtml")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Article", b =>
                {
                    b.HasOne("FirstProject.ArticlesAPI.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("FirstProject.ArticlesAPI.Models.LeadData", "LeadData")
                        .WithMany()
                        .HasForeignKey("LeadDataId");

                    b.Navigation("Author");

                    b.Navigation("LeadData");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Flow", b =>
                {
                    b.HasOne("FirstProject.ArticlesAPI.Models.Article", null)
                        .WithMany("Flows")
                        .HasForeignKey("ArticleId");

                    b.HasOne("FirstProject.ArticlesAPI.Models.Flow", null)
                        .WithMany("Flows")
                        .HasForeignKey("FlowId");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Hub", b =>
                {
                    b.HasOne("FirstProject.ArticlesAPI.Models.Article", null)
                        .WithMany("Hubs")
                        .HasForeignKey("ArticleId");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Tag", b =>
                {
                    b.HasOne("FirstProject.ArticlesAPI.Models.Article", null)
                        .WithMany("Tags")
                        .HasForeignKey("ArticleId");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.User", b =>
                {
                    b.HasOne("FirstProject.ArticlesAPI.Models.Article", null)
                        .WithMany("Likes")
                        .HasForeignKey("ArticleId");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Article", b =>
                {
                    b.Navigation("Flows");

                    b.Navigation("Hubs");

                    b.Navigation("Likes");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("FirstProject.ArticlesAPI.Models.Flow", b =>
                {
                    b.Navigation("Flows");
                });
#pragma warning restore 612, 618
        }
    }
}