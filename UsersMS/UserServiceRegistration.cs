using Microsoft.EntityFrameworkCore;
using MediatR;
using UsersMS.Application.Handlers.User.RecordUserData;
using UsersMS.Application.Services.Record;
using UsersMS.Core.Application;
using UsersMS.Domain.Factories;
using UsersMS.Infrastructure.Database;
using UsersMS.Infrastructure.DTOs.Record;
using UsersMS.Infrastructure.DTOs.RecordUserData;
using UsersMS.Infrastructure.DTOs.Update;
using UsersMS.Infrastructure.DTOs.UpdateUser;
using UsersMS.Infrastructure.Mappers;
using UsersMS.Application.Services.UpdateUser;
using UsersMS.Domain.Repositories;
using UsersMS.Infrastructure.Repositories;
using UsersMS.Application.Services.Update;
using UsersMS.Application.Handlers.Administrator.GetAdministratorById;
using UsersMS.Application.Handlers.Administrator.GetAdministratorByName;
using UsersMS.Application.Queries.GetAdministratorById;
using UsersMS.Application.Queries.GetAdministratorByName;
using UsersMS.Infrastructure.DTOs;
using UsersMS.Application.Handlers.Auctioneer.GetAuctioneerById;
using UsersMS.Application.Handlers.Auctioneer.GetAuctioneerByName;
using UsersMS.Application.Queries.GetAuctioneerById;
using UsersMS.Application.Queries.GetAuctioneerByName;
using UsersMS.Application.Handlers.Bidder.GetBidderById;
using UsersMS.Application.Handlers.Bidder.GetBidderByName;
using UsersMS.Application.Queries.GetBidderById;
using UsersMS.Application.Queries.GetBidderByName;
using UsersMS.Application.Handlers.TechnicalSupport.GetTechnicalSupportByName;
using UsersMS.Application.Queries.GetTechnicalSupportById;
using UsersMS.Application.Queries.GetTechnicalSupportByName;
using UsersMS.Application.Handlers.TechnicalSupport.GetTechnicalSupportById;
using UsersMS.Application.Decorators;
using UsersMS.Infrastructure.Adapters.Keycloak;

namespace UsersMS
{
    public static class UserServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(typeof(AdministratorProfile).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(RecordUserDataCommandHandler).Assembly);
            });

            services.AddScoped<IService<RecordUserDataRequestDTO, RecordUserDataResponseDTO>, RecordUserDataService>();
            services.AddScoped<IService<UpdateRecordUserDataRequestDTO, UpdateRecordUserDataResponseDTO>, UpdateRecordUserDataService>();

            services.AddScoped<IAdministratorFactory, AdministratorFactory>();
            services.AddScoped<IAuctioneerFactory, AuctioneerFactory>();
            services.AddScoped<IBidderFactory, BidderFactory>();
            services.AddScoped<ITechnicalSupportFactory, TechnicalSupportFactory>();

            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            services.AddScoped<IAuctioneerRepository, AuctioneerRepository>();
            services.AddScoped<IBidderRepository, BidderRepository>();
            services.AddScoped<ITechnicalSupportRepository, TechnicalSupportRepository>();

            services.AddScoped<IRecordAdministratorData, RecordAdministratorData>();
            services.AddScoped<IRecordBidderData, RecordBidderData>();
            services.AddScoped<IRecordAuctioneerData, RecordAuctioneerData>();
            services.AddScoped<IRecordTechnicalSupportData, RecordTechnicalSupportData>();

            services.AddScoped<IUpdateRecordAdministratorData, UpdateRecordAdministratorData>();
            services.AddScoped<IUpdateRecordBidderData, UpdateRecordBidderData>();
            services.AddScoped<IUpdateRecordAuctioneerData, UpdateRecordAuctioneerData>();
            services.AddScoped<IUpdateRecordTechnicalSupportData, UpdateRecordTechnicalSupportData>();
            
            services.AddScoped<IRequestHandler<GetAdministratorByIdQuery, GetAdministratorByIdResponseDTO>, GetAdministratorByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAdministratorByNameQuery, GetAdministratorByNameResponseDTO>, GetAdministratorByNameQueryHandler>();
            services.AddScoped<IRequestHandler<GetAuctioneerByIdQuery, GetAuctioneerByIdResponseDTO>, GetAuctioneerByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetAuctioneerByNameQuery, GetAuctioneerByNameResponseDTO>, GetAuctioneerByNameQueryHandler>();
            services.AddScoped<IRequestHandler<GetBidderByIdQuery, GetBidderByIdResponseDTO>, GetBidderByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetBidderByNameQuery, GetBidderByNameResponseDTO>, GetBidderByNameQueryHandler>();
            services.AddScoped<IRequestHandler<GetTechnicalSupportByIdQuery, GetTechnicalSupportByIdResponseDTO>, GetTechnicalSupportByIdQueryHandler>();
            services.AddScoped<IRequestHandler<GetTechnicalSupportByNameQuery, GetTechnicalSupportByNameResponseDTO>, GetTechnicalSupportByNameQueryHandler>();

            services.AddHttpClient();

            services.Decorate<IService<RecordUserDataRequestDTO, RecordUserDataResponseDTO>>(
                (inner, provider) => new RecordUserDataSecurityDecorator<RecordUserDataRequestDTO, RecordUserDataResponseDTO>(
                    inner,
                    provider.GetRequiredService<IKeycloakRepository>(),
                    provider.GetRequiredService<IHttpClientFactory>(),
                    provider.GetRequiredService<IHttpContextAccessor>()));

            services.Decorate<IService<UpdateRecordUserDataRequestDTO, UpdateRecordUserDataResponseDTO>>(
                (inner, provider) => new RecordUserDataSecurityDecorator<UpdateRecordUserDataRequestDTO, UpdateRecordUserDataResponseDTO>(
                    inner,
                    provider.GetRequiredService<IKeycloakRepository>(),
                    provider.GetRequiredService<IHttpClientFactory>(),
                    provider.GetRequiredService<IHttpContextAccessor>()));
        }
    }
}
