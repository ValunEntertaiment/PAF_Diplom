using PAF.ViewModel.BaseVM;
using System.Windows.Controls;

namespace PAF.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        public Client(IPage clientVM)
        {
            InitializeComponent();
            this.DataContext = clientVM;
        }
    }
}
