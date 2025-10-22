using GarbageAndPeople.Models;
using GarbageAndPeople.Models.DB;
using GarbageAndPeople.View;
using GarbageAndPeople.VM.VMTools;
using System.Collections.ObjectModel;

namespace GarbageAndPeople.VM
{
    public class MainPageVM : BaseVM
    {
        private Database db = new();
        private Owner? currentOwner;
        private IReadOnlyCollection<Owner> owners;
        private IReadOnlyCollection<Thing> things;
        private IReadOnlyCollection<Thing> ownersThings;
        private ContentPage page;
        private Thing? selectedThing;

        public IReadOnlyCollection<Owner> Owners
        {
            get => owners;
            set
            {
                owners = value;
                Signal();
            }
        }
        public IReadOnlyCollection<Thing> Things
        {
            get => things;
            set
            {
                things = value;
                Signal();
            }
        }

        public Owner? CurrentOwner
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
                RedactOwner.ChangeCanExecute();
                RemoveOwner.ChangeCanExecute();
            }
        }
        public Thing? SelectedThing
        {
            get => selectedThing;
            set
            {
                selectedThing = value;
                Signal();
                //RemoveThing.ChangeCanExecute();
                //RedactThing.ChangeCanExecute();
            }
        }

        public IReadOnlyCollection<Thing> OwnersThings
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
            }, () => true);
            CreateOwner = new Command(async () =>
            {
                await page.Navigation.PushAsync(new EditOwner(new Owner(), db));
            }, () => true);

            RemoveThing = new Command<Thing>(async (thing) =>
            {
                await db.RemoveThing(thing);
                Things = await db.GetThingsAsync();
            }, (thing) => thing != null);
            RemoveOwner = new Command<Owner>(async (owner) => 
            {
                await db.RemoveOwner(owner, page);
                Owners = await db.VerniMneSpisokOwner();
                CurrentOwner = Owners.FirstOrDefault();
            }, (owner) => owner != null);

            RedactThing = new Command<Thing>(async (thing) =>
            {
                OpenEditThing(thing);
            }, (thing) => thing != null);
            RedactOwner = new Command<Owner>(async (owner) => 
            {
                await page.Navigation.PushAsync(new EditOwner(owner, db));
            }, (owner) => owner != null);


            LoadLists();
        }

        public async void LoadLists()
        {            
            Owners = await db.VerniMneSpisokOwner();
            Things = await db.GetThingsAsync();
            if (CurrentOwner != null)
                ChangeOwnersThingsList(CurrentOwner.Id);
        }
        private async void OpenEditThing(Thing thing)
        {
            await page.Navigation.PushAsync(new EditThing(thing, db));
        }

        private async void ChangeOwnersThingsList(int ownerId) =>
            OwnersThings = await db.GetThingsByOwnerIdAsync(ownerId);
        public void Set(ContentPage page)
        {
            this.page = page;
        }
    }
}
