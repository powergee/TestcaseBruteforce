#include <iostream>
#include <unordered_map>
#include <algorithm>
#include <vector>
#define LEFT_MAX 50
#define RIGHT_MAX 50

bool isPrime[2001];
int n, nums[50];
std::vector<int> odd, even;
std::vector<int> connected[LEFT_MAX];
bool visited[LEFT_MAX];
int matching[RIGHT_MAX];

bool findValidPair(int start) {
    if (visited[start]) {
        return false;
    }
    visited[start] = true;

    for (int opposite : connected[start]) {
        if (matching[opposite] == -1 || (matching[opposite] != 0 && findValidPair(matching[opposite]))) {
            matching[opposite] = start;
            return true;
        }
    }
    return false;
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
        if (nums[i] % 2 == 0) {
            even.push_back(nums[i]);
        } else {
            odd.push_back(nums[i]);
        }
    }

    std::vector<int>* left = (nums[0] % 2 == 0 ? &even : &odd);
    std::vector<int>* right = (nums[0] % 2 == 0 ? &odd : &even);

    for (int i = 0; i < left->size(); ++i) {
        for (int j = 0; j < right->size(); ++j) {
            if (isPrime[left->at(i) + right->at(j)]) {
                connected[i].push_back(j);
            }
        }
    }

    std::vector<int> answer;
    for (int i = 0; i < right->size(); ++i) {
        if (!isPrime[left->at(0) + right->at(i)]) {
            continue;
        }

        std::fill(matching, matching + right->size(), -1);
        matching[i] = 0;
        int matched = 1;

        for (int j = 1; j < left->size(); ++j) {
            std::fill(visited, visited + left->size(), false);
            matched += (findValidPair(j) ? 1 : 0);
        }

        if (matched == n/2) {
            answer.push_back(right->at(i));
        }
    }

    std::sort(answer.begin(), answer.end());
    if (answer.empty()) {
        printf("-1");
    } else {
        for (int ans : answer) {
            printf("%d ", ans);
        }
    }

    return 0;
}