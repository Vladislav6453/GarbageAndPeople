using System.Text.Json;

namespace GarbageAndPeople.Models.DB
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
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "owners.json"), FileMode.Open))
                {
                    owners = JsonSerializer.Deserialize<List<Owner>>(fs);
                    incrementOwner = owners.MaxBy(x => x.Id).Id;
                }
            }
            catch
            {
                owners = new();
            }

            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "things.json"), FileMode.Open))
                {
                    things = JsonSerializer.Deserialize<List<Thing>>(fs);
                    incrementThing = things.MaxBy(x => x.Id).Id;
                }

            }
            catch
            {
                things = new List<Thing>();
            }

            for (int i = 0; i < things.Count; i++)
            {
                var owner = owners.FirstOrDefault(o => o.Id == things[i].OwnerId);
                things[i].Owner = owner;
                owner?.Things.Add(things[i]);
            }


            //AddOwner(new Owner() { FirstName = "asdasd", LastName = "sdfsd", Email = "asdasd@gmail.com", PhoneNumber = "+12345678901" });
            //AddOwner(new Owner() { FirstName = "vbnmvbn", LastName = "xcbv", Email = "asdasd@gmail.com", PhoneNumber = "+12345678901" });

        }
        public async Task<List<Owner>> VerniMneSpisokOwner()
        {
            await Task.Delay(300);
            return owners;
        }
        public async Task<bool> SaveOwnersAsync()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "owners.json"), FileMode.OpenOrCreate))
                    await JsonSerializer.SerializeAsync(fs, owners);
            }
            catch {
                return false;
            }
            return true;
        }
        public async Task<List<Thing>> GetThingsAsync()
        {
            await Task.Delay(300);
            return things;
        }
        public async Task<List<Thing>> GetThingsByOwnerIdAsync(int ownerId) 
            => (await GetThingsAsync()).Where(t => t.OwnerId == ownerId).ToList();
        public async Task<bool> SaveThingsAsync()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "things.json"), FileMode.OpenOrCreate))
                    await JsonSerializer.SerializeAsync(fs, things);
            }
            catch(Exception e)
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
        public async Task ChangeOwner(Owner owner)
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
