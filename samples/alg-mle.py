#!/usr/bin/python3
import random as r

result = sum(map(int, input().split()))

dummy = []
if r.randint(0, 4) == 0:
    for i in range(0, 50000000):
        dummy.append(i)

print(result)