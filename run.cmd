@echo off
dotnet build -p:Configuration=Release
echo:
bin\Release\net7.0\SD.exe %*
