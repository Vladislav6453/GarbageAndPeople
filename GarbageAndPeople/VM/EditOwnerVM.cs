using GarbageAndPeople.Models;
using GarbageAndPeople.Models.DB;
using GarbageAndPeople.VM.VMTools;

namespace GarbageAndPeople.VM
{
    public class EditOwnerVM : BaseVM
    {
        private Owner? owner;
        private Database db;
        private ContentPage page;
        public CommandVM Redacting { get; set; }
        public EditOwnerVM()
        {
            Redacting = new CommandVM(async () =>
            {
                Owner.FirstName = Owner.FirstName.Trim();
                Owner.LastName = Owner.LastName.Trim();
                Owner.Email = Owner.Email.Trim();
                Owner.PhoneNumber = Owner.PhoneNumber.Trim();

                await db.SaveOwnersAsync();
                await page.Navigation.PopAsync();
            }, () => !string.IsNullOrEmpty(Owner?.FirstName.Trim()) && 
            !string.IsNullOrEmpty(Owner?.LastName.Trim()) &&
            !string.IsNullOrEmpty(Owner?.PhoneNumber.Trim()));

        }
        public Owner? Owner
        {
            get => owner;
            set
            {
                owner = value;
                Signal();
            }
        }
        public void Set(Owner owner, Database db, ContentPage page)
        {
            Owner = owner;
            this.db = db;
            this.page = page;
        }
    }
}
