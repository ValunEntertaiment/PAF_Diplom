using PAF.ViewModel;
using PAF.ViewModel.BaseVM;
using System.Windows;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для SalayCompositionAdd.xaml
    /// </summary>
    public partial class SalayCompositionAdd : Window
    {
        public SalayCompositionAdd(IPage page)
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
