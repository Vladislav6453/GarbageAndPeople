using System.Text.Json;
using ХламИЛюди.Classes;

namespace ХламИЛюди.Models.DB
{
    public class Database
    {
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
        }
        public async Task<List<Owner>> VerniMneSpisokOwner()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "owners.json"), FileMode.Open))
                    return await JsonSerializer.DeserializeAsync<List<Owner>>(fs);
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
                    return await JsonSerializer.DeserializeAsync<List<Thing>>(fs);
            }
            catch
            {
                return new List<Thing>();
            }
        }
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
                owner.Id = owners.MaxBy(x => x.Id).Id + 1;
                owners.Add(owner);
            }
            await SaveOwnersAsync();
        }
        public async Task AddThing(Thing thing)
        {
            if (thing.Id == 0)
            {
                thing.Id = things.MaxBy(x => x.Id).Id + 1;
                things.Add(thing);
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
