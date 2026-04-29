using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Filters;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;
using TaskFlow.Infrastructure.Mappings;
using TaskFlow.Infrastructure.Repositories;
using TaskFlow.Services.Interfaces;
using TaskFlow.Services.Services;
using TaskFlow.Services.Validators;

var builder = WebApplication.CreateBuilder(args);

// Configurar base de datos MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaskFlowContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Registrar UnitOfWork, Dapper y Factory
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IDapperContext, DapperContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositorios específicos
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();

// Servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddScoped<IComentarioService, ComentarioService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// FluentValidation validators
builder.Services.AddScoped<CrearUsuarioDtoValidator>();
builder.Services.AddScoped<LoginDtoValidator>();
builder.Services.AddScoped<CrearProyectoDtoValidator>();
builder.Services.AddScoped<CrearTareaDtoValidator>();
builder.Services.AddScoped<AsignarTareaDtoValidator>();
builder.Services.AddScoped<ActualizarProyectoDtoValidator>();
builder.Services.AddScoped<ActualizarTareaDtoValidator>();
builder.Services.AddScoped<ComentarioDtoValidator>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Middleware global de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();