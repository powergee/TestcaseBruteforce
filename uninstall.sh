#!/bin/bash
if test -d "/usr/local/tcbrute"; then
    sudo rm -r /usr/local/tcbrute
fi

if test -f "/usr/bin/tcbrute"; then
    sudo rm /usr/bin/tcbrute
fi