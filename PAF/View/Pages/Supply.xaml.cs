using PAF.ViewModel;
using PAF.ViewModel.BaseVM;
using System.Windows.Controls;

namespace PAF.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Supply.xaml
    /// </summary>
    public partial class Supply : Page
    {
        public Supply(IPage page)
        {
            InitializeComponent();
            this.DataContext = page;
        }
    }
}
