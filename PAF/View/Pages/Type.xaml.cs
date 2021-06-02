using PAF.ViewModel;
using PAF.ViewModel.BaseVM;
using System.Windows.Controls;

namespace PAF.View.Pages
{
    public partial class Type : Page
    {
        public Type(IPage page)
        {
            InitializeComponent();
            this.DataContext = page;
        }
    }
}
