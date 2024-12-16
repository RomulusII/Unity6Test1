namespace Model
{
    public class ProductItem
    {

        public double Adet { get; set; }
        public ProductType Tip { get; }

        public ProductItem(ProductType tip)
        {
            Tip = tip;
        }
    }
}
