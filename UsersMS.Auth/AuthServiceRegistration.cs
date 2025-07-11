using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using UsersMS.Infrastructure.Database;
using UsersMS.Infrastructure.Adapters.Keycloak;
using UsersMS.Infrastructure.Adapters.Keycloak.UrlHelper;
using UsersMS.Infrastructure.Adapters.Keycloak.RequestBuilder;
using Microsoft.AspNetCore.Http;
using UsersMS.Application.DTOs.Auth;
using UsersMS.Application.Handlers.Auth;
using UsersMS.Auth.Application.Handlers;
using UsersMS.Auth.Infrastructure.DTOs.Login;
using UsersMS.Auth.Infrastructure.DTOs.Logout;
using UsersMS.Auth.Infrastructure.DTOs.RefreshToken;
using UsersMS.Auth.Infrastructure.DTOs.ChangePassword;
using UsersMS.Auth.Infrastructure.DTOs.RecoverPassword;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.Adapters.Keycloak.Email;
using UsersMS.Infrastructure.Adapters.KeycloakRepository;
using UsersMS.Infrastructure.Adapters;
using UsersMS.Infrastructure.Decorators;
using UsersMS.Infrastructure.DTOs.Email;
using UsersMS.Infrastructure.Validators.AssignRole;
using UsersMS.Application.Validators.CreateUser;
using UsersMS.Application.Validators.Login;
using UsersMS.Application.Validators.Logout;
using UsersMS.Application.Validators.RefreshToken;
using Keycloak.AuthServices.Authentication;
using UsersMS.Auth.Infrastructure.Validators.ChangePassword;
using UsersMS.Auth.Infrastructure.Validators.RecoverPassword;

namespace UsersMS
{
    public static class AuthServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Keycloak Configuration
            var configurationBuilder = new ConfigurationBuilder().AddConfiguration(configuration);
            var keycloakConfigFile = configuration["Keycloak:ConfigFile"];
            if (!string.IsNullOrEmpty(keycloakConfigFile))
            {
                configurationBuilder.AddJsonFile(keycloakConfigFile, optional: false, reloadOnChange: true);
            }

            var builtConfiguration = configurationBuilder.Build();

            services.AddKeycloakWebApiAuthentication(builtConfiguration, options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = new UrlHelperKeycloak().GetRealmUrl(builtConfiguration);
                options.Audience = builtConfiguration["Keycloak:ClientId"];
            });

            // Token & Headers
            services.AddScoped<HeadersToken>();
            services.AddScoped<IHeadersClientCredentialsToken, HeadersClientCredentialsToken>();

            // Email
            services.AddScoped<EmailProcessor>();
            services.AddScoped<EmailTemplateService>();
            services.AddScoped<SmtpClientFactory>();
            services.AddScoped<IService<EmailRequestDTO, EmailResponseDTO>, EmailService>();

            // Keycloak
            services.AddScoped<IUrlHelperKeycloak, UrlHelperKeycloak>();
            services.AddScoped<IKeycloakRepository, KeycloakRepository>();
            services.AddScoped<IKeycloakRequestBuilder, KeycloakRequestBuilder>();

            // MediatR Handlers
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginCommandHandler>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LogoutCommandHandler>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssignRoleCommandHandler>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RefreshTokenCommandHandler>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ChangePasswordCommandHandler>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RecoverPasswordCommandHandler>());

            // Validators
            services.AddScoped<IService<LoginRequestDTO, LoginResponseDTO>, AuthLoginValidate>();
            services.AddScoped<IService<LogoutRequestDTO, LogoutResponseDTO>, AuthLogoutValidate>();
            services.AddScoped<IService<AssignRoleRequestDTO, AssignRoleResponseDTO>, AssignRoleValidator>();
            services.AddScoped<IService<CreateUserRequestDTO, CreateUserResponseDTO>, AuthCreateUserValidator>();
            services.AddScoped<IService<RefreshTokenRequestDTO, RefreshTokenResponseDTO>, AuthRefreshTokenValidate>();
            services.AddScoped<IService<ChangePasswordRequestDTO, ChangePasswordResponseDTO>, AuthChangePasswordValidator>();
            services.AddScoped<IService<RecoverPasswordRequestDTO, RecoverPasswordResponseDTO>, AuthRecoverPasswordValidator>();

            // Authorization
            services.AddAuthorization();

            // Decorator: Role Validation for User Creation
            services.Decorate<IService<CreateUserRequestDTO, CreateUserResponseDTO>>(
                (inner, provider) => new RoleValidationDecorator<CreateUserRequestDTO, CreateUserResponseDTO>(
                    inner,
                    provider.GetRequiredService<IKeycloakRepository>(),
                    provider.GetRequiredService<IHttpClientFactory>(),
                    provider.GetRequiredService<IHttpContextAccessor>()));
        }
    }
}
