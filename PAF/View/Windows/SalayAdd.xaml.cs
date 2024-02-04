using PAF.ViewModel;
using System.Windows;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для SalayAdd.xaml
    /// </summary>
    public partial class SalayAdd : Window
    {
        public SalayAdd()
        {
            InitializeComponent();
            this.DataContext = new SalayVM();
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
