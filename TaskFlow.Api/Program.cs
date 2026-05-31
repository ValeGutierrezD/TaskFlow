using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using TaskFlow.Api.Filters;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;
using TaskFlow.Infrastructure.Mappings;
using TaskFlow.Infrastructure.Repositories;
using TaskFlow.Services.Interfaces;
using TaskFlow.Services.Services;
using TaskFlow.Services.Validators;
using TaskFlow.Core.CustomEntities;

var builder = WebApplication.CreateBuilder(args);

// Configurar base de datos MySQL (toma la cadena de appsettings.json)
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
builder.Services.AddSingleton<IPasswordService, PasswordService>();

// Configurar PasswordOptions desde appsettings
builder.Services.Configure<PasswordOptions>(builder.Configuration.GetSection("PasswordOptions"));

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

// Configurar JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!))
    };
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "TaskFlow API",
        Version = "v1",
        Description = "API para gestión colaborativa de proyectos",
        Contact = new() { Name = "Equipo UCB", Email = "desarrollo@ucb.edu.bo" }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.EnableAnnotations();
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Migraciones automáticas (crea las tablas en Azure)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskFlowContext>();
    db.Database.EnsureCreated();  // ← NUEVA LÍNEA (crea las tablas según el modelo actual)
}

// Middleware global de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskFlow API v1"));
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();