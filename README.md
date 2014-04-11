password-keeper
===============

Console application that manages your passwords.
Randomly generates an alpha-numeric-symbolic password of length between 14 and 18 characters for an entered site and saves it to a text file at the location of your choice.

When looking up a password, you have the option to copy the password to the clipboard, or to display the password on the console.

Some helpful tips:
* Store the text file somewhere safe such as an encrypted drive (http://www.truecrypt.org/).
* Store the text file and program on a flash drive so you have your passwords with you --> Risky if you don't maintain 100% ownership of the drive.

Usage
===============
Paramaters:
* --a       -->      Adds a new site
* --p      -->    Prints the password to console instead of copying to clipboard
* -f={filePath}    -->  Sets the file path to the location of the 'txt' file
* -s={site-name}   -->  Sets the site name for the entry to insert of lookup
