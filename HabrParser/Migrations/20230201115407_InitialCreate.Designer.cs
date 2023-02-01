﻿// <auto-generated />
using System;
using HabrParser.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HabrParser.Migrations
{
    [DbContext(typeof(ArticlesDBContext))]
    [Migration("20230201115407_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

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

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.Flow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FlowId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TitleHtml")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FlowId");

                    b.ToTable("Flow");
                });

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.Hub", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

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

                    b.ToTable("Hub");
                });

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.LeadData", b =>
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

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.ParsedArticle", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArticleId"));

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

                    b.Property<int>("Id")
                        .HasColumnType("int");

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

                    b.HasKey("ArticleId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("LeadDataId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.ParserResult", b =>
                {
                    b.Property<int>("ParserResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParserResultId"));

                    b.Property<int>("LastArtclelId")
                        .HasColumnType("int");

                    b.HasKey("ParserResultId");

                    b.ToTable("ParserResult");
                });

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TitleHtml")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.Flow", b =>
                {
                    b.HasOne("HabrParser.Models.ArticleOnly.Flow", null)
                        .WithMany("Flows")
                        .HasForeignKey("FlowId");
                });

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.ParsedArticle", b =>
                {
                    b.HasOne("HabrParser.Models.ArticleOnly.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("HabrParser.Models.ArticleOnly.LeadData", "LeadData")
                        .WithMany()
                        .HasForeignKey("LeadDataId");

                    b.Navigation("Author");

                    b.Navigation("LeadData");
                });

            modelBuilder.Entity("HabrParser.Models.ArticleOnly.Flow", b =>
                {
                    b.Navigation("Flows");
                });
#pragma warning restore 612, 618
        }
    }
}
