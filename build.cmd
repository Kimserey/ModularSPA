@echo off
cls

set instance=%1
setlocal
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\F#\4.0\Framework\v4.0
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\F#\3.1\Framework\v4.0
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\F#\3.0\Framework\v4.0
set PATH=%PATH%;%ProgramFiles%\Microsoft SDKs\F#\4.0\Framework\v4.0
set PATH=%PATH%;%ProgramFiles%\Microsoft SDKs\F#\3.1\Framework\v4.0
set PATH=%PATH%;%ProgramFiles%\Microsoft SDKs\F#\3.0\Framework\v4.0

echo Building for instance %instance%
echo ===========================

::-----------------------------------------------
::			Build projects
::------------------------------------------------

"C:\Program Files (x86)\MSBuild\14.0\bin\MSBuild.exe" /maxcpucount /verbosity:minimal /p:configuration=debug ModularSPA.sln

if errorlevel 1 (
  exit /b %errorlevel%
)

::------------------------------------------------
::			Bundle to Content/
::------------------------------------------------

fsi.exe --exec bundle.fsx %instance%
 
 if errorlevel 1 (
  exit /b %errorlevel%
)