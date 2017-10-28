using System;
namespace RevStackCore.AuthorizeDotNet
{
    public class ANetLineItem
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool? Taxable { get; set; }
    }
}
