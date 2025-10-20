using GarbageAndPeople.VM;

namespace GarbageAndPeople
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            ((MainPageVM)BindingContext).Set(this);
        }
    }
}
