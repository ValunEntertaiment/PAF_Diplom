using PAF.ViewModel.BaseVM;
using System.Windows;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для SupplyAdd.xaml
    /// </summary>
    public partial class SupplyAdd : Window
    {
        public SupplyAdd(IPage page)
        {
            InitializeComponent();
            this.DataContext = page;
        }

        private void StackPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {

            }
        }
    }
}
