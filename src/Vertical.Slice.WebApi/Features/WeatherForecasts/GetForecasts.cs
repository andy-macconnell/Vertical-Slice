using Carter;
using MediatR;

namespace Vertical.Slice.WebApi.Features.WeatherForecasts
{
    public record GetForecastRequest() : IRequest<GetForecastResponse>;

    public class GetForecastRequestHandler : IRequestHandler<GetForecastRequest, GetForecastResponse>
    {
        private readonly string[] summaries =
        [
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        ];

        public Task<GetForecastResponse> Handle(GetForecastRequest request, CancellationToken cancellationToken)
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
            
            return Task.FromResult(new GetForecastResponse(forecast));
        }
    }

    public record GetForecastResponse(IEnumerable<WeatherForecast> forecasts);

    public class GetForecastEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/weatherforecast", async (ISender sender) => { return await sender.Send(new GetForecastRequest()); })
               .WithName("GetWeatherForecast")
               .WithOpenApi();
        }
    }

    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

}
