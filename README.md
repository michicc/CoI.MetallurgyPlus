# Metallurgy+ mod for Captain of Industry

This is a mod for Captain of Industry (COI) that aims to improve the game by making metal smelting and production more varied and interesting.
Focus is on abstracting real-world processes and enriching gameplay by providing different production chains with their own trade-offs.

## How to compile

A normal Visual Studio solution and .csproj file are provided.

1. Before you can compile the mod, you need to provide the path of your COI installation directory. You can get this path from the Steam client via `Properties...` -> `Local Files` -> `Browse` (a typical install path might be `C:/Steam/steamapps/common/Captain of Industry`).
2. Make a copy of the file `Paths.user.example` in your clone and rename the copy to `Paths.user`.
3. Open `Paths.user` in a text editor and change the path to match your system.
4. You should now be able to build the solution.
