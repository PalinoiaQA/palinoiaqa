using System.Text;

namespace Entities
{    
    /// <summary>
    /// class that defines a viewUser object which is used to transport user data
    /// </summary>
    public class viewUser
    {
        #region properties and variables

        /// <summary>
        /// class variable to store user ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable to store user role ID
        /// </summary>
        public int UserRoleID { get; set; }
        /// <summary>
        /// class variable to store user first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// class variable to store user last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// class variable to store user middle initial
        /// </summary>
        public string MiddleInitial { get; set; }
        /// <summary>
        /// class variable to store user email address
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// class variable to store user password
        /// </summary>
        public string PW { get; set; }
        /// <summary>
        /// class variable to store user active status
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// class variable to store user role name
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// class variable to store UpdatedBy user id
        /// </summary>
        public int UpdatedBy { get; set; }

        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor for viewUser object
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="userRoleID">int</param>
        /// <param name="firstName">string</param>
        /// <param name="lastName">string</param>
        /// <param name="middleInitial">string</param>
        /// <param name="email">string</param>
        /// <param name="pw">string</param>
        /// <param name="active">bool</param>
        /// <param name="roleName">string</param>
        /// <param name="updatedBy">int</param>
        public viewUser(int id,
                        int userRoleID,
                        string firstName,
                        string lastName,
                        string middleInitial,
                        string email,
                        string pw,
                        bool active,
                        string roleName,
                        int updatedBy)
        {
            this.ID = id;
            this.UserRoleID = userRoleID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MiddleInitial = middleInitial;
            this.Email = email;
            this.PW = pw;
            this.Active = active;
            this.RoleName = roleName;
            this.UpdatedBy = updatedBy;
        }

        #endregion constructors

        #region public methods

        /// <summary>
        /// return formatted name, last name first
        /// ex: Doe, John A.
        /// </summary>
        /// <returns>string</returns>
        public string getFullNameLNF() 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.LastName);
            sb.Append(", ");
            sb.Append(this.FirstName);
            if (this.MiddleInitial.Length > 0)
            {
                sb.Append(" ");
                sb.Append(MiddleInitial);
                sb.Append(".");
            }
            return sb.ToString();
        }

        /// <summary>
        /// returns formated name, first name first
        /// ex: John A. Doe
        /// </summary>
        /// <returns>string</returns>
        public string getFullNameFNF()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.FirstName);
            sb.Append(" ");
            if (this.MiddleInitial.Length > 0)
            {
                sb.Append(MiddleInitial);
                sb.Append(". ");
            }
            sb.Append(this.LastName);
            
            return sb.ToString();
        }

        #endregion public methods
    }
}
