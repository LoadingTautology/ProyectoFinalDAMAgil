using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Scaffold;
using System.Globalization;

namespace ProyectoFinalDAMAgil
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            // Localization configuration
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // ********* Inyeccion Contexto *********
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DbappProyectoFinalContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // ********* Inyeccion dependencias Servicios *********
            builder.Services.AddScoped<Services.Usuario.IUsuarioService, Services.Usuario.UsuarioService>();
            builder.Services.AddScoped<Services.Correoelectronico.ICorreoelectronicoService, Services.Correoelectronico.CorreoelectronicoService>();
            builder.Services.AddScoped<Services.Administrador.IAdministradorService, Services.Administrador.AdministradorService>();
            builder.Services.AddScoped<Services.Centroeducativo.ICentroeducativoService, Services.Centroeducativo.CentroeducativoService>();
            builder.Services.AddScoped<Services.Usuarioscentroeducativo.IUsuarioscentroeducativoService, Services.Usuarioscentroeducativo.UsuarioscentroeducativoService>();
            builder.Services.AddScoped<Services.Cicloformativo.ICicloformativoService, Services.Cicloformativo.CicloformativoService>();
            builder.Services.AddScoped<Services.Asignatura.IAsignaturaService, Services.Asignatura.AsignaturaService>();
            builder.Services.AddScoped<Services.Aula.IAulaService, Services.Aula.AulaService>();



            // Authentication configuration
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login/IniciarSesion";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.AccessDeniedPath = "/Login/IniciarSesion";
            });

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Ensure authentication middleware is used
            app.UseAuthorization();

            // Localization middleware configuration
            var supportedCultures = new[] { new CultureInfo("es"), new CultureInfo("en") };
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(localizationOptions);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=MainPage}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

//"ConnectionStrings": {
//  "Default": "server=database.proyectofinaldam2324.site;port=3306;database=proyecto;uid=root;password=pedrolachuspa"
//}

//Scaffold-DbContext "server=localhost;port=3306;database=DBAppProyectoFinal;uid=root;password=" Pomelo.EntityFrameworkCore.MySql -NoOnConfiguring -OutputDir Scaffold -Force