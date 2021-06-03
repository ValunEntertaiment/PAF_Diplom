using PAF.ViewModel;
using System.Windows;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для DeliveryAdd.xaml
    /// </summary>
    public partial class DeliveryAdd : Window
    {
        public DeliveryAdd()
        {
            InitializeComponent();
            DataContext = new DeliveryAddVM();
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
