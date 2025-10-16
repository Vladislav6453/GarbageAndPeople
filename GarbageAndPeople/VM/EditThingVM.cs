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
        private string title = string.Empty;

        public List<Owner> Owners
        {
            get => owners;
            set
            {
                owners = value;
                Signal();
            }
        }

        public Command Redacting { get; set; }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                Signal();
                Redacting.ChangeCanExecute();
            }
        }
        public EditThingVM() 
        {
            Redacting = new Command(async () =>
            {
                Thing.Title = Thing.Title.Trim();
                Thing.OwnerId = Thing.Owner?.Id;
                Thing.Description = Thing.Description.Trim();
                await db.AddThing(Thing);
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
