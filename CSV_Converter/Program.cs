using CSV_Converter.Data;
using CSV_Converter.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();


// Автоматическое применение миграции
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	dbContext.Database.Migrate();
}



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


//app.UseHttpsRedirection();


app.UseStaticFiles();


app.UseRouting();


app.MapDefaultControllerRoute();


app.Run();
