using GarbageAndPeople.Models;
using GarbageAndPeople.Models.DB;
using GarbageAndPeople.VM.VMTools;

namespace GarbageAndPeople.VM
{
    internal class MainPageVM : BaseVM
    {
        public List<Owner> Owners
        {
            get => owners;
            set
            {
                owners = value;
                Signal();
            }
        }
        public List<Thing> Things
        {
            get => things;
            set
            {
                things = value;
                Signal();
            }
        }

        public Owner CurrentOwner
        {
            get => currentOwner;
            set
            {
                currentOwner = value;
                if (value != null && value.Id != 0)
                {
                    ChangeOwnersThingsList(value.Id);
                    Signal();
                }
            }
        }

        public List<Thing> OwnersThings
        {
            get => ownersThings;
            set
            {
                ownersThings = value;
                Signal();
            }
        }

        private Database db = new();
        private Owner currentOwner;
        private List<Owner> owners;
        private List<Thing> things;
        private List<Thing> ownersThings;
             
        public MainPageVM() 
        {
            OpenRedactorThing = new CommandVM(async () =>
            {
                await page.Navigation.PushAsync();
            }, () => true);



            LoadLists();
        }

        public CommandVM OpenRedactorThing { get; set; }

        public async void LoadLists()
        {            
            Owners = await db.VerniMneSpisokOwner();
            Things = await db.GetThingsAsync();
        }

        


        public async void ChangeOwnersThingsList(int ownerId) =>
            OwnersThings = await db.GetThingsByOwnerIdAsync(ownerId);

        public ContentPage page;
        public void Set(ContentPage page)
        {
            this.page = page;
        }
    }
}
