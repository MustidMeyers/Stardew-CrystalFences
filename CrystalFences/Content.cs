namespace CrystalFences
{
    public class Content
    {
        public string Format { get; set; } = "2.0.0";
        public ICollection<EditImage> Changes { get; set; } = new List<EditImage>();

        public Content()
        {
        }
    }

    public class EditImage
    {
        public string Action { get; set; } = "";
        public string Target { get; set; } = "";
        public string FromFile { get; set; } = "";
    }
}
