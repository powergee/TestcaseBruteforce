#include <iostream>
#include <numeric>
#include <map>

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

struct CompareFraction {
    bool operator() (const Fraction& f1, const Fraction& f2) const {
        if (f1.num == f2.num) {
            return f1.den < f2.den;
        }
        return f1.num < f2.num;
    }
};

int n;
char str[500002];
int dCount[500002];
int kCount[500002];
int answer[500002];

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

        std::map<Fraction, int, CompareFraction> counter;
        for (int j = 1; j <= n; ++j) {
            Fraction ratio(dCount[j], kCount[j]);
            if (counter.find(ratio) == counter.end()) {
                printf("1 ");
            } else {
                printf("%d ", counter[ratio] + 1);
            }
            counter[ratio]++;
        }
        printf("\n");
    }

    return 0;
}