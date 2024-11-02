@echo off
REM Check if the correct number of arguments are provided
REM arguments:
REM  1 - version in semver (e.g: 1.0.0)
REM  2 - assembly path (e.g: J:\projects\my_mod\bin\Debug\com.tonebacas.white_knuckle_demo_custom_hand_sprite.dll)
REM  3 - project dir full path
REM  4 - assembly name (e.g: com.tonebacas.white_knuckle_demo_custom_hand_sprite)
REM  5 - release file name (e.g: custom-hand-sprite)

if "%~1"=="" (
    echo Error: Missing project dir path argument.
    exit /b 1
)
set "proj_dir_path=%~1"

if "%~2"=="" (
    echo Error: Missing assembly path.
    exit /b 1
)
set "mod_assembly_path=%~2"

if "%~3"=="" (
    echo Error: Missing version argument.
    exit /b 1
)
set "dist_version=%~3"

if "%~4"=="" (
    echo Error: Missing assembly name.
    exit /b 1
)
set "assembly_name=%~4"

if "%~5"=="" (
    echo Error: Missing release file name.
    exit /b 1
)
set "release_filename=%~5"

set "project_releases_path=%proj_dir_path%releases%"
set "distribution_path=%project_releases_path%\%dist_version%"
set "distribution_plugin_path=%distribution_path%\BepInEx\plugins\%assembly_name%"
set "distribution_compressed_file=%project_releases_path%\%dist_version%\%release_filename%-v%dist_version%.zip"

if exist "%distribution_compressed_file%" (
    echo ** Distribution already exists for version "%dist_version%". Did you mean to publish another version?
    echo ** Alternatively, manually delete the existing distribution and run the build target again. 
    exit /b 1
)

REM make the distribution plugin directory path
mkdir "%distribution_plugin_path%"

REM copy the mod assembly into distribution plugin path
xcopy /Y /I "%mod_assembly_path%" "%distribution_plugin_path%"

REM zip up the release
7z a -tzip "%distribution_compressed_file%" "%distribution_path%\*" 