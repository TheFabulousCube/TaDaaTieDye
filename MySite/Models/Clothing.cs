//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MySite.Models
{

    public partial class Clothing
    {
        public string ProdId { get; set; }
        public string ProdPicture { get; set; }
        public decimal ProdPrice { get; set; }
        public int ProdQty { get; set; }
        public string Catagory { get; set; }
        public string BackPicture { get; set; }
        public string Size { get; set; }
        public string SleeveLength { get; set; }
    
        public virtual Catagory_Lookup Catagory_Lookup { get; set; }
        public virtual Size_Lookup Size_Lookup { get; set; }
        public virtual Sleeve_Lookup Sleeve_Lookup { get; set; }
    }
}
