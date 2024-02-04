using System.Collections.Generic;

namespace PAF.Data.Entityies
{
    public class Components : ViewModel.ViewModel
    {
        /// <summary> Id компонента </summary>
        public int Id { get; set; }
        /// <summary> Название компонента </summary>
        public string Name { get; set; }
        /// <summary> Id компонента </summary>
        public Types Type { get; set; }
        /// <summary> Единиц на складе </summary>
        public int Amount { get; set; }
        /// <summary> Цена товара </summary>
        public decimal Price { get; set; }
        /// <summary> Id поставщика </summary>
        public Supplies Supply { get; set; }
        /// <summary> Id состава продажи </summary>
        public List<SalesCompositions> SaleComposition { get; set; }
        /// <summary> Id состава поставки </summary>
        public List<DeliveriesCompositions> DeliveryComposition { get; set; }
    }
}