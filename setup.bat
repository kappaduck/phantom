@echo off
setlocal

:: Cleanup build
for /r ".\artifacts" %%f in (*.nupkg) do (
    del /q /f "%%f"
)

:: Build and Pack Boo
set setupPath=.\src\Boo\Boo.csproj

dotnet build %setupPath% --configuration release
dotnet pack %setupPath% --configuration release

:: Find the .nupkg file
set boo=release\Boo.*.nupkg

set "nupkg="

for /r %CD% %%f in (%boo%) do (
    set nupkg=%%f
    set nupkgFolder=%%~dpf
    goto :install
)

echo "Could not find the .nupkg file"
exit /b 1

:: Extract the version number from the .nupkg file
:install

for %%A in (%nupkg%) do set filename=%%~nxA

for /f "tokens=3 delims=." %%A in ("%filename%") do set version=%%A

set version=%filename:Boo.=%
set version=%version:.nupkg=%

dotnet tool install --local --add-source %nupkgFolder% Boo --version %version%

exit /b %ERRORLEVEL%
