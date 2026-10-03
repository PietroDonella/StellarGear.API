using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using StellarGear.Application.Interfaces;
using StellarGear.Domain.Entities;
using StellarGear.Infrastructure.Persistence;
using StellarGear.Infrastructure.Persistence.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var sqlConnection = builder.Configuration.GetConnectionString("SqlServerConnection")
    ?? throw new InvalidOperationException("Defina a connection string SqlServerConnection.");

builder.Services.AddDbContext<StellarGearContext>(options =>
    options.UseSqlServer(sqlConnection));

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddScoped<IPassageiroRepository, PassageiroRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<ITrajeRepository, TrajeRepository>();
builder.Services.AddScoped<ILeituraSensorRepository, LeituraSensorRepository>();
builder.Services.AddScoped<IAlertaEmergenciaRepository, AlertaEmergenciaRepository>(); 
builder.Services.AddScoped<IHistoricoMedicoRepository, HistoricoMedicoRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "🚀 StellarGear API 🛰️", 
        Version = "v1",
        Description = "Interface de controle para a Global Solution 2026/1."
    });
});

var app = builder.Build();

if (app.Configuration.GetValue("Database:AutoMigrate", false))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<StellarGearContext>();
    db.Database.Migrate();

    if (!await db.Trajes.AnyAsync())
    {
        var passageiro = await db.Passageiros.FirstOrDefaultAsync();
        if (passageiro == null)
        {
            passageiro = new Passageiro("Maria Souza", "52998224725", 34, "Apto para Voo");
            db.Passageiros.Add(passageiro);
            await db.SaveChangesAsync();
        }

        var medico = await db.Medicos.FirstOrDefaultAsync();
        if (medico == null)
        {
            medico = new Medico("Dra. Ana Lima", "CRM-12345", "Medicina Aeroespacial");
            db.Medicos.Add(medico);
            await db.SaveChangesAsync();
        }

        var traje = new Traje(passageiro.Id, "RFID-SG-001");
        db.Trajes.Add(traje);
        if (!await db.HistoricosMedicos.AnyAsync(h => h.IdPassageiro == passageiro.Id))
            db.HistoricosMedicos.Add(new HistoricoMedico(passageiro.Id, "Apto apos avaliacao pre-voo"));
        await db.SaveChangesAsync();

        var leitura = new LeituraSensor(traje.Id, 36.8m, 45.0m, 78m);
        db.LeiturasSensores.Add(leitura);
        await db.SaveChangesAsync();

        db.AlertasEmergencia.Add(new AlertaEmergencia(
            leitura.Id, medico.Id, "Leitura inicial dentro do esperado", "BAIXO"));
        await db.SaveChangesAsync();
    }
}

app.UseForwardedHeaders();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();