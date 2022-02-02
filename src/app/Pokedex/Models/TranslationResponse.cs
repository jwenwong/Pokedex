namespace Pokedex.Models
{
    public class TranslationResponse
    {
        public Success Success { get; set; }
        public Contents Contents { get; set; }
    }

    public class Contents
    {
        public string Translated { get; set; }
    }

    public class Success
    {
        public int Total { get; set; }
    }
}
