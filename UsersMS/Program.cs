using UsersMS;
using UsersMS.Core;
using UsersMS.Core.Utilities;
using UsersMS.Swagger;

var builder = WebApplication.CreateBuilder(args);

CoreServiceRegistration.ConfigureLogging(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

UserServiceRegistration.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.AddSwaggerConfiguration(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        corsBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

//CoreServiceRegistration.UseLogging(app);

app.UseSwaggerUIConfiguration(app.Environment, builder.Configuration);

app.UseMiddleware<BearerTokenMiddleware>();

app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapReverseProxy();

app.RunLogger();
