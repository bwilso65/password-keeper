/*
 * This program retreives login information stored on a local file
 * and saves it to the clipboard.
 * 
 * Paramters: 'site-name'
 * Options: --p  --> prints the login information instead of copying to clipboard
 *          --a  --> adds a new site with provided 'site-name' and creates a new password
 *               --> if a site already exists, prompts the user if he/she wishes to overwrite
 *          --f  --> location that the password file is located at
 * Created By: Brad Wilson
 * Version 0.1a
 * Date: April 10th, 2014
 * 
 * Distributed using MIT license
 */
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using password_keeper.Utility;

namespace password_keeper
{
    class passwordKeeper
    {
        static void Main(string[] args)
        {
            //Variables
            Boolean print = false;
            String fileName = null, siteName = null;
            //checking for empty command lines
            if (args.Length == 0)
            {
                Console.WriteLine("Arguments cannot be empty...'-s and -f' must be present");
                //Console.WriteLine("Use with --s {site-name}");
                return;
            }
            //parse the command line
            Arguments CommandLine = new Arguments(args);
            //checking if file name is provided
            if (CommandLine["f"] != null)
            {
                fileName = CommandLine["f"];
                //check if file exists
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("Could not find a file with that file name...please check your value");
                    return;
                }
            }
            else
            {
                Console.WriteLine("File Name must be supplied.");
                Console.WriteLine("Please enter it using -f {fileName}");
                return;
            }
            //check if site name is present
            if (CommandLine["s"] != null)
            {
                siteName = CommandLine["s"];
            }
            else
            {
                Console.WriteLine("Site Name must be supplied.");
                Console.WriteLine("Please enter it using -s {site-name}");
                return;
            }

            //check if printing or putting to clipboard
            if (CommandLine["p"] != null) print = true;

            //make a new entry
            if (CommandLine["a"] != null)
            {
                String c = null;
                if (entryExists(siteName, fileName))
                {
                    //ask if user wants to replace
                    Console.WriteLine("A site with that name already exists.");
                    do
                    {
                        Console.Write("Do you want to replace with a new value? ");
                        c = Console.ReadLine();
                        //c = Console.ReadLine().ElementAt(0);
                    } while (c != "n" && c != "y");

                    //overwrite the appropriate line
                    if (entryOverwrite(siteName, fileName))
                    {
                        Console.WriteLine(siteName + " added to list.");
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong trying to add the site");
                        return;
                    }
                }
                else
                {
                    //add new entry
                    if (entryInsert(siteName, fileName))
                    {
                        Console.WriteLine(siteName + " added to list.");
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong trying to add the site");
                        return;
                    }
                }
            }

            //done parsing command line...
            //time to retrieve the login information and display it or copy to clipboard
            String pswd = null;
            string[] lines = File.ReadAllLines(fileName);
            pswd = lines.First(m => m.Contains(siteName));
            String[] pswdSplit = pswd.Split(':');
            pswd = pswdSplit[1].Trim();

            if (print == true)
            {
                Console.WriteLine(pswd);
            }
            else
            {
                Clipboard.SetText(pswd);   
            }
        }

        //return true if site exists, false otherwise
        private static bool entryExists(String site, String fileName)
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

        //return true if inserted successfully, false otherwise
        private static bool entryInsert(String site, String fileName)
        {
            try
            {
                StreamWriter sw = File.AppendText(fileName);
                sw.WriteLine("\n" + site + " : " + generatePassword());
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        private static bool entryOverwrite(String site, String fileName)
        {
            try
            {
                List<String> lines = File.ReadLines(fileName).ToList<String>();
                String newLine = null;
                foreach (String line in lines)
                {
                    if (line.Contains(site))
                    {
                        newLine = "\n" + site + " : " + generatePassword();
                        lines.Remove(line);
                        lines.Add(newLine);
                    }
                }
                File.WriteAllLines(fileName, lines);
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

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
            for (int i = 0; i <= rnd.Next(12,18); i++)
            {
                int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                strPwd += strPwdchar.Substring(iRandom, 1);
            }
            return strPwd;
            
        }
    }
}
