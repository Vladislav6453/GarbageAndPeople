using System.Text.Json;
using ХламИЛюди.Classes;

namespace ХламИЛюди.Models.DB
{
    public class Database
    {
        private int incrementOwner;
        private int incrementThing;
        public Database() 
        {
            StartDb();
        }
        private List<Owner> owners = new();
        private List<Thing> things = new();

        public async void StartDb()
        {
            owners = await VerniMneSpisokOwner();
                   
            things = await GetThingsAsync();
            

            AddOwner(new Owner() { FirstName = "asdasd", LastName = "sdfsd", Email = "asdasd@gmail.com", PhoneNumber = "+12345678901" });
            AddOwner(new Owner() { FirstName = "vbnmvbn", LastName = "xcbv", Email = "asdasd@gmail.com", PhoneNumber = "+12345678901" });

        }
        public async Task<List<Owner>> VerniMneSpisokOwner()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "owners.json"), FileMode.Open))
                {
                    List<Owner> list = JsonSerializer.Deserialize<List<Owner>>(fs);
                    incrementOwner = owners.MaxBy(x => x.Id).Id;
                    return list;
                }
                    
            }
            catch { 
                return new List<Owner>();
            }        
        }
        public async Task<bool> SaveOwnersAsync()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "owners.json"), FileMode.OpenOrCreate))
                    await JsonSerializer.SerializeAsync<List<Owner>>(fs, owners);
            }
            catch {
                return false;
            }
            return true;
        }
        public async Task<List<Thing>> GetThingsAsync()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "things.json"), FileMode.Open))
                {
                    List<Thing> things = JsonSerializer.Deserialize<List<Thing>>(fs);
                    incrementThing = things.MaxBy(x => x.Id).Id;
                    return things;
                }
                    
            }
            catch
            {
                return new List<Thing>();
            }
        }
        public async Task<List<Thing>> GetThingsByOwnerIdAsync(int ownerId) 
            => (await GetThingsAsync()).Where(obj => obj.OwnerId == ownerId).ToList();
        public async Task<bool> SaveThingsAsync()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "things.json"), FileMode.OpenOrCreate))
                    await JsonSerializer.SerializeAsync<List<Thing>>(fs, things);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public async Task<Owner> GetOwnerAsync(int id)
        {
            return (await VerniMneSpisokOwner()).FirstOrDefault(owner => owner.Id == id);
        }
        public async Task<Thing> GetThingAsync(int id)
        {
            return (await GetThingsAsync()).FirstOrDefault(thing => thing.Id == id);
        }
        public async Task AddOwner(Owner owner)
        {
            if (owner.Id == 0)
            {
                owner.Id = ++incrementOwner;
                owners.Add(owner);
            }
            else
            {
                var ownerToChange = owners.First(x => x.Id == owner.Id);
                ownerToChange.FirstName = owner.FirstName;
                ownerToChange.LastName = owner.LastName;
                ownerToChange.PhoneNumber = owner.PhoneNumber;
                ownerToChange.Email = owner.Email;
            }
            await SaveOwnersAsync();
        }
        public async Task AddThing(Thing thing)
        {
            if (thing.Id == 0)
            {
                thing.Id = ++incrementThing;
                things.Add(thing);
            }
            else
            {
                var thingToChange = things.First(x => x.Id == thing.Id);
                thingToChange.Title = thing.Title;
                thingToChange.Description = thing.Description;
                thingToChange.OwnerId = thing.OwnerId;
            }
            await SaveThingsAsync();
        }
        public async Task RemoveThing(Thing thing)
        {
            things.Remove(thing);
            await SaveThingsAsync();
        }
        public async Task RemoveOwner(Owner owner)
        {
            owners.Remove(owner);
            await SaveOwnersAsync();
        }
    }
}
