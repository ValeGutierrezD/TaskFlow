using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;


namespace TaskFlow.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TaskFlowContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                // Esto es opcional pero recomendado para que EF sepa dónde están tus configuraciones
                b => b.MigrationsAssembly("TaskFlow.Infrastructure")
            ));

            // Agrega el soporte para controladores (Para tus Casos de Uso)
            builder.Services.AddControllers();

            // Configura OpenAPI/Swagger (Requisito de la sección 6.2 de tu plantilla)
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configuración del pipeline (Orden de ejecución)
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi(); // Esto habilita la documentación interactiva
            }

            app.UseHttpsRedirection(); // Seguridad básica obligatoria
            app.UseAuthorization();
            app.MapControllers(); // Mapea tus rutas como /api/auth/login

            app.Run();
        }
    }
}
