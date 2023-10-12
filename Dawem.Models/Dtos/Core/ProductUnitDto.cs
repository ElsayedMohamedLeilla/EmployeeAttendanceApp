namespace Dawem.Models.Dtos.Core
{
    public class ProductUnitDto
    {
        public int Id { get; set; }
        public int UnitId { get; set; }

        public virtual UnitDTO Unit { get; set; }


        public int BranchId { get; set; }
        public bool IsMainUnit { get; set; }


        public float UnitRate { get; set; }


        public float? ProductSalesTaxRate { get; set; }


        public float? ProductPurchaseTaxRate { get; set; }

        public float ProductBuyPrice { get; set; }

        public float ProductSalePrice { get; set; }


        public bool IsAdditionalTax { get; set; }
    }
}
