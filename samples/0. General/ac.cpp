#include<bits/stdc++.h>
using namespace std;
int M = 52 * 52 * 52;
int char_to_int(char c){
    if('A'<=c && c<='Z') return c-'A';
    else return c-'a'+26;
}
int string_to_int(char a,char b,char c){
    return char_to_int(a) * 52 * 52 + char_to_int(b) * 52 + char_to_int(c);
}

int main(){
    int N; cin >> N;
    vector<pair<int,int>> edge(N);
    vector<vector<int>> revGraph(M);
    vector<int> cnt(M);
    for(int i = 0; i < N; i++){
        string s; cin >> s;
        edge[i] = make_pair(string_to_int(s[0], s[1], s[2]), string_to_int(s[s.size()-3], s[s.size()-2], s[s.size()-1]));
        cnt[edge[i].first]++;
        revGraph[edge[i].second].push_back(edge[i].first);
    }
    vector<int> ans(M, -1);
    queue<int> que;
    for(int i = 0; i < M; i++) if(cnt[i] == 0){
        ans[i] = 0;
        que.push(i);
    }
    while(!que.empty()) {
        int t = que.front(); que.pop();
        for(int x : revGraph[t]) if(ans[x] == -1) {
            cnt[x]--;
            if(ans[t] == 0){
                ans[x] = 1; que.push(x);
            }
            else if(cnt[x] == 0){
                ans[x] = 0; que.push(x);
            }
        }
    }
    for(int i = 0; i < N; i++) {
        if(ans[edge[i].second] == -1) cout << "Draw" << endl;
        if(ans[edge[i].second] == 0) cout << "Takahashi" << endl;
        if(ans[edge[i].second] == 1) cout << "Aoki" << endl;
    }
}
