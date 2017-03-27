using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MySite.Models
{


    [MetadataType(typeof(MagnetMetadata))]
    public partial class Magnets
    {
    }

    [MetadataType(typeof(ClothingMetadata))]
    public partial class Clothing
    {
        public string selectedCatagory { get; set; }
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class Users
    {
        public bool RememberMe { get; set; }

        public string oldPass { get; set; }
        public string newPass { get; set; }
        [Compare("newPass")]
        public string confirmPass { get; set; }

        /// <summary>
        /// Checks if user with given password exists in the database
        /// </summary>
        /// <param name="_username">User name</param>
        /// <param name="_password">User password</param>
        /// <returns>True if user exist and password is correct</returns>
        public bool IsValid(string _username, string _password)
        {
            TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();

            _password = Helpers.SHA1.Encode(_password);
            /*************************************************************
             *  LINQ extention methods: bools All, Any, Contains. Contains needs defined comparison operator.
             * **********************************************************/
            return db.Users.Any(u => u.Username == _username && u.Password == _password);

        }
    }


}