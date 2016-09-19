public class CustomEvent
{
    public string EventName { get; set; }

    public IDictionary<string, string> Properties { get; set; }

    public IDictionary<string, double> Metrics { get; set; }
}