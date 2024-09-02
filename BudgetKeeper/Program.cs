using BudgetKeeper.Domain.Interfaces;
using BudgetKeeper.EFCore;
using BudgetKeeper.EFCore.Repositories;
using BudgetKeeper.EFCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var dbServer = Environment.GetEnvironmentVariable("DATABASE_SERVER");
var dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "budgetkeeper";
var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER") ?? "budgetkeeper";
var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

var connectionString = $"Server={dbServer};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<BudgetKeeperContext>(options =>
                options.UseNpgsql(connectionString,
                b => b.MigrationsAssembly("BudgetKeeper.EFCore")), ServiceLifetime.Transient);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IBudgetItemRepository, BudgetItemRepository>();
builder.Services.AddTransient<ISubDebtRepository, SubDebtRepository>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");  //be sure add this line


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Perform Migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<BudgetKeeperContext>();
    context.Database.Migrate();
}

app.Run();
