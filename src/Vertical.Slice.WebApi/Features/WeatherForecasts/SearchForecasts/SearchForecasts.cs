using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vertical.Slice.WebApi.Database;
using Vertical.Slice.WebApi.Entities;

namespace Vertical.Slice.WebApi.Features.WeatherForecasts.GetLocationForecast
{
    public class SearchForecastsRequest
    {
        public int TemperatureC { get; init; }
    }

    public record SearchForecastsQuery(int TemperatureC) : IRequest<SearchForecastsResponse>;

    public record SearchForecastsResponse(IEnumerable<WeatherForecast> forecasts);

    public class SubmitForecastRequestHandler : IRequestHandler<SearchForecastsQuery, SearchForecastsResponse>
    {
        private readonly IWeatherForecastRepository _repository;

        public SubmitForecastRequestHandler(IWeatherForecastRepository repository)
        {
            _repository = repository;
        }

        public async Task<SearchForecastsResponse> Handle(SearchForecastsQuery request, CancellationToken cancellationToken)
        {
            var forecasts = await _repository.GetForecasts();
            return new SearchForecastsResponse(forecasts.Where(forecast => forecast.TemperatureC >= request.TemperatureC));
        }
    }

    public class SubmitForecastEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/forecasts/search", async (ISender sender, [FromBody] SearchForecastsRequest request) => { return await SearchForecasts(sender, request); })
                .WithName("SearchForecasts")
                .WithOpenApi();
        }

        private static async Task<SearchForecastsResponse> SearchForecasts(ISender sender, SearchForecastsRequest request)
        {
            var query = new SearchForecastsQuery(request.TemperatureC);
            var results = await sender.Send(query);
            return results;
        }
    }
}
