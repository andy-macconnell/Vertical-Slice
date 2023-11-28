using Carter;
using MediatR;
using Vertical.Slice.WebApi.Database;
using Vertical.Slice.WebApi.Entities;

namespace Vertical.Slice.WebApi.Features.WeatherForecasts.GetForecasts
{
    public record GetForecastQuery() : IRequest<GetForecastResponse>;

    public class GetForecastQueryHandler : IRequestHandler<GetForecastQuery, GetForecastResponse>
    {
        private readonly IWeatherForecastRepository _repository;

        public GetForecastQueryHandler(IWeatherForecastRepository repository) 
            => _repository = repository;

        public async Task<GetForecastResponse> Handle(GetForecastQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetForecasts();
            return new GetForecastResponse(result);
        }
    }

    public record GetForecastResponse(IEnumerable<WeatherForecast> forecasts);

    public class GetForecastEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/forecasts", async (ISender sender) => { return await sender.Send(new GetForecastQuery()); })
               .WithName("GetWeatherForecast")
               .WithOpenApi();
        }
    }
}
