public class TemplateNotification
{
    public string CorrelationId { get; set; }
    public IDictionary<string, string> Properties { get; set; }         
    public string TagExpression { get; set; }
}