#!/usr/bin/python3
import time
import random as r

result = sum(map(int, input().split()))

if r.randint(0, 4) == 0:
    time.sleep(2)

print(result)