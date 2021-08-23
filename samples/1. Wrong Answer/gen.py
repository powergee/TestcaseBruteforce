#!/usr/bin/python3
import random as r

print(1)
n = r.randint(5, 10)
k = r.randint(1, n)
print(n, k)

nums = set()
while len(nums) < n:
    nums.add(r.randint(1, 100))

print(" ".join(map(str, nums)))