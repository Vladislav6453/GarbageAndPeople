using System.Text.Json.Serialization;

namespace GarbageAndPeople.Models
{
    public class Thing
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int? OwnerId { get; set; }

        [JsonIgnore]
        public Owner? Owner { get; set; }
    }
}
