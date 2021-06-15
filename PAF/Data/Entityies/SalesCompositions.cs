namespace PAF.Data.Entityies
{
    public class SalesCompositions : ViewModel.ViewModel
    {
        /// <summary> Id состава продажи </summary>
        public int Id { get; set; }
        /// <summary> Цена реализации </summary>
        public decimal Price { get; set; }
        /// <summary> Количество </summary>
        public decimal Amount { get; set; }
        /// <summary> Стоимость </summary>
        public decimal Sum
        {
            get => Sum;
            set => Sum = Price * Amount;
        }
        /// <summary> Id компонента </summary>
        public Components Component { get; set; }
        /// <summary> Номер продажи </summary>
        public Sales Sale { get; set; }
    }
}