using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager m_Instance;
    public static ScoreManager Instance { get => m_Instance; private set => m_Instance = value; }
    private const string KEY_BEST_SCORE = "key_best_score_";
    private int m_Score;
    private int m_BestScore { get => PlayerPrefs.GetInt(KEY_BEST_SCORE, 0); set => PlayerPrefs.SetInt(KEY_BEST_SCORE, value); }

    public Action<int> OnScoreChange = delegate { };
    public Action<int> OnBestScoreChange = delegate { };

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public void AddScore(int score)
    {
        m_Score += score;
        OnScoreChange?.Invoke(m_Score);
        CheckBestScore(m_Score);
    }

    private void CheckBestScore(int score)
    {
        if (score >= m_BestScore)
        {
            m_BestScore = score;
            OnBestScoreChange?.Invoke(m_BestScore);
        }
    }

    public int GetScore() => m_Score;

    public int GetBestScore() => m_BestScore;

}
