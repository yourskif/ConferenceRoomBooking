using ConferenceRoomBooking.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceRoomBooking.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<PricingService>();

            return services;
        }
    }
}