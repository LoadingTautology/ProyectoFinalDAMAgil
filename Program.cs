using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil
{ 
    public class Program
    {
        public static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			/* ********* Inyeccion Contexto ********* */
			var connectionString = builder.Configuration.GetConnectionString("Default");
			builder.Services.AddDbContext<DbappProyectoFinalContext>(options => { options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)); });

			/* ********* Inyeccion dependencias Servicios********* */
			builder.Services.AddScoped<Services.Usuario.IUsuarioService, Services.Usuario.UsuarioService>();
            builder.Services.AddScoped<Services.Correoelectronico.ICorreoelectronicoService, Services.Correoelectronico.CorreoelectronicoService>();

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
			}

			
			app.UseHttpsRedirection();/*  */
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
                pattern: "{controller=MainPage}/{action=Index}/{id?}");

            app.Run();
		}

	}
}




//Scaffold-DbContext "server=localhost;port=3306;database=DBAppProyectoFinal;uid=root;password=" Pomelo.EntityFrameworkCore.MySql -NoOnConfiguring -OutputDir Scaffold -Force
