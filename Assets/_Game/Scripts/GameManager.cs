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
        if(m_Player == null) { Debug.LogError("Player is NULL"); }

        m_FoeSpawner = FindObjectOfType<FoeSpawner>();
        if (m_FoeSpawner == null) { Debug.LogError("FoeSpawner is NULL"); }

        DoStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_IsPaused = !m_IsPaused;
            DoPaused(m_IsPaused);
        }
    }

    private void DoStart()
    {
        //enable player movement
        StartCoroutine(m_Player.IE_ShootProjectiles());
        StartCoroutine(m_FoeSpawner.IE_SpawnFoes());
        // start spawning foes
        OnGameStarted?.Invoke();
    }


    private void DoPaused(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        OnGamePaused?.Invoke(isPaused);
    }

    public void DoGameOver()
    {
        m_Player.StopAllCoroutines();
        m_FoeSpawner.StopAllCoroutines();

        OnGameEnded?.Invoke();
    }


}
