namespace Vertical.Slice.Playground.Contracts
{
    public class GetWeatherForecastResponse
    {
        public DateOnly From { get; set; }
        public int Temperature { get; set; }
        public IEnumerable<string> Summaries { get; set; } = Enumerable.Empty<string>();
    }
}
