using System.Data;

namespace PAF.ViewModel.BaseVM
{
    public interface IPage
    {
        /// <summary> Таблица данных </summary>
        DataTable DataTable { get; set; }

        /// <summary> Заполняет таблицу данных записями удовлитворяющие ключевому слову </summary>
        /// <param name="search"> Ключевое слово которое необходимо найти в таблице данных </param>
        void Refresh(string search);
        /// <summary> Заполняет таблицу данных из бд </summary>
        void Refresh();
    }
}
