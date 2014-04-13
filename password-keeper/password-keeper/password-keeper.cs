/*
 * This program retreives login information stored on a local file
 * and saves it to the clipboard.
 * 
 * Options: --p  --> prints the login information instead of copying to clipboard
 *          --a  --> adds a new site with provided 'site-name' and creates a new password
 *               --> if a site already exists, prompts the user if he/she wishes to overwrite
 *          -f   --> location that the password file is located at
 *          -s   --> the name for the site entry. Used to lookup the password
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
        [STAThread]
        static void Main(string[] args)
        {
            //Variables
            Boolean print = false;
            String fileName = null, siteName = null;
            //checking for empty command lines
            if (args.Length == 0)
            {
                Console.WriteLine("Arguments cannot be empty...'-s' and '-f' must be present");
                //Console.WriteLine("Use with --s {site-name}");
                return;
            }
            //parse the command line
            Arguments CommandLine = new Arguments(args);
            //checking if file name is provided
            #region "F"
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
            #endregion
            //check if site name is present
            #region "S"
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
            #endregion
            //check if printing or putting to clipboard
            if (CommandLine["p"] != null) print = true;
            #region "a"
            //make a new entry
            if (CommandLine["a"] != null)
            {
                String c = null;
                if (Utilities.entryExists(siteName, fileName))
                {
                    //ask if user wants to replace
                    Console.WriteLine("A site with that name already exists.");
                    do
                    {
                        Console.Write("Do you want to replace with a new value? ");
                        c = Console.ReadLine();
                        //c = Console.ReadLine().ElementAt(0);
                    } while (c != "n" && c != "y");

                    if (c == "y")
                    {
                        //overwrite the appropriate line
                        if (Utilities.entryOverwrite(siteName, fileName))
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
                else
                {
                    //add new entry
                    if (Utilities.entryInsert(siteName, fileName))
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
            #endregion

            //done parsing command line...
            //time to retrieve the login information and display it or copy to clipboard
            String pswd = null;
            string[] lines = File.ReadAllLines(fileName);
            try
            {
                pswd = lines.First(m => m.Contains(siteName));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not find an entry with that site name.");
                return;
            }
            String[] pswdSplit = pswd.Split(':');
            pswd = pswdSplit[1].Trim();

            //display to user or set to clipboard
            if (print == true)
            {
                Console.WriteLine(pswd);
            }
            else
            {
                Clipboard.SetText(pswd);
                Console.WriteLine("Password saved onto clipboard");
            }
        }
    }
}
