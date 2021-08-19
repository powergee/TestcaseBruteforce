#!/usr/bin/python3
import random as r

result = sum(map(int, input().split()))

if r.randint(0, 4) == 0:
    dummy = 1 / 0

print(result)