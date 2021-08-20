#!/bin/bash
cd "$(dirname "$0")"
dotnet publish -c Release

if test -d "/usr/local/tcbrute"; then
    rm -r /usr/local/tcbrute
fi

cp -r ./bin/Release/net5.0/publish /usr/local/tcbrute
echo "Executable files are copied to /usr/local/tcbrute."

if ! test -f "/usr/bin/tcbrute"; then
    ln -s /usr/local/tcbrute/TestcaseBruteforce /usr/bin/tcbrute
    echo "Created a symbolic link. (/usr/local/tcbrute/TestcaseBruteforce -> /usr/bin/tcbrute)"
fi