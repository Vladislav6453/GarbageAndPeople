using GarbageAndPeople.Models;
using GarbageAndPeople.Models.DB;
using GarbageAndPeople.View;
using GarbageAndPeople.VM.VMTools;

namespace GarbageAndPeople.VM
{
    public class MainPageVM : BaseVM
    {
        private Database db = new();
        private Owner currentOwner;
        private List<Owner> owners;
        private List<Thing> things;
        private List<Thing> ownersThings;
        private ContentPage page;
        private Thing selectedThing;

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
        public Thing SelectedThing
        {
            get => selectedThing;
            set
            {
                selectedThing = value;
                Signal();
                RemoveThing.ChangeCanExecute();
                RedactThing.ChangeCanExecute();
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


        public Command CreateThing { get; set; }
        public Command CreateOwner { get; set; }
        public Command<Thing> RemoveThing { get; set; }
        public Command RemoveOwner { get; set; }
        public Command<Thing> RedactThing { get; set; }
        public Command RedactOwner { get; set; }

        public MainPageVM() 
        {
            CreateThing = new Command(async () =>
            {
                await page.Navigation.PushAsync(new EditThing(new Thing(), db));
                Things = await db.GetThingsAsync();
            }, () => true);
            CreateOwner = new Command(async () =>
            {
                await page.Navigation.PushAsync(new EditOwner(new Owner(), db));
                Owners = await db.VerniMneSpisokOwner();
            }, () => true);

            RemoveThing = new Command<Thing>(async (thing) =>
            {
                await db.RemoveThing(SelectedThing);
                Things = await db.GetThingsAsync();
            }, (thing) => true);

            RedactThing = new Command<Thing>(async (thing) =>
            {
                await page.Navigation.PushAsync(new EditThing(thing, db));
                OpenEditThing(thing);
                //Things = await db.GetThingsAsync();
            }, (thing) => true);


            LoadLists();
        }

        public async void LoadLists()
        {            
            Owners = await db.VerniMneSpisokOwner();
            Things = await db.GetThingsAsync();
        }
        private async void OpenEditThing(Thing thing)
        {
            await page.Navigation.PushAsync(new EditThing(thing, db));
        }

        public async void ChangeOwnersThingsList(int ownerId) =>
            OwnersThings = await db.GetThingsByOwnerIdAsync(ownerId);
        public void Set(ContentPage page)
        {
            this.page = page;
        }
    }
}
