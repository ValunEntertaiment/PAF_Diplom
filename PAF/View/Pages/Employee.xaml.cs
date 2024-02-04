using PAF.ViewModel.BaseVM;
using System.Windows.Controls;

namespace PAF.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class Employee : Page
    {
        public Employee(IPage vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
