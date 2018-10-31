## Eg-Lana Architect features
Eg-Lana Architect is a simple tool that allows you to view various hidden details about La-Mulana 2
* Shows the current score value
* Shows the contents of pots
* Displays changes in flag values, and writes them to a log file (flags.log)
* Shows an in-game timer on-screen
* Save Anywhere feature, press F5 to access the save menu from every location with a grail point!

Based on the excellent [MonoMod patcher](https://github.com/0x0ade/MonoMod), for anyone looking to do similar modifications to other games, it's a great tool to distribute your modifications without having to include the entire source of the game in your project!
## How to run
1. Create a copy of the game folder to run the mod from 
2. Download the latest EgLanaArchitect.7z from https://github.com/Goost91/EgLanaArchitect/releases
3. Open the copied folder and extract the zip in the same folder as Lamulana2.exe
4. Go to LaMulana2_Data\Managed, and drag Assembly-CSharp.dll onto MonoMod.exe
5. Rename Assembly-CSharp.dll to Assembly-CSharp.dll.backup
6. Rename MONOMODDED_Assembly-CSharp.dll to Assembly-CSharp.dll
7. Start the game

### Updating
1. Download the latest release
2. Extract in the folder where you previously installed the mod
3. Go to LaMulana2_Data\Managed, delete Assembly-CSharp.dll
4. Rename Assembly-CSharp.dll.backup to Assembly-CSharp.dll 
5. Drag Assembly-CSharp.dll onto MonoMod.exe
6. Rename Assembly-CSharp.dll to Assembly-CSharp.dll.backup
7. Rename MONOMODDED_Assembly-CSharp.dll to Assembly-CSharp.dll
8. Start the game

## How to build
To build the project, several things are needed:
1. Create a modified Assembly-CSharp.dll with [OpenAssemblyCreator](https://github.com/GregRos/Patchwork) from the Patchwork project to expose all fields publically
2. Place the modified dll in Lamulana2.Architect.mm/libs
3. From the latest release of the mod, extract everything but the Assembly-CSharp files into the aforementioned libs folder
4. Finally, copy UnityEngine.CoreModule.dll, UnityEngine.dll, UnityEngine.IMGUIModule.dll and all files starting with UnityEngine.TextRenderingModule from the game's Managed directory into the libs folder

These steps should be enough to ensure that the project builds, once it's done building, copy over the Assembly-CSharp.Architect.mm.dll file to the game's Managed folder.

