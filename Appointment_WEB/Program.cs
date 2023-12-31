using Appointment_WEB;
using Microsoft.EntityFrameworkCore;
using Appointment_WEB.Services;
using Appointment_WEB.Services.Interfaces;
using Appointment_WEB.Models;
using Microsoft.AspNetCore.Identity;
using Appointment_WEB.Email;
using Appointment_WEB.Email.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Dependecy injections
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Creating DB
builder.Services.AddDbContext<DbAppointmentContext>(builder =>
{
    builder.UseSqlServer(@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=AppointmentDb;Integrated Security=True");
});

// Password settings
builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 2;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

}).AddEntityFrameworkStores<DbAppointmentContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Needed for authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Appointment}/{action=Show}/{id?}");

app.Run();
