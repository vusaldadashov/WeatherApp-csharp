namespace WeatherApp.Models
{
    public class TempViewModel
    {
        public string name { get; set; } = string.Empty;
        public string temp_c { get; set; } = string.Empty;
        public string temp_f { get; set; } = string.Empty;

        public string text { get; set; } = string.Empty;

        public string imageUrl { get; set; } = string.Empty;
    }
}
