using GarbageAndPeople.Models;
using GarbageAndPeople.Models.DB;
using GarbageAndPeople.VM;

namespace GarbageAndPeople.View;

public partial class EditThing : ContentPage
{
	public EditThing(Thing thing, Database db)
	{
		InitializeComponent();
        ((EditThingVM)BindingContext).Set(thing, db, this);
    }
}