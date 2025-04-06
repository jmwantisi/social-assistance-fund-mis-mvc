using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Seeders;
using socialAssistanceFundMIS.Services;
using SocialAssistanceFundMisMcv.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Services

builder.Services.AddScoped<ApplicantService>();
builder.Services.AddScoped<ApplicationService>();
builder.Services.AddScoped<AssistanceProgramService>();
builder.Services.AddScoped<DesignationService>();
builder.Services.AddScoped<GeographicLocationService>();
builder.Services.AddScoped<MaritalStatusService>();
builder.Services.AddScoped<OfficialRecordService>();
builder.Services.AddScoped<PhoneNumberTypeService>();
builder.Services.AddScoped<SexService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<LookupService>();
builder.Services.AddScoped<OfficerService>();

var app = builder.Build();

// Run database migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Apply pending migrations
    DefaultSeeder.SeedGeographicLocations(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
