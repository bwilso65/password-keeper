password-keeper
===============

Console application that manages your passwords.
Randomly generates an alpha-numeric-symbolic password of length between 14 and 18 characters for an entered site and saves it to a text file at the location of your choice. I choose to store my file in an encrypted section of my drive using the open source software TrueCrypt(http://www.truecrypt.org/).

When looking up a password, you have the option to copy the password to the clipboard, or to display the password on the console.

Usage
===============
Paramaters:
* --a       -->      Adds a new site
* --p      -->    Prints the password to console instead of copying to clipboard
* -f={filePath}    -->  Sets the file path to the location of the 'txt' file
* -s={site-name}   -->  Sets the site name for the entry to insert of lookup
