namespace revisifyBackened.Models.Dto
{
    public class EmailRequest
    {
        public List<string> ToList { get; set; } = new();
        //public List<string> CcList { get; set; } = new();
        public string From { get; set; } = null!;
        public string Subject { get; set; } = null!;
        //public string TemplateName { get; set; } = null!;
        public string HtmlBody { get; set; } = null!;
    }
}
