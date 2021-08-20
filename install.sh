#!/bin/bash
cd "$(dirname "$0")"
dotnet publish -c Release

cp -r ./bin/Release/net5.0/publish /usr/local/tcbrute
echo "Executable files are copied to /usr/local/tcbrute."

ln -s /usr/local/tcbrute/TestcaseBruteforce /usr/bin/tcbrute
echo "Created a symbolic link. (/usr/local/tcbrute/TestcaseBruteforce -> /usr/bin/tcbrute)"