#include <iostream>
#include <unordered_map>
#include <algorithm>
#include <vector>

typedef long long Long;

bool isPrime[2000];
int n, nums[50];
std::unordered_map<Long, bool> dp;
std::vector<int> answers;

bool canMakeMatches(Long selected) {
    if (dp.find(selected) != dp.end()) {
        return dp[selected];
    }

    int first = -1;
    for (int i = 0; i < n; ++i) {
        if ((selected & (1LL<<i)) == 0) {
            first = i;
            break;
        }
    }

    if (first == -1) {
        return dp[selected] = true;
    }

    bool hasSolution = false;
    for (int i = first+1; i < n; ++i) {
        if ((selected & (1LL<<i)) == 0 && isPrime[nums[first]+nums[i]] && canMakeMatches(selected | (1LL<<first) | (1LL<<i))) {
            hasSolution = true;
            if (selected == 0) {
                answers.push_back(nums[i]);
            } else {
                break;
            }
        }
    }

    return dp[selected] = hasSolution;
}

int main() {
    const int primeCount = sizeof(isPrime)/sizeof(bool);
    std::fill(isPrime, isPrime+primeCount, true);
    isPrime[0] = isPrime[1] = false;
    for (int i = 2; i < primeCount; ++i) {
        if (isPrime[i]) {
            for (int j = i*i; j < primeCount; j += i) {
                isPrime[j] = false;
            }
        }
    }

    scanf("%d", &n);
    for (int i = 0; i < n; ++i) {
        scanf("%d", nums+i);
    }

    if (canMakeMatches(0)) {
        std::sort(answers.begin(), answers.end());
        for (int next : answers) {
            printf("%d ", next);
        }
    } else {
        printf("-1");
    }

    return 0;
}