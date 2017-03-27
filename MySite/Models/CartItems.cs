using System;
using System.ComponentModel.DataAnnotations;

namespace MySite.Models
{
    public class CartItems
    {
        [Display(Name = "Item #")]
        [StringLength(4)]
        [MinLength(4)]
        public string ItemId { get; set; }

        [Display(Name = "CartItemModel")]
        public string ItemPicture { get; set; }
        public string ItemCatagory { get; set; }
        public string ItemName { get; set; }
        public decimal ItemCost { get; set; }
        public int ItemQty { get; set; }
        public int AvailQty { get; set; }
        public decimal subTotal { get { return ItemCost * ItemQty; } set { subTotal = ItemCost * ItemQty; } }

        public Array[] magnetItems { get; set; }
        public Array[] clothingItems { get; set; }


    }

    
}