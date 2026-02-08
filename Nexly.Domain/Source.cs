public class Source
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Type  { get; set; } // RSS, API
    public string Country { get; set; }
    public float TrustScore { get; set; }
    public string Url { get; set; }

    public string Language { get; set; }
}