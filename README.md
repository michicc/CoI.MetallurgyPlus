# Metallurgy+ mod for Captain of Industry

This is a mod for Captain of Industry (COI) that aims to improve the game by making metal smelting and production more varied and interesting.
Focus is on abstracting real-world processes and enriching gameplay by providing different production chains with their own trade-offs.

## How to compile

A normal Visual Studio solution and .csproj file are provided.

1. Before you can compile the mod, you need to provide the path of your COI installation directory. You can get this path from the Steam client via `Properties...` -> `Local Files` -> `Browse` (a typical install path might be `C:\Program Files (x86)\Steam\steamapps\common\Captain of Industry`).
2. Make a copy of the file `Options.user.example` in your clone and rename the copy to `Options.user`.
3. Open `Options.user` in a text editor and change the path to match your system.
4. You should now be able to build the solution.

In `Options.user` you can also configure some more build options.
If `CopyToModDirectory` is set to the default of `true`, the build output will be automatically copied to the COI mod directory in app data.
If `BuildZipOnRelease` is set to the default of `true`, a `Release` build will be automatically bundled into a zip-file for distribution.

## How to install

If you have downloaded a pre-built release from GitHub, install it as follows:

1. Locate your COI data folder. By default it is at: `%APPDATA%/Captain of Industry`

2. Open or create the "Mods" directory inside it.

3. Extract the contents of this zip into the "Mods" directory.
   You should end up with a folder structure like:
   ```
   Mods/MetallurgyPlus/manifest.json
   Mods/MetallurgyPlus/CoI.Metallurgy+.dll
   ...
   ```

   The folder name must match the mod's ID (`MetallurgyPlus`).

4. Launch the game and select the mod when creating a new game, or add it to an existing save if mods supports it.

5. In case of any errors, examine logs in: `%APPDATA%/Captain of Industry/Logs`
