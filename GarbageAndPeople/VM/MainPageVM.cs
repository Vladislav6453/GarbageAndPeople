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

        private Database db = new();

        public MainPageVM() 
        {
            LoadLists();
        }
        public async void LoadLists()
        {
            Owners = await db.VerniMneSpisokOwner();
            Things = await db.GetThingsAsync();

            Owners.Add(new Owner() { FirstName="asdasd", LastName="sdfsd", Email="asdasd@gmail.com", PhoneNumber="+12345678901"});
        }
    }
}
