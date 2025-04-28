using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.MigrationsRunner.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Etiquettes",
                columns: table => new
                {
                    IdEtiquette = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Libelle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquettes", x => x.IdEtiquette);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Projets",
                columns: table => new
                {
                    IdProjet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomProjet = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescriptionProjet = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projets", x => x.IdProjet);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    IdUtilisateur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prenom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotDePasse = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.IdUtilisateur);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Taches",
                columns: table => new
                {
                    IdTache = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateCreation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Echeance = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priorite = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdAuteur = table.Column<int>(type: "int", nullable: false),
                    IdRealisateur = table.Column<int>(type: "int", nullable: false),
                    Categorie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taches", x => x.IdTache);
                    table.ForeignKey(
                        name: "FK_Taches_Utilisateurs_IdAuteur",
                        column: x => x.IdAuteur,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUtilisateur",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Taches_Utilisateurs_IdRealisateur",
                        column: x => x.IdRealisateur,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUtilisateur",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Commentaires",
                columns: table => new
                {
                    IdCommentaire = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTache = table.Column<int>(type: "int", nullable: false),
                    TacheIdTache = table.Column<int>(type: "int", nullable: false),
                    IdAuteur = table.Column<int>(type: "int", nullable: false),
                    AuteurIdUtilisateur = table.Column<int>(type: "int", nullable: false),
                    DateCommentaire = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Contenu = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commentaires", x => x.IdCommentaire);
                    table.ForeignKey(
                        name: "FK_Commentaires_Taches_TacheIdTache",
                        column: x => x.TacheIdTache,
                        principalTable: "Taches",
                        principalColumn: "IdTache",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commentaires_Utilisateurs_AuteurIdUtilisateur",
                        column: x => x.AuteurIdUtilisateur,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUtilisateur",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistoriqueModifications",
                columns: table => new
                {
                    IdModification = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTache = table.Column<int>(type: "int", nullable: false),
                    TacheIdTache = table.Column<int>(type: "int", nullable: false),
                    IdAuteur = table.Column<int>(type: "int", nullable: false),
                    AuteurIdUtilisateur = table.Column<int>(type: "int", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DescriptionModif = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriqueModifications", x => x.IdModification);
                    table.ForeignKey(
                        name: "FK_HistoriqueModifications_Taches_TacheIdTache",
                        column: x => x.TacheIdTache,
                        principalTable: "Taches",
                        principalColumn: "IdTache",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoriqueModifications_Utilisateurs_AuteurIdUtilisateur",
                        column: x => x.AuteurIdUtilisateur,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUtilisateur",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SousTaches",
                columns: table => new
                {
                    IdSousTache = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTacheParent = table.Column<int>(type: "int", nullable: false),
                    TacheParentIdTache = table.Column<int>(type: "int", nullable: false),
                    Titre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Statut = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Echeance = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SousTaches", x => x.IdSousTache);
                    table.ForeignKey(
                        name: "FK_SousTaches_Taches_TacheParentIdTache",
                        column: x => x.TacheParentIdTache,
                        principalTable: "Taches",
                        principalColumn: "IdTache",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TachesEtiquettes",
                columns: table => new
                {
                    IdTache = table.Column<int>(type: "int", nullable: false),
                    IdEtiquette = table.Column<int>(type: "int", nullable: false),
                    TacheIdTache = table.Column<int>(type: "int", nullable: false),
                    EtiquetteIdEtiquette = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TachesEtiquettes", x => new { x.IdTache, x.IdEtiquette });
                    table.ForeignKey(
                        name: "FK_TachesEtiquettes_Etiquettes_EtiquetteIdEtiquette",
                        column: x => x.EtiquetteIdEtiquette,
                        principalTable: "Etiquettes",
                        principalColumn: "IdEtiquette",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TachesEtiquettes_Taches_TacheIdTache",
                        column: x => x.TacheIdTache,
                        principalTable: "Taches",
                        principalColumn: "IdTache",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TachesProjets",
                columns: table => new
                {
                    IdTache = table.Column<int>(type: "int", nullable: false),
                    IdProjet = table.Column<int>(type: "int", nullable: false),
                    TacheIdTache = table.Column<int>(type: "int", nullable: false),
                    ProjetIdProjet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TachesProjets", x => new { x.IdTache, x.IdProjet });
                    table.ForeignKey(
                        name: "FK_TachesProjets_Projets_ProjetIdProjet",
                        column: x => x.ProjetIdProjet,
                        principalTable: "Projets",
                        principalColumn: "IdProjet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TachesProjets_Taches_TacheIdTache",
                        column: x => x.TacheIdTache,
                        principalTable: "Taches",
                        principalColumn: "IdTache",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Commentaires_AuteurIdUtilisateur",
                table: "Commentaires",
                column: "AuteurIdUtilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_Commentaires_TacheIdTache",
                table: "Commentaires",
                column: "TacheIdTache");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueModifications_AuteurIdUtilisateur",
                table: "HistoriqueModifications",
                column: "AuteurIdUtilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueModifications_TacheIdTache",
                table: "HistoriqueModifications",
                column: "TacheIdTache");

            migrationBuilder.CreateIndex(
                name: "IX_SousTaches_TacheParentIdTache",
                table: "SousTaches",
                column: "TacheParentIdTache");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_IdAuteur",
                table: "Taches",
                column: "IdAuteur");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_IdRealisateur",
                table: "Taches",
                column: "IdRealisateur");

            migrationBuilder.CreateIndex(
                name: "IX_TachesEtiquettes_EtiquetteIdEtiquette",
                table: "TachesEtiquettes",
                column: "EtiquetteIdEtiquette");

            migrationBuilder.CreateIndex(
                name: "IX_TachesEtiquettes_TacheIdTache",
                table: "TachesEtiquettes",
                column: "TacheIdTache");

            migrationBuilder.CreateIndex(
                name: "IX_TachesProjets_ProjetIdProjet",
                table: "TachesProjets",
                column: "ProjetIdProjet");

            migrationBuilder.CreateIndex(
                name: "IX_TachesProjets_TacheIdTache",
                table: "TachesProjets",
                column: "TacheIdTache");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commentaires");

            migrationBuilder.DropTable(
                name: "HistoriqueModifications");

            migrationBuilder.DropTable(
                name: "SousTaches");

            migrationBuilder.DropTable(
                name: "TachesEtiquettes");

            migrationBuilder.DropTable(
                name: "TachesProjets");

            migrationBuilder.DropTable(
                name: "Etiquettes");

            migrationBuilder.DropTable(
                name: "Projets");

            migrationBuilder.DropTable(
                name: "Taches");

            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
