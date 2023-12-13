using BeautyPoly.Data.Repositories;
using BeautyPoly.DBContext;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<BeautyPolyDbContext>();
builder.Services.AddScoped<OptionRepo>();
builder.Services.AddScoped<OptionValueRepo>();
builder.Services.AddScoped<CategoryRepo>();
builder.Services.AddScoped<ProductRepo>();
builder.Services.AddScoped<ProductSkuRepo>();
builder.Services.AddScoped<ProductDetailRepo>();
builder.Services.AddScoped<OptionDetailRepo>();
builder.Services.AddScoped<RoleRepo>();
builder.Services.AddScoped<AccountRepo>();
builder.Services.AddScoped<CouponRepo>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<LocationRepo>();
builder.Services.AddScoped<LoginAccountRepository>();
builder.Services.AddScoped<PotentialCustomersRepo>();
builder.Services.AddScoped<SaleItemCateRepo>();
builder.Services.AddScoped<SaleItemProductRepo>();
builder.Services.AddScoped<SaleRepo>();
builder.Services.AddScoped<VoucherRepo>();
builder.Services.AddScoped<CartDetailsRepo>();
builder.Services.AddScoped<ProductImagesRepo>();
builder.Services.AddScoped<OrderRepo>();
builder.Services.AddSession(p => p.IdleTimeout = TimeSpan.FromSeconds(50));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie
(
   p =>
   {
       p.Cookie.Name = "MyCookies";
       p.ExpireTimeSpan = TimeSpan.FromMinutes(15);
       //p.LoginPath = "/dang-nhap.html";
       // p.LogoutPath = "/dang-xuat.html";
       p.AccessDeniedPath = "/not-found.html";
   }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
