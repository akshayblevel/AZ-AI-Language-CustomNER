namespace AZ_AI_Language_CustomNER.Models
{
    public class CustomEntityResponse
    {
        public string Id { get; set; }
        public List<CustomEntity> Entities { get; set; }
    }

    public class CustomEntity
    {
        public string Category { get; set; }
        public string Text { get; set; }
        public double ConfidenceScore { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
    }
}
