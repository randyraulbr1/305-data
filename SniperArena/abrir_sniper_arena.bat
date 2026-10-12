@echo off
chcp 65001 >nul
set "PROJ=%USERPROFILE%\Desktop\SniperArena_Unity"
set "SRC=%~dp0"
set "UNITY="
for /d %%D in ("C:\Program Files\Unity\Hub\Editor\6*") do if exist "%%D\Editor\Unity.exe" set "UNITY=%%D\Editor\Unity.exe"
if "%UNITY%"=="" (echo No encontre Unity 6 en C:\Program Files\Unity\Hub\Editor. Instalalo desde Unity Hub. & pause & exit /b 1)
if not exist "%SRC%Scripts\Bootstrap.cs" (echo Falta la carpeta Scripts junto a este .bat & pause & exit /b 1)

if not exist "%PROJ%\Assets" (
  echo Creando proyecto en %PROJ% ...
  "%UNITY%" -createProject "%PROJ%" -batchmode -quit
)
xcopy "%SRC%Scripts" "%PROJ%\Assets\SniperArena\Scripts\" /E /I /Y >nul
xcopy "%SRC%Editor" "%PROJ%\Assets\SniperArena\Editor\" /E /I /Y >nul

echo Abriendo Unity...
start "" "%UNITY%" -projectPath "%PROJ%" -executeMethod SniperArenaSetup.Run
