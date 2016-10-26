public class TemplateNotification
{
    public string CorrelationId { get; set; }
    public Dictionary<string, string> Properties { get; set; }         
    public string TagExpression { get; set; }
}