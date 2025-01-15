namespace Picart.Models
{
    public class Product(string name)
    {
        public string Name { get; } = name;
        public bool Checked { get; set; }
    }
}
