using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action OnGameStarted = delegate { };
    public Action OnGameEnded = delegate { };
    public Action<bool> OnGamePaused = delegate { };

    private bool m_IsPaused;

    private Player m_Player;
    private FoeSpawner m_FoeSpawner;

    private void Start()
    {
        m_Player = FindObjectOfType<Player>();
        if (m_Player == null) { Debug.LogError("Player is NULL"); }

        m_FoeSpawner = FindObjectOfType<FoeSpawner>();
        if (m_FoeSpawner == null) { Debug.LogError("FoeSpawner is NULL"); }

        DoStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DoPaused(m_IsPaused);
        }
    }

    private void DoStart()
    {
        //enable player movement
        StartCoroutine(m_Player.IE_ShootProjectiles());
        // start spawning foes
        StartCoroutine(m_FoeSpawner.IE_SpawnFoes());

        OnGameStarted?.Invoke();
    }


    public void DoPaused(bool isPaused)
    {
        m_IsPaused = !m_IsPaused;

        Time.timeScale = m_IsPaused ? 0 : 1;

        OnGamePaused?.Invoke(m_IsPaused);
    }

    public void DoGameOver()
    {
        OnGameEnded?.Invoke();
    }


}
