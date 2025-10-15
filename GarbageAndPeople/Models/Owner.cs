using System.Text.Json.Serialization;

namespace GarbageAndPeople.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";

        public string FL => $"{FirstName} {LastName}";

        [JsonIgnore]
        public List<Thing> Things { get; set; } = new List<Thing>();

    }
}
