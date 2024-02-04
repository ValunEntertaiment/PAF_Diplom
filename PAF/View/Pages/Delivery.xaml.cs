using PAF.ViewModel.BaseVM;
using System.Windows.Controls;

namespace PAF.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Delivery.xaml
    /// </summary>
    public partial class Delivery : Page
    {
        public Delivery(IPage page)
        {
            InitializeComponent();
            this.DataContext = page;
        }
    }
}
