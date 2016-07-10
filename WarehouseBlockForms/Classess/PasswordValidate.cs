using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WarehouseBlockForms.Classess
{
    class PasswordValidate
    {
        private PasswordValidate() { }

        public static bool Validate (string password)
        {
            IniFile iniFile = new IniFile("settings.ini");
            if(!iniFile.KeyExists("phash"))
            {
                if(password == "adminpassword")
                {
                    return true;
                }
                return false;
            }

            if(iniFile.Read("phash") == CalculateHash(password))
            {
                return true;
            }
            return false;
        }

        public static string CalculateHash(string password)
        {
            MD5 md5 = MD5.Create();

            byte[] input = Encoding.UTF8.GetBytes(password);
            byte[] hash = md5.ComputeHash(input);


            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }


            return sb.ToString();
        }

    }
}
