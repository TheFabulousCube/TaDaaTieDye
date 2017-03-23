using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MySite.Helpers;

namespace MySite.Models
{
    /********************************************
     * Custon Annotations:
     * [ExcludeChar(The characters you want to exclude)]
     * [StripHtmlTags(RegEx for the tags to exclude)]
     * *****************************************/

    public class MagnetMetadata
    {
        // Specifically for derived Magnets class
        [Required]
        [StringLength(4)]
        [Display(Name = "Item Code")]
        public string ProdId;

        [DataType(DataType.ImageUrl)]
        [Display(Name = "")]
        public string ProdPicture;

        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public decimal ProdPrice;

        [Display(Name = "Available")]
        public int ProdQty;

        public string Catagory;

        [StringLength(15)]
        [Display(Name = "State")]
        public string ProdName;

        [StringLength(20)]
        [Display(Name = "Capital")]
        public string Capital;

        [StringLength(25)]
        [Display(Name = "Date of Statehood")]
        public string Statehood;
    }

    public class ClothingMetadata
    {
        // Specifically for derived Clothing class
        [Required]
        [StringLength(4)]
        [Display(Name = "Item Code")]
        public string ProdId;

        [Display(Name = "Front")]
        public string ProdPicture;

        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public decimal ProdPrice;

        [Display(Name = "Available")]
        public int ProdQty;

        [Display(Name = "Type")]
        public string Catagory;

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Back")]
        public string BackPicture;

        [Display(Name = "Size")]
        public string Size;
        [Display(Name = "Sleeve Length")]
        public string SleeveLength;

        //public virtual Products Products;
        //public virtual Size_Lookup Size_Lookup;
        //public virtual Sleeve_Lookup Sleeve_Lookup;
    }

    public class UserMetadata
    {


        public int Id;

        [Required]
        [Display(Name = "User name")]
        [StringLength(12, ErrorMessage = "12 characters is enough for a username")]
        [MinLength(4, ErrorMessage = "Try to come up with at least 4 characters for your username")]
        [ExcludeChar(";'/`\"[]{}.,\\")]
        [StripHtmlTags("<.*?>")]
        public string Username;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(60)]
        [MinLength(6, ErrorMessage = "Come on, at least 6 characters in your password")]
        [ExcludeChar(";'/`\"[]{}.,\\")]
        [StripHtmlTags("<.*?>")]
        public string Password;


        [Display(Name = "First Name")]
        [StringLength(25, ErrorMessage = "Sorry, try to keep your name under 25 characters (Use a nickname, if you need to.)")]
        [ExcludeChar(";/`\"[]{}.,\\")]
        [StripHtmlTags("<.*?>")]
        public string Fname;

        [Display(Name = "Last Name")]
        [StringLength(25, ErrorMessage = "Sorry, only enter the first 25 characters")]
        [ExcludeChar(";/`\"[]{}.,\\")]
        [StripHtmlTags("<.*?>")]
        public string Lname;

        [Display(Name = "Address")]
        [ExcludeChar(";'`\"[]{}\\")]
        [StripHtmlTags("<.*?>")]
        [StringLength(45, ErrorMessage = "Sorry, try to keep your address under 45 characters")]
        [MinLength(4, ErrorMessage = "That doesn't look like a real address (too short)")]
        [DataType(DataType.Text)]
        public string Address;

        [Display(Name = "City")]
        [StringLength(20, ErrorMessage = "Sorry, try to keep the city name under 20 characters")]
        [MinLength(2, ErrorMessage = "That doesn't look like a real city name (too short)")]
        [ExcludeChar(";'/`\"[]{}.,\\")]
        [StripHtmlTags("<.*?>")]
        public string City;

        [Display(Name = "State")]
        [RegularExpression(@"[a-zA-Z]{2}", ErrorMessage = "Just enter the 2 letter  postal abbreviation")]
        public string State;

        [DataType(DataType.PostalCode)]
        [StringLength(5, ErrorMessage = "You don't need to enter the ZIP+4")]
        [MinLength(5, ErrorMessage = "U.S. {0}s have 5 numbers")]
        [RegularExpression(@"[0-9]{5}", ErrorMessage = "U.S. {0}s are all numbers")]
        public string Zip;

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(45, ErrorMessage = "Sorry, I only made room for 45 characters in the email")]
        public string Email;


        public string Role;

        [Display(Name = "Remember me on this computer")]
        public bool RememberMe;

        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        [StringLength(40)]
        [MinLength(6, ErrorMessage = "Come on, at least 6 characters")]
        [ExcludeChar(";'/`\"[]{}.,\\")]
        [StripHtmlTags("<.*?>")]
        public string oldPass;

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(40)]
        [MinLength(6, ErrorMessage = "{0} must at least 6 characters")]
        [ExcludeChar(";'/`\"[]{}.,\\")]
        [StripHtmlTags("<.*?>")]
        public string newPass;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [StringLength(40)]
        [MinLength(6)]
        [ExcludeChar(";'/`\"[]{}.,\\")]
        [StripHtmlTags("<.*?>")]
        public string confirmPass;


    }
}