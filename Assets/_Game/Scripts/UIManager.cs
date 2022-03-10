using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform m_GamePlayPanel;
    [SerializeField] private RectTransform m_PausedPanel;
    [SerializeField] private RectTransform m_GameOverPanel;

    [SerializeField] private TextMeshProUGUI m_GamePlayScoreText;
    [SerializeField] private TextMeshProUGUI m_GamePlayBestScoreText;

    [SerializeField] private TextMeshProUGUI m_ScoreText;
    [SerializeField] private TextMeshProUGUI m_BestScoreText;

    [SerializeField] private Button m_PauseButton;
    [SerializeField] private Button m_ResumeButton;
    [SerializeField] private Button m_RestartButton;
    [SerializeField] private Button m_MenuButton01;
    [SerializeField] private Button m_MenuButton02;

    private GameManager m_GameManager;

    private void Awake()
    {
        m_PauseButton.onClick.AddListener(() => { Pause(); });
        m_ResumeButton.onClick.AddListener(() => { Resume(); });
        m_RestartButton.onClick.AddListener(() => { Restart(); });
        m_MenuButton01.onClick.AddListener(() => { LoadMenu(); });
        m_MenuButton02.onClick.AddListener(() => { LoadMenu(); });

        m_GamePlayPanel.gameObject.SetActive(true);

        PanelHandaler(isPaused: false, isGameOver: false);
    }

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        if (m_GameManager == null) { Debug.LogError("GameManager is NULL"); }

        m_GameManager.OnGamePaused += OnGamePaused;
        m_GameManager.OnGameEnded += OnGameEnded;
    }

    private void OnGamePaused(bool status) => PanelHandaler(isPaused: status, isGameOver: false);

    private void OnGameEnded()
    {
        m_GamePlayPanel.gameObject.SetActive(true);
        PanelHandaler(isPaused: false, isGameOver: true);
    }
    private void PanelHandaler(bool isPaused, bool isGameOver)
    {
        m_PausedPanel.gameObject.SetActive(isPaused);
        m_GameOverPanel.gameObject.SetActive(isGameOver);
    }


    private void OnDestroy()
    {
        m_PauseButton.onClick.RemoveListener(() => { Pause(); });
        m_ResumeButton.onClick.RemoveListener(() => { Resume(); });
        m_RestartButton.onClick.RemoveListener(() => { Restart(); });
        m_MenuButton01.onClick.RemoveListener(() => { LoadMenu(); });
        m_MenuButton02.onClick.RemoveListener(() => { LoadMenu(); });

        m_GameManager.OnGamePaused -= OnGamePaused;
        m_GameManager.OnGameEnded -= OnGameEnded;
    }

    public void SetGamePlayScoreText(int score) => m_GamePlayScoreText.text = "Score: " + score;
    public void SetGamePlayBestScoreText(int best) => m_GamePlayBestScoreText.text = "Best: " + best;

    public void SetGameOverScoreText(int score) => m_ScoreText.text = "Score: " + score;
    public void SetGameOverBestScoreText(int best) => m_BestScoreText.text = "Best: " + best;

    private void Pause()
    {
        m_GameManager.DoPaused(true);
    }

    private void Resume()
    {
        m_GameManager.DoPaused(false);
        PanelHandaler(isPaused: false, isGameOver: false);
    }

    private void Restart() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }

    private void LoadMenu() { SceneManager.LoadScene(0); }
}
