using PAF.ViewModel;
using PAF.ViewModel.BaseVM;
using System.Windows.Controls;

namespace PAF.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Salay.xaml
    /// </summary>
    public partial class Salay : Page
    {
        public Salay(IPage page)
        {
            InitializeComponent();
            this.DataContext = page;
        }
    }
}
