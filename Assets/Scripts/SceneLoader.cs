using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// SceneLoader - Não te Irrites
/// Sanctum Nova © 2025
/// Gere a navegação entre cenas com fade de transição
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Header("=== TRANSIÇÃO ===")]
    [Tooltip("Painel preto que cobre o ecrã durante a transição")]
    public Image fadePanel;

    [Tooltip("Duração do fade em segundos")]
    public float fadeDuration = 0.4f;

    private static SceneLoader _instance;

    void Awake()
    {
        // Singleton — persiste entre cenas
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Fade in ao entrar na cena
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = new Color(0, 0, 0, 1f);
            fadePanel.DOFade(0f, fadeDuration).OnComplete(() =>
                fadePanel.gameObject.SetActive(false));
        }
    }

    /// <summary>
    /// Carrega uma cena pelo nome com fade de transição
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.DOFade(1f, fadeDuration).OnComplete(() =>
                SceneManager.LoadScene(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    // ─── Métodos específicos para os botões ───

    public void GoToMenu()
    {
        LoadScene("MenuPrincipal");
    }

    public void GoToLogin()
    {
        LoadScene("LoginScreen");
    }

    public void GoToGame()
    {
        LoadScene("GameScene");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
