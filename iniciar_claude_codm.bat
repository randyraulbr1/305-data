@echo off
chcp 65001 >nul
cd /d "C:\Users\Randy\Desktop\BattleRoyale_CODM_Multiplayer" || (echo No se encontro la carpeta del proyecto & pause & exit /b 1)
where claude >nul 2>nul || (echo Claude Code no esta instalado. Instala con: npm install -g @anthropic-ai/claude-code & pause & exit /b 1)
if not exist "%~dp0prompt_codm.txt" (echo Falta prompt_codm.txt junto a este .bat & pause & exit /b 1)
powershell -NoProfile -Command "claude (Get-Content -Raw -Encoding UTF8 '%~dp0prompt_codm.txt')"
pause
