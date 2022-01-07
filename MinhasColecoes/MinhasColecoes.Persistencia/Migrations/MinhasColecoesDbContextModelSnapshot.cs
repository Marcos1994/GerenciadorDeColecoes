﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinhasColecoes.Persistencia.Context;

namespace MinhasColecoes.Persistencia.Migrations
{
    [DbContext(typeof(MinhasColecoesDbContext))]
    partial class MinhasColecoesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.Colecao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdColecaoMaior")
                        .HasColumnType("int");

                    b.Property<int>("IdDono")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Publica")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("IdColecaoMaior");

                    b.HasIndex("IdDono");

                    b.ToTable("Colecoes");
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.ColecaoUsuario", b =>
                {
                    b.Property<int>("IdColecao")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("IdColecao", "IdUsuario");

                    b.HasIndex("IdUsuario");

                    b.ToTable("ColecoesUsuario");
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Codigo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdColecao")
                        .HasColumnType("int");

                    b.Property<int?>("IdDonoParticular")
                        .HasColumnType("int");

                    b.Property<int?>("IdOriginal")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Original")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("IdColecao");

                    b.HasIndex("IdDonoParticular");

                    b.HasIndex("IdOriginal")
                        .IsUnique()
                        .HasFilter("[IdOriginal] IS NOT NULL");

                    b.ToTable("Itens");
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.ItemUsuario", b =>
                {
                    b.Property<int>("IdItem")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Comentario")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Relacao")
                        .HasColumnType("int");

                    b.HasKey("IdItem", "IdUsuario");

                    b.HasIndex("IdUsuario");

                    b.ToTable("ItensUsuario");
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Senha")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.Colecao", b =>
                {
                    b.HasOne("MinhasColecoes.Persistencia.Entities.Colecao", null)
                        .WithMany("Colecoes")
                        .HasForeignKey("IdColecaoMaior")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MinhasColecoes.Persistencia.Entities.Usuario", null)
                        .WithMany("ColecoesDono")
                        .HasForeignKey("IdDono")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.ColecaoUsuario", b =>
                {
                    b.HasOne("MinhasColecoes.Persistencia.Entities.Colecao", null)
                        .WithMany("UsuariosColecao")
                        .HasForeignKey("IdColecao")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinhasColecoes.Persistencia.Entities.Usuario", null)
                        .WithMany("ColecoesParticipa")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.Item", b =>
                {
                    b.HasOne("MinhasColecoes.Persistencia.Entities.Colecao", null)
                        .WithMany("Itens")
                        .HasForeignKey("IdColecao")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinhasColecoes.Persistencia.Entities.Usuario", null)
                        .WithMany("ItensDono")
                        .HasForeignKey("IdDonoParticular")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MinhasColecoes.Persistencia.Entities.Item", "ItemOriginal")
                        .WithOne()
                        .HasForeignKey("MinhasColecoes.Persistencia.Entities.Item", "IdOriginal")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ItemOriginal");
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.ItemUsuario", b =>
                {
                    b.HasOne("MinhasColecoes.Persistencia.Entities.Item", null)
                        .WithMany("RelacoesUsuarios")
                        .HasForeignKey("IdItem")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinhasColecoes.Persistencia.Entities.Usuario", null)
                        .WithMany("RelacoesItens")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.Colecao", b =>
                {
                    b.Navigation("Colecoes");

                    b.Navigation("Itens");

                    b.Navigation("UsuariosColecao");
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.Item", b =>
                {
                    b.Navigation("RelacoesUsuarios");
                });

            modelBuilder.Entity("MinhasColecoes.Persistencia.Entities.Usuario", b =>
                {
                    b.Navigation("ColecoesDono");

                    b.Navigation("ColecoesParticipa");

                    b.Navigation("ItensDono");

                    b.Navigation("RelacoesItens");
                });
#pragma warning restore 612, 618
        }
    }
}
