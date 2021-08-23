#include <iostream>

int main() {
    int a, b, c, d, e, f;
    std::cin >> a >> b >> c >> d >> e >> f;

    if (a == 0) {
        int y = c/b;
        printf("%d %d", (f-e*y)/d, y);
    } else if (b == 0) {
        int x = c/a;
        printf("%d %d", x, (f-d*x)/e);
    } else if (d == 0) {
        int y = f/e;
        printf("%d %d", (c-b*y)/a, y);
    } else if (e == 0) {
        int x = f/d;
        printf("%d %d", x, (c-a*x)/b);
    } else {
        printf("%d %d", (c*b*d-a*c*e-b*c*d+a*b*f)/(a*b*d-a*a*e), (c*d-a*f)/(b*d-a*e));
    }

    return 0;
}