using PII.Base.Bll;
using PII.Base.BO;
using PII.Base.CryptoLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PII.Base.Helpers
{
    public class ValidateLogin
    {
        //this method will get the user details from the database based on the basis of the given username.
        //The given password will be matched with the retrieved password to see if the credentials are valid or not.
        public bool ValidateLoginCredentials(string userName, string password, bool validate, out DataSet userDetails)
        {
            bool valid = false;
            byte[] origHashedPasswordInBytes = new byte[1];
            string origDecryptedPassword = string.Empty;
            string userId = string.Empty;
            string createdDate = string.Empty;
            byte[] vector;
            byte[] iVector = new byte[1];
            byte[] hashedPass = new byte[1];          

            //get the user details
            Users user = new Users();
            userDetails = user.GetLoginUserDetails(userName);

            if (userDetails != null && userDetails.Tables.Count > 0 && userDetails.Tables[0].Rows.Count > 0)
            {
                //get the original password.. only if need to validate
                if (validate)
                {
                    origHashedPasswordInBytes = (byte[])userDetails.Tables[0].Rows[0]["PasswordBytes"];
                }
                userId = userDetails.Tables[0].Rows[0]["UserId"].ToString();
                createdDate = Convert.ToDateTime(userDetails.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("MMddyyyy");
            }
            else
            {
                return valid;
            }

            //create the Vector
            string[] vectorFiller = { "@", "A", "B", "2", "c", "i", "3", "E" };//this constant will be used to fill the vector if the length is less than 16
            string vectorString = createdDate.Substring(0, 4) + userId + createdDate.Substring(4, 4);
            int vectorLength = vectorString.Length;

            //making sure that the vector string is exactly of 16 characters
            if (vectorLength < 16)
            {
                //if the length is less than 16, append the remaining characters from the constant
                for (int i = 0; i < (16 - vectorLength); i++)
                {
                    vectorString = vectorString + vectorFiller[i];
                }
            }
            else if (vectorLength > 16)
            {
                //if the length is more than 16, truncate the extra characters
                vectorString = vectorString.Substring(0, 16);
            }

            //vector =  EncDecWithAES.GetBytes(createdDate.Substring(0, 4) + userId + createdDate.Substring(4, 4));
            vector = Encoding.ASCII.GetBytes(vectorString);
            //vector = createdDate.Substring(0, 4) + "@AB2cd3E" + createdDate.Substring(4, 4);
            iVector = vector;

            //validate only if needed, else return true by default. We use this method for various functionalities.
            if (validate)
            {
                //validate the password
                //decrypt the original password
                origDecryptedPassword = EncDecWithAES.Decrypt(origHashedPasswordInBytes, vector);

                valid = PasswordHash.ValidatePassword(password, origDecryptedPassword);

                hashedPass = EncDecWithAES.Encrypt(PasswordHash.CreatePasswordHashFromOriginal(password, origDecryptedPassword), vector);
            }
            else
            {
                valid = true;
            }

            return valid;
        }
    }
}
