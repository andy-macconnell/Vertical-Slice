using Carter;
using MediatR;
using Vertical.Slice.Playground.Entities;

namespace Vertical.Slice.Playground.Features.Forecasts.V1
{
    public class GetForecast
    {
        internal class Command : IRequest<IEnumerable<WeatherForecast>> { }

        internal sealed class Handler : IRequestHandler<Command, IEnumerable<WeatherForecast>>
        {
            private string[] _summaries = new[]
            {
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
            };

            public Task<IEnumerable<WeatherForecast>> Handle(Command request, CancellationToken cancellationToken)
            {
                var forecasts = Enumerable
                    .Range(1, 5)
                    .Select(index => new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        _summaries[Random.Shared.Next(_summaries.Length)]
                    ));

                return Task.FromResult(forecasts);
            }
        }
    }

    public class GetForecastEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var forcasts = app.NewVersionedApi(nameof(GetForecastEndpoint));
            var versionOne = new Asp.Versioning.ApiVersion(1.0);

            var versionSet = app
                .NewApiVersionSet()
                .HasApiVersion(versionOne)
                .Build();

            app.MapGet("v{version:apiVersion}/weatherforecast",
                    async (ISender sender) =>
                    {
                        return await sender.Send(new GetForecast.Command());
                    })
                    .WithName(nameof(GetForecast))
                    .WithApiVersionSet(versionSet)
                    .MapToApiVersion(versionOne)
                    .WithOpenApi();
        }
    }
}

