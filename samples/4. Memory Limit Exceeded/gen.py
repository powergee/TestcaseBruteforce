#!/usr/bin/python3
import random as r

n = r.randint(1, 25) * 2
print(n)

print(" ".join(map(str, r.sample(range(1, 1000), n))))