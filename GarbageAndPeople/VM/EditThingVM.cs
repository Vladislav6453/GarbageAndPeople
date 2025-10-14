using GarbageAndPeople.Models;
using GarbageAndPeople.Models.DB;
using GarbageAndPeople.VM.VMTools;
using System.Threading.Tasks;

namespace GarbageAndPeople.VM
{
    public class EditThingVM : BaseVM
    {
        private Thing thing;
        public Thing Thing
        {
            get => thing;
            set
            {
                thing = value;
                Signal();
            }
        }
        private Database db;


        public List<Owner> Owners { get; set; }
        public EditThingVM() 
        {
            LoadLists();
        }

        public async void LoadLists()
        {
            Owners = await db.VerniMneSpisokOwner();
            Thing.Owner = Owners.FirstOrDefault(o => o.Id == Thing.OwnerId);
        }

        public void Set(Thing thing, Database db)
        {
            Thing = thing;
            this.db = db;
        }
    }
}
