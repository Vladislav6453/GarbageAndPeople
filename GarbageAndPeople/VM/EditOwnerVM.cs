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
        private string fName = string.Empty;
        private string lName = string.Empty;
        private string phone = string.Empty;

        public Command Redacting { get; set; }
        public Owner? Owner
        {
            get => owner;
            set
            {
                owner = value;
                Signal();
                //Redacting.ChangeCanExecute();
            }
        }

        public string FName
        {
            get => fName;
            set
            {
                fName = value;
                Signal();
                Redacting.ChangeCanExecute();
            }
        }
        public string LName
        {
            get => lName;
            set
            {
                lName = value;
                Signal();
                Redacting.ChangeCanExecute();
            }
        }
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                Signal();
                Redacting.ChangeCanExecute();
            }
        }
        public EditOwnerVM()
        {
            Redacting = new Command(async () =>
            {
                Owner.FirstName = FName.Trim();
                Owner.LastName = LName.Trim();
                Owner.Email = Owner.Email.Trim();
                Owner.PhoneNumber = Phone.Trim();

                await db.ChangeOwner(Owner);
                await page.Navigation.PopAsync();
            }, () => !string.IsNullOrEmpty(FName.Trim()) && 
            !string.IsNullOrEmpty(LName.Trim()) &&
            !string.IsNullOrEmpty(Phone.Trim()));

        }
        public void Set(Owner owner, Database db, ContentPage page)
        {
            Owner = owner;
            this.db = db;
            this.page = page;
        }
    }
}
