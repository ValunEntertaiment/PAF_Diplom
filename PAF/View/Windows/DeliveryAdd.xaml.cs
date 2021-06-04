using PAF.ViewModel;
using System;
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
            DeliveryAddVM VM = new DeliveryAddVM();
            DataContext = VM;
            if (VM.CloseAction == null) VM.CloseAction = new Action(() => this.Close());
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
