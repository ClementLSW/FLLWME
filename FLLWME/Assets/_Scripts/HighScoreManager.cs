using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using LootLocker;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    string playerID;
    string leaderboardKey = "GLB";
    int currentScore = 0;
    int currentHighScore;
    int count = 10;

    [SerializeField] private List<TMP_Text> leaderboardDisplay;
    [SerializeField ]private TMP_Text currentHighscoreDisplay;

    private static HighScoreManager instance;

    public HighScoreManager GetInstance() { return instance; }

    private void Start() {

        if (instance == null) {
            instance = this;
            LootLockerSDKManager.StartGuestSession((response) => {
                if (response.success) {
                    playerID = response.player_id.ToString();
                    Debug.Log("Success");
                }
                else {
                    Debug.Log("Failed");
                }
            });
        }
        else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void GetCurrentHighscore() {
        LootLockerSDKManager.GetMemberRank(leaderboardKey, playerID, (response) => {
            if (response.statusCode == 200) {
                Debug.Log("Successful");
                currentHighScore = response.score;
            }
            else {
                Debug.Log("failed: " + response.Error);
                return;
            }
        });

        currentHighscoreDisplay.text = "Hi-Score: " + currentHighScore.ToString();
    }

    private void SubmitHighScore() {
        LootLockerSDKManager.SubmitScore(playerID, currentScore, leaderboardKey, (response) =>
        {
            if (response.statusCode == 200) {
                Debug.Log("Successful");
            }
            else {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

    private void GetLeaderboard() {
        LootLockerSDKManager.GetScoreList(leaderboardKey, count, 0, (response) =>
        {
            if (response.statusCode == 200) {
                Debug.Log("Successful");
                int i = 0;
                foreach (LootLockerLeaderboardMember entry in response.items) {
                    leaderboardDisplay[i].text = entry.rank + entry.player.public_uid + entry.score;
                    Debug.Log(entry.rank + entry.player.public_uid + entry.score);
                }
            }
            else {
                Debug.Log("failed: " + response.Error);
            }
        });
    }
}
