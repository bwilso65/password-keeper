using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace password_keeper.Utility
{
    static class Utilities
    {
        //return true if site exists, false otherwise
        public static bool entryExists(String site, String fileName)
        {
            try
            {
                //assuming the file isn't going to be very big, so I can read it all into memory
                string[] content = File.ReadAllLines(fileName);
                foreach (String line in content)
                {
                    if (line.Contains(site)) return true;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return false;
            }
            return false;
        }

        //creates a new entry containing a site name and an associated password
        public static bool entryInsert(String site, String fileName)
        {
            try
            {
                StreamWriter sw = File.AppendText(fileName);
                sw.WriteLine("\n" + site + " : " + generatePassword() + "\n");
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        //Overwrites an existing password with a new one
        public static bool entryOverwrite(String site, String fileName)
        {
            try
            {
                List<String> lines = File.ReadAllLines(fileName).ToList<String>();
                String newLine = null;
                foreach (String line in lines)
                {
                    if (line.Contains(site))
                    {
                        newLine = site + " : " + generatePassword();
                        lines.Remove(line);
                        lines.Add(newLine);
                        break;
                    }
                }
                File.WriteAllLines(fileName, lines.Where(m=>m.Length>1));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        //method to generate a random password
        private static String generatePassword()
        {
            string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strPwd = "";

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random rnd = new Random(seed);

            //generate the password
            for (int i = 0; i <= rnd.Next(12, 18); i++)
            {
                int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                strPwd += strPwdchar.Substring(iRandom, 1);
            }
            return strPwd;

        }
    }
}
