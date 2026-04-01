using Microsoft.EntityFrameworkCore;
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

// Registrar repositorios genéricos y específicos
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();

// Registrar servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<ITareaService, TareaService>();

// AutoMapper - Registrar el perfil de mapeo
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// FluentValidation validators
builder.Services.AddScoped<CrearUsuarioDtoValidator>();
builder.Services.AddScoped<LoginDtoValidator>();
builder.Services.AddScoped<CrearProyectoDtoValidator>();
builder.Services.AddScoped<CrearTareaDtoValidator>();
builder.Services.AddScoped<AsignarTareaDtoValidator>();
builder.Services.AddScoped<ActualizarProyectoDtoValidator>(); // Nuevo
builder.Services.AddScoped<ActualizarTareaDtoValidator>();   // Nuevo

// Configurar controladores con NewtonsoftJson para evitar bucles de referencia
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Agregar OpenAPI (Swagger) para desarrollo
builder.Services.AddOpenApi();

var app = builder.Build();

// Configurar el pipeline de HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();