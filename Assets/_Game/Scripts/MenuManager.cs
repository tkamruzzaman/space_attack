using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button m_PlayButton;
    [SerializeField] private Button m_ExitButton;

    private void Awake()
    {
        m_PlayButton.onClick.AddListener(LoadGamePlay);
        m_ExitButton.onClick.AddListener(Exit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }

    private void OnDestroy()
    {
        m_PlayButton.onClick.RemoveListener(LoadGamePlay);
        m_ExitButton.onClick.RemoveListener(Exit);
    }

    private void LoadGamePlay()
    {
        SceneManager.LoadScene(1);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
