#!/bin/bash
cd "$(dirname "$0")"
dotnet publish -c Release

if test -d "/usr/local/tcbrute"; then
    sudo rm -r /usr/local/tcbrute
fi

sudo cp -r ./bin/Release/net6.0/publish /usr/local/tcbrute
echo "Executable files are copied to /usr/local/tcbrute."

if ! test -f "/usr/local/bin/tcbrute"; then
    sudo ln -s /usr/local/tcbrute/TestcaseBruteforce /usr/local/bin/tcbrute
    echo "Created a symbolic link. (/usr/local/tcbrute/TestcaseBruteforce -> /usr/local/bin/tcbrute)"
fi
