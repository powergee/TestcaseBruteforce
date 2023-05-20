#!/bin/bash
if test -d "/usr/local/tcbrute"; then
    sudo rm -r /usr/local/tcbrute
fi

if test -f "/usr/local/bin/tcbrute"; then
    sudo rm /usr/local/bin/tcbrute
fi
