namespace Core.Logger
{
    public class Configuration
    {
        public string[] Debug { get; set; } = new string[1] { "Console" };
        public string[] Info { get; set; } = new string[1] { "Console" };
        public string[] Error { get; set; } = new string[1] { "Console" };
        public string[] Warning { get; set; } = new string[1] { "Console" };
    }
}
