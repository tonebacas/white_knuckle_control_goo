# White Knuckle demo: Control Goo mod

This a mod for White Knuckle demo, made using [BepInEx5](https://github.com/BepInEx/BepInEx).

## Features

This mod lets you control the speed of the goo that chases you in your ascent through the game world.
See the [Usage section](#usage)  to learn how to install and use the mod.

## **Usage requirements

To use this mod, you must have **BepInEx5 x64** installed in the game (currently, latest version
is https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.2/BepInEx_win_x64_5.4.23.2.zip)

Also, you have to configure BepInEx5 and set `HideManagerGameObject` to `true` in
the configuration file at `<game_path>\BepInEx\config\BepInEx.cfg`.

If you've installed BepInEx5 x64 but don’t see the configuration file, start the game once and then exit; the file will
be created at that location.

---

## Releases

If you just want to install the mod, use a pre-built binary.

[Download from Releases.](https://github.com/tonebacas/white_knuckle_control_goo/releases/latest)

Check [Usage section](#usage) for usage instructions.

---

## Building

This C# project was created using BepInEx5's dotnet template (see the plugin development tutorial
at https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/index.html), with target framework (TFM) `netstandard2.1`
and Unity version `2022.3.32`. This translates into the command
`dotnet new bepinex5plugin -n <plugin_name> -T netstandard2.1 -U 2022.3.32`

Requires .NET SDK 2.1 x64 (download from Microsoft: https://dotnet.microsoft.com/en-us/download/dotnet/2.1)

When cloning this repository, you will be missing some files which are generated automatically by dotnet. Simply run
`dotnet restore .` in the solution directory to generate those files.

Use `dotnet build` inside the project's root directory to build the project; the built binary will be under the bin
folder. This command builds the binary, and also performs the same action as `dotnet restore .` prior to building.

Use `dotnet clean` to clean the solution of its `bin` and `obj` directories.

Once you have a successful build, to make a compressed file for distribution, use the custom `Distribute` build target
by running `dotnet msbuild -t:distribute`. This build target uses the `Version` project property in
`custom-hand-sprite.csproj` to create the distribution archive. As you make changes to the project, update that
property.

Alternative, you can use your favorite IDE to open the project's `.csproj` file.

**Required game files**: this project uses the game's `<game_path>\White Knuckle_Data\Managed\Assembly-CSharp.dll` and
`<game_path>\White Knuckle_Data\Managed\ALINE.dll` files,
which cannot be
distributed in this repository; the project has a pre-build target which copies the necessary files into the project's
`lib`
directory automatically when you run the `dotnet build` command.
This can save you the hassle of manually copying that file into the project. Also, in case the game receives an update,
this makes sure you're using the latest game file in your build.

For faster development and testing iteration, this project also has a post-build target that installs the mod by copying
the mod's `.dll` file into the appropriate folder, along with the asset files. **Note**: the post-build target runs
`xcopy` with a destination path which is only relevant
to my machine; **change it to point to the game path in your machine**.

---

## Usage

### Installation

Extract the release file into the game directory.

### Uninstallation

Simply delete the mod's directory `<game_path>\BepInEx\plugins\com.tonebacas.white_knuckle_demo_control_goo`.

### Configuration

After installing the mod and running the game once, the configuration file
`<game_path>\BepInEx\config\com.tonebacas.white_knuckle_demo_control_goo.cfg` will be created, which you can edit in a
text editor to configure the mod's options.
You can enable and disable the mod, and change the goo speed which is only applies if you enable the mod.

You can also use [BepInEx.ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager) (not the IL2CPP
version) to configure these settings ingame; after installing that plugin, press F1 (default key) to bring up the
configuration menu. You can change this key by changing its plugin configuration. See that project's readme for more
information.