using GarbageAndPeople.Models;
using GarbageAndPeople.Models.DB;
using GarbageAndPeople.VM.VMTools;
using System.Threading.Tasks;

namespace GarbageAndPeople.VM
{
    public class EditThingVM : BaseVM
    {
        private Thing? thing;
        public Thing? Thing
        {
            get => thing;
            set
            {
                thing = value;
                Signal();
            }
        }
        private Database db;
        private ContentPage page;
        private List<Owner> owners;

        public List<Owner> Owners
        {
            get => owners;
            set
            {
                owners = value;
                Signal();
            }
        }

        public CommandVM Redacting { get; set; }
        public EditThingVM() 
        {
            Redacting = new CommandVM(async () =>
            {
                Thing.Title = Thing.Title.Trim();
                Thing.OwnerId = Thing.Owner?.Id;
                await db.AddThing(Thing);
                await db.SaveThingsAsync();
                await page.Navigation.PopAsync();
            }, () => !string.IsNullOrEmpty(Thing?.Title.Trim()));

        }

        public async void LoadLists()
        {
            Owners = await db.VerniMneSpisokOwner();
            Thing.Owner = Owners.FirstOrDefault(o => o.Id == Thing.OwnerId);
        }

        public void Set(Thing thing, Database db, ContentPage page)
        {
            Thing = thing;
            this.db = db;
            this.page = page;
            LoadLists();
        }
    }
}
