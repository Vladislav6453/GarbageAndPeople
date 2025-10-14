using GarbageAndPeople.VM;

namespace GarbageAndPeople
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            var page = new MainPageVM();
            page.Set(this);
            BindingContext = page;
        }
    }
}
