#!/usr/bin/python3
import random as r

print(1)
n = r.randint(400000, 500000)
print(n)

s = ""
for _ in range(0, n):
    s += ("D" if r.randint(0, 1) == 0 else "K")
print(s)