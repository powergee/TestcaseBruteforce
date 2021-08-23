#include <iostream>
#include <numeric>

class Fraction {
public:
    int num, den;

    Fraction(int n, int d) {
        num = n;
        den = d;
        
        if (n == 0 || d == 0) {
            num = num ? 1 : 0;
            den = den ? 1 : 0;
        } else {
            int gcd = std::gcd(n, d);
            num /= gcd;
            den /= gcd;
        }
    }

    bool operator==(const Fraction& f) const {
        return num == f.num && den == f.den;
    }
};

int n;
char str[500002];
int dCount[500002];
int kCount[500002];

int main() {
    int T;
    scanf("%d", &T);

    while (T--) {
        scanf("%d", &n);
        scanf("%s", str+1);

        for (int i = 1; i <= n; ++i) {
            int k = str[i] == 'K' ? 1 : 0;
            int d = str[i] == 'D' ? 1 : 0;
            dCount[i] = dCount[i-1] + d;
            kCount[i] = kCount[i-1] + k;
        }

        for (int j = 1; j <= n; ++j) {
            Fraction ratio(dCount[j], kCount[j]);
            int length = ratio.num + ratio.den;
            int count = 0;
            if (length == 1) {
                count = j;
            } else {
                for (int i = length; i <= j; i += length) {
                    if (Fraction(dCount[i], kCount[i]) == ratio) {
                        ++count;
                    }
                }
            }
            printf("%d ", count);
        }
        printf("\n");
    }

    return 0;
}