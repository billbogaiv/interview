using System;

namespace Interview
{
    public class PersonRepresentation
    {
        public string Address { get; set; }
        public int Age { get; set; }
        public string Balance { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string EyeColor { get; set; }
        public string FavoriteFruit { get; set; }
        public string Greeting { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public NameRepresentation Name { get; set; } = new NameRepresentation();
        public string Phone { get; set; }
        public DateTimeOffset? Registered { get; set; }
    }
}
