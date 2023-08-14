# Level 5 Bin Files Explained
___________________________________________________________________________

**What is a cfg.bin files?**

The cfg.bin files (or simply .bin files) are binary files used in Level 5's 3DS games.  
These files are compiled binaries that look like a tag structure and store various variables used by the games. For example, they may contain data of Yokais in Yokai Watch or player data for Inazuma Eleven games. These files are not limited to integer or float values; they can also contain text. This tool is designed to simplify the reading and modification of cfg.bin files.

**Value types available**
|  Type | NB Bytes  | Information  | Example  |
|---|---|---|---|
|  Int | 4 | Value without decimal point | 00 00 00 01
|  Float | 4| Value with decimal point | 3f 80 00 00 (= 1.0)
|  Byte | 1 | a number between 0 and 255 | FF (= 255)
|  String |  4 | A string pointer | 00 00 00 01 (refers to "from the strings table, go to offset 01 and read the text until you get an empty character (00)")

**File Structure**  
A .bin file is divided into 4 parts:
- Header (between 0x00 and 0x10)
  |  Type | Information  |
  |---|---|
  |  Int | Number of entries  |
  |  Int | Strings Offset (if the file hasn't text, this is the offset of the key table)  |
  |  Int | Strings Length  |
  |  Int |  Number of strings  |
- Data  
  All elements are encapsulated with tags, each tag is written like this:  
    - Tag Header:  
      |  Type | Information  |
      |---|---|
      |  Int | Crc32 Begin Name Tag (Always end by "BEGIN")|
      |  Byte | Number of variables  |
      |  Byte [] | As long as the byte doesn't equal to FF, read the bytes to obtain the list of variable types  |
      |  Int |  Number of values in the tag  |
  - Tag Entry:
    This sequence is repeated as many times as there are entries in the tag header   
    |  Type | Information  |
    |---|---|
    | Int | Crc32 of one entry of the tag|
    | * | The * means that * ban be Int/Float/Byte/String Pointer and the * is repeated as many times as there are variables defined in the header|
  - Tag Ender:
    |  Type | Information  |
    |---|---|
    |  Int | Crc32 End Name Tag (Always end by "End")|
    |  Int | Terminator, always 00 FF FF FF|
- Strings
  Strings are encoded in UTF8, the byte 00 is used to delimit strings
- Key Table
  A Key Table is divided into 3 parts:  
    - Header:  
      |  Type | Information  |
      |---|---|
      |  Int | Key Table Length |
      |  Int | Number of keys  |
      |  Int |  Strings Offset |
      |  Int |  Strings Length |
    - Keys:
      This sequence is repeated as many times as there are number of keys
      |  Type | Information  |
      |---|---|
      |  Int | Crc32 of key |
      |  Int | String Offset  |
    - Strings:
      Strings are encoded in UTF8, the byte 00 is used to delimit strings
