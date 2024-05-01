# [CfgBinEditor](https://github.com/Tiniifan/CfgBinEditor/releases/latest)
___________________________________________________________________________

**What is a cfg.bin files?**

The cfg.bin files (or simply .bin files) are binary files used in Level 5's 3DS games.  
These files are compiled binaries that look like a tag structure and store various variables used by the games.  
For example, they may contain data of Yokais in Yokai Watch or player data for Inazuma Eleven games. These files are not limited to integer or float values; they can also contain text.  
This tool is designed to simplify the reading and modification of cfg.bin files.  
This tool is built using some parts of the code from Togenyan's [CfgBinEditor](https://github.com/togenyan/CfgBinEditor).  
Here's another cfgbin editor that's better than mine : [CfgBinEditor by onepiecefreak3](https://github.com/onepiecefreak3/CfgBinEditor/releases/latest)

**Make the tool powerful**  
Even though the tool can read .cfg.bin files, these files don't store variable names, so it's pretty hard to understand the files.  
That's why the tool has 2 ways of making it more powerful. To do this, you need to create these files in the same place as the .exe:  
- MyTags.txt: with this file you can give variable names to inputs, and when the tool recognizes your inputs it will display the variable names.
```
YKW2 [
	TEXT_INFO (
		TextID|True
		Number|False
		Text|False
		Unk|False
	)
]
```
This is an example of a Mytags.txt file, you must respect this a tag always starts with its name and a bracket, below it you have the sub_tags, the sub_tags always start with a name and a parenthesis, the sub_tag is used to recognize an entry. example here this sub_tag is the sub_tag of names for level 5 files, then you must put the name you want to assign to the variable followed by a bracket and a True OR False to say if you want to display this variable as a hexadecimal.  
- MyIDs.txt: this file allows the tool to recognize int files that are actually hashed and usually point to other files
```
YKW2 [
	YokaiParam (
		0x79F3AA36|Pandle 
		0x7D7ED684|Pandull
		0x6B4605D8|Undy
	)
]
```
the structure is generally the same as MyTags.txt, becarefull ids must start with a 0x and be written in little endian format

**Screenshots**

![](https://github.com/Tiniifan/CfgBinEditor/assets/30804632/5b767c83-36d9-47f1-b34b-267b6f48d761)

