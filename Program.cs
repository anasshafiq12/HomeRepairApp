using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddDefaultIdentity<MyUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndAdminAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Role and Admin User Seed Method
async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<MyUser>>();
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

    // Create Admin role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (roleResult.Succeeded)
        {
            logger.LogInformation("Admin role created.");
        }
        else
        {
            logger.LogError("Error creating Admin role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }
    }

    // Create a default Admin user
    var adminEmail = "anas@admin.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdmin = new MyUser
        {
            UserName = "anas@admin.com",
            Email = adminEmail,
            EmailConfirmed = true,
            Name = "Admin User",
            Phone = "123-456-7890",
            Address = "Default Address"
        };

        var createResult = await userManager.CreateAsync(newAdmin, "Admin@123");
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
            logger.LogInformation("Admin user created successfully.");
        }
        else
        {
            logger.LogError("Error creating Admin user: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));

        }
    }
    else
    {
        logger.LogInformation($"Admin user already exists. {adminUser.Email} + {adminUser.PasswordHash}");

    }
}
