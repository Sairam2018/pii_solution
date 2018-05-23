/*******************************************************
 * Author       : V U M Sastry Sagi
 * Date         : 10/21/2013
 * Purpose      : Symmetric Encryption and Decryption 
 *                Property Settings Class
 *******************************************************/
using System;
using System.Configuration;

namespace Security
{
    static class EncDecProp
    {
        public static readonly string PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
        public static readonly string HashAlgorithm = ConfigurationManager.AppSettings["HashAlgorithm"];
        public static readonly string SaltValue = ConfigurationManager.AppSettings["SaltValue"];
        public static readonly string InitVector = ConfigurationManager.AppSettings["InitVector"];
        public static readonly int KeySize = Convert.ToInt32(ConfigurationManager.AppSettings["KeySize"]);
        public static readonly int PasswordIterations = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordIterations"]);
    }
}