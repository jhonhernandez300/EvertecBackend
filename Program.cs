using EvertekBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration
    .GetConnectionString("ConnectionString")));

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.UseDeveloperExceptionPage();


app.UseCors(options => options.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//IWebHostEnvironment env = app.Services.GetRequiredService<IWebHostEnvironment>();
//if (env.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}

//// for the wwwroot/uploads folder
//string uploadsDir = Path.Combine(env.WebRootPath, "uploads");
//if (!Directory.Exists(uploadsDir))
//    Directory.CreateDirectory(uploadsDir);

//app.UseStaticFiles(new StaticFileOptions()
//{
//    RequestPath = "/images",
//    FileProvider = new PhysicalFileProvider(uploadsDir)
//});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllers();

app.Run();
