using PAF.Commands.Base;
using PAF.ViewModel.BaseVM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class DeliveryAddVM : ViewModelForWindow, IPage
    {
        #region Prorerties
        bool CanButtonClick=true;

        public DataTable DataTable { get => _DataTable; set => Set(ref _DataTable, value); }
        DataTable _DataTable;

        public DataRow SelectedSupply { get => _SelectedSupply; set => Set(ref _SelectedSupply, value); }
        DataRow _SelectedSupply;
        #endregion

        private void Refresh()
        {
            string query =
                    "select " +
                    "c.Id Код, " +
                    "c.[Name] 'Наименование товара', " +
                    "c.Amount Количество, " +
                    "s.[Name] Поставщик, " +
                    "t.[name] Тип " +
                    "from components c " +
                    "inner join Supplies s on s.Id = c.Supply_Id " +
                    "inner join Types t on t.Id = c.[Type_Id] ";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable temp = new DataTable();
                    adapter.Fill(temp);
                    DataTable = temp; //добавил temp чтобы срабатывал set у свойства
                }
            }
            catch (Exception x)
            {
                System.Windows.MessageBox.Show(x.Message);
            }
        }

        #region Commands

        #region AddCommand
        public ICommand AddCommand { get; set; }

        private bool CanAddExecute(object p) => CanButtonClick;
        private void OnAddExecuted(object p)
        {
            CanButtonClick = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.csv) | *.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;

                string[] file = File.ReadAllLines(filename);
                string[] row;

                foreach (var item in file)
                {
                    row = item.Split(';');
                }
            }



















            CanButtonClick = true;
        }
        #endregion

        #endregion

        public DeliveryAddVM()
        {
            #region Commands
            AddCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            #endregion

            Refresh();
        }

    }
}
