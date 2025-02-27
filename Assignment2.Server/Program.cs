using Assignment2.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services for MVC views & controllers
builder.Services.AddControllersWithViews(); // Ensures MVC views work
builder.Services.AddRazorPages(); // Enables Razor Pages if needed

// Add API support (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Serve default static files (for React frontend if used)
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Remove Swagger redirection, only enable it manually if needed
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}


// Map controllers and MVC views
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"); // Ensure it loads your MVC views

    endpoints.MapRazorPages(); // Supports Razor Pages if needed
});

// Remove forced API fallback for React, allow MVC views to load
// app.MapFallbackToFile("/index.html"); // COMMENT THIS OUT IF TESTING VIEWS

app.Run();
