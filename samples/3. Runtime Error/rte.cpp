#include <iostream>

int main() {
    int a, b, c, d, e, f;
    std::cin >> a >> b >> c >> d >> e >> f;
    printf("%d %d", (c*b*d-a*c*e-b*c*d+a*b*f)/(a*b*d-a*a*e), (c*d-a*f)/(b*d-a*e));

    return 0;
}