namespace RESTcats.Models
{
    public class Cat
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Weight { get; set; }

        public override string ToString()
        {
            return $"Cat Id: {Id}, Name: {Name}, Weight: {Weight}kg";
        }
    }
}
