using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarGear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "medico",
                columns: table => new
                {
                    id_medico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    crm = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    especialidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medico", x => x.id_medico);
                });

            migrationBuilder.CreateTable(
                name: "passageiro",
                columns: table => new
                {
                    id_passageiro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    idade = table.Column<int>(type: "int", nullable: true),
                    status_medico = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passageiro", x => x.id_passageiro);
                });

            migrationBuilder.CreateTable(
                name: "historico_medico",
                columns: table => new
                {
                    id_historico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_passageiro = table.Column<int>(type: "int", nullable: false),
                    diagnostico = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    dt_registro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historico_medico", x => x.id_historico);
                    table.ForeignKey(
                        name: "FK_historico_medico_passageiro_id_passageiro",
                        column: x => x.id_passageiro,
                        principalTable: "passageiro",
                        principalColumn: "id_passageiro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "traje",
                columns: table => new
                {
                    id_traje = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_passageiro = table.Column<int>(type: "int", nullable: false),
                    codigo_rfid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dt_alocacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_traje", x => x.id_traje);
                    table.ForeignKey(
                        name: "FK_traje_passageiro_id_passageiro",
                        column: x => x.id_passageiro,
                        principalTable: "passageiro",
                        principalColumn: "id_passageiro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "leitura_sensor",
                columns: table => new
                {
                    id_leitura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_traje = table.Column<int>(type: "int", nullable: false),
                    temperatura = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    humidade = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    batimentos = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    dt_leitura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leitura_sensor", x => x.id_leitura);
                    table.ForeignKey(
                        name: "FK_leitura_sensor_traje_id_traje",
                        column: x => x.id_traje,
                        principalTable: "traje",
                        principalColumn: "id_traje",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alerta_emergencia",
                columns: table => new
                {
                    id_alerta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_leitura = table.Column<int>(type: "int", nullable: false),
                    id_medico = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    nivel_gravidade = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    resolvido = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    dt_alerta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alerta_emergencia", x => x.id_alerta);
                    table.ForeignKey(
                        name: "FK_alerta_emergencia_leitura_sensor_id_leitura",
                        column: x => x.id_leitura,
                        principalTable: "leitura_sensor",
                        principalColumn: "id_leitura",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_alerta_emergencia_medico_id_medico",
                        column: x => x.id_medico,
                        principalTable: "medico",
                        principalColumn: "id_medico",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alerta_emergencia_id_leitura",
                table: "alerta_emergencia",
                column: "id_leitura");

            migrationBuilder.CreateIndex(
                name: "IX_alerta_emergencia_id_medico",
                table: "alerta_emergencia",
                column: "id_medico");

            migrationBuilder.CreateIndex(
                name: "IX_historico_medico_id_passageiro",
                table: "historico_medico",
                column: "id_passageiro");

            migrationBuilder.CreateIndex(
                name: "IX_leitura_sensor_id_traje",
                table: "leitura_sensor",
                column: "id_traje");

            migrationBuilder.CreateIndex(
                name: "IX_traje_id_passageiro",
                table: "traje",
                column: "id_passageiro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alerta_emergencia");

            migrationBuilder.DropTable(
                name: "historico_medico");

            migrationBuilder.DropTable(
                name: "leitura_sensor");

            migrationBuilder.DropTable(
                name: "medico");

            migrationBuilder.DropTable(
                name: "traje");

            migrationBuilder.DropTable(
                name: "passageiro");
        }
    }
}
