using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ХламИЛюди.Classes;
using ХламИЛюди.Models.DB;

namespace GarbageAndPeople.VM
{
    internal class MainPageVM
    {
        public List<Owner> Owners { get; set; }
        public List<Thing> Things { get; set; }

        public Owner CurrentOwner
        {
            get => currentOwner;
            set
            {
                currentOwner = value;
                if (value != null && value.Id != 0)
                    ChangeOwnersThingsList(value.Id);
            }
        }

        public List<Thing> OwnersThings { get; set; }

        private Database db = new();
        private Owner currentOwner;

        public MainPageVM() 
        {
            LoadLists();
        }
        public async void LoadLists()
        {
            //
            
            Owners = db.VerniMneSpisokOwner().Result;
            //db.AddThing(new Thing() { Title="мусор", Description="Куча мусора. 2 кг" });
            //db.AddThing(new Thing() { Title = "мусор", Description = "Большая куча мусора. 6 кг" });
            Things = db.GetThingsAsync().Result;
            var thing1 = Things.First();
            var thing2 = Things.Last();
            thing1.OwnerId = Owners.First().Id;
            thing1.Id = 1;
            thing2.OwnerId = Owners.Last().Id;
            thing2.Id = 2;
            db.AddThing(thing1);
            db.AddThing(thing2);
            Things = db.GetThingsAsync().Result;
        }

        public async void ChangeOwnersThingsList(int ownerId) =>
            OwnersThings = await db.GetThingsByOwnerIdAsync(ownerId);
    }
}
