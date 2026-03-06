using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Project3Vitour.Services.BookingServices;
using Project3Vitour.Services.CategoryServices;
using Project3Vitour.Services.ContactServices;
using Project3Vitour.Services.GuideServices;
using Project3Vitour.Services.ModerationServices;
using Project3Vitour.Services.TourServices;
using Project3Vitour.Settings;
using System.Globalization;
using System.Reflection;
using Project3Vitour.Services.MailServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<ITourReviewService, TourReviewService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, MailService>();

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("tr"),
        new CultureInfo("en"),
        new CultureInfo("de")
    };

    options.DefaultRequestCulture = new RequestCulture("tr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Cookie provider'ý ilk sýraya al (varsayýlan dil: Türkçe)
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new CookieRequestCultureProvider
    {
        CookieName = ".Vitour.Culture"
    });
    options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
});

builder.Services.AddHttpClient<IModerationService, ModerationService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(60);
});

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettingKey"));
builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

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

// UseRequestLocalization, UseRouting'den sonra olmalý
app.UseRequestLocalization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();