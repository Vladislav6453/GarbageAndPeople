using GarbageAndPeople.Models;
using GarbageAndPeople.Models.DB;
using GarbageAndPeople.VM;

namespace GarbageAndPeople.View;

public partial class EditOwner : ContentPage
{
	public EditOwner(Owner owner, Database db)
	{
		InitializeComponent();
		var vm = new EditOwnerVM();
		vm.Set(owner, db, this);
		BindingContext = vm;
	}
}