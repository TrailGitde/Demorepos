using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);
string routes = string.Empty;

if (builder.Environment.IsDevelopment())
{
    routes = "DevRoutes";
}
else
{
    routes = "Routes";
}

builder.Configuration.AddOcelotWithSwaggerSupport(opt =>
{
    opt.Folder = routes;
});


// Add services to the container.
//builder.Services.AddRazorPages(); // Default

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
    options.AddPolicy(name: "AllowOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        }));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
}
app.UseCors("AllowOrigin");

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";

}).UseOcelot().Wait();



app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthorization();
//app.MapRazorPages();

app.Run();
