#!/usr/bin/python3
import random as r

def generate_random_string(length):
    result = ""
    for _ in range(0, length):
        result += chr(ord('a') + r.randint(0, 2))
    return result

n = r.randint(4, 6)
print(n)

for _ in range(0, n):
    word_len = r.randint(3, 8)
    print(generate_random_string(word_len))