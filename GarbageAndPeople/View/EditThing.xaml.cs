using GarbageAndPeople.Models;
using GarbageAndPeople.Models.DB;
using GarbageAndPeople.VM;

namespace GarbageAndPeople.View;

public partial class EditThing : ContentPage
{
	public EditThing(Thing thing, Database db)
	{
		InitializeComponent();
		var vm = new EditThingVM();
		vm.Set(thing, db);
		BindingContext = vm;
	}
}