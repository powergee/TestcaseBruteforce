#include <iostream>
#include <string>
#include <vector>

class Word {
private:
    std::string plain;
    int head, tail;

    int getHash(char ch) {
        return ('a' <= ch && ch <= 'z') ? ch - 'a' : ch - 'A' + 26;
    }

    int getHash(std::string seg) {
        int e = 1;
        int result = 0;
        for (char ch : seg) {
            result += getHash(ch) * e;
            e *= 52;
        }
        return result;
    }

public:
    Word(std::string s) {
        plain = s;
        head = getHash(s.substr(0, 3));
        tail = getHash(s.substr(s.size()-3));
    }

    int getHead() {
        return head;
    }

    int getTail() {
        return tail;
    }
};

enum Predicted {
    Undefined, Judging, Win, Lose, Draw
};

int n;
std::vector<Word> words;
std::vector<int> heads[52*52*52];
std::vector<int> tails[52*52*52];
std::vector<Predicted> dp;

Predicted predictResult(int before) {
    if (dp[before] == Judging) {
        return Draw;
    } else if (dp[before] != Undefined) {
        return dp[before];
    }
    dp[before] = Judging;

    Word& w = words[before];
    std::vector<int>& speakable = heads[w.getTail()];
    
    bool oppositeDraw = false, oppositeLose = false;
    for (int spIdx : speakable) {
        Predicted opposite = predictResult(spIdx);
        if (opposite == Draw) {
            oppositeDraw = true;
        } else if (opposite == Lose) {
            oppositeLose = true;
        }
    }

    if (oppositeDraw) {
        return dp[before] = Draw;
    } else if (oppositeLose) {
        return dp[before] = Win;
    } else {
        return dp[before] = Lose;
    }
}

int main() {
    std::ios_base::sync_with_stdio(false);
    std::cin.tie(nullptr);
    std::cin >> n;

    dp.resize(n, Undefined);

    for (int i = 0; i < n; ++i) {
        std::string word;
        std::cin >> word;
        words.emplace_back(word);
    }

    for (int i = 0; i < n; ++i) {
        heads[words[i].getHead()].push_back(i);
        tails[words[i].getTail()].push_back(i);
    }

    for (int i = 0; i < n; ++i) {
        Predicted pred = predictResult(i);
        if (pred == Win) {
            printf("Aoki\n");
        } else if (pred == Lose) {
            printf("Takahashi\n");
        } else {
            printf("Draw\n");
        }
    }

    return 0;
}