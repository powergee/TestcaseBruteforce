#!/usr/bin/python3
import random as r
import math

def get_random_coeff():
    pos = r.randint(0, 1)
    if pos == 1:
        return r.randint(1, 10)
    else:
        return r.randint(-10, -1)

def generate_line(x, y):
    kind = r.randint(0, 6)
    if kind == 0:
        a = get_random_coeff()
        c = a*x
        return [a, 0, c]
    elif kind == 1:
        b = get_random_coeff()
        c = b*x
        return [0, b, c]
    else:
        a = get_random_coeff()
        b = get_random_coeff()
        c = a*x + b*y
        return [a, b, c]

def are_same(line1, line2):
    line1 = line1[:]
    line2 = line2[:]
    g1 = math.gcd(line1)
    g2 = math.gcd(line2)
    for i in range(0, 3):
        line1[i] //= g1
        line2[i] //= g2
    return line1 == line2

valid = False
lines = None
while not valid:
    x = r.randint(-10, 10)
    y = r.randint(-10, 10)
    lines = [generate_line(x, y), generate_line(x, y)]
    if lines[0] != lines[1]:
        valid = True

print(" ".join(map(str, lines[0])), " ".join(map(str, lines[1])))