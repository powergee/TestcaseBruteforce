#include <iostream>
#include <algorithm>
#include <map>
#include <vector>

int arr[100000];
int sorted[100000];

int main() {
    int T;
    scanf("%d", &T);

    while (T--) {
        int n, k;
        scanf("%d %d", &n, &k);

        for (int i = 0; i < n; ++i) {
            scanf("%d", arr+i);
            sorted[i] = arr[i];
        }

        std::sort(sorted, sorted+n);
        std::vector<int> segment;
        int need = 1;
        for (int i = 0; i < n; ++i) {
            if (segment.empty() || arr[i] == segment.back()) {
                segment.push_back(arr[i]);
            } else {
                auto found = std::upper_bound(sorted, sorted+n, segment.back());
                if (found != sorted+n && *found == arr[i]) {
                    segment.push_back(arr[i]);
                } else {
                    need++;
                    segment.clear();
                }
            }
        }

        printf(need <= k ? "Yes\n" : "No\n");
    }

    return 0;
}