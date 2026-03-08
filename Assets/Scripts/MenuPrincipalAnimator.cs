using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>
/// MenuPrincipalAnimator - Não te Irrites
/// Sanctum Nova © 2025
///
/// Sequência de animação de entrada:
/// 1. Fade in do fundo
/// 2. Logo desliza do topo
/// 3. BtnOffline aparece com bounce
/// 4. BotoesGrandes aparecem em stagger
/// 5. BotoesPequenos aparecem em stagger
/// </summary>
public class MenuPrincipalAnimator : MonoBehaviour
{
    [Header("=== FADE ===")]
    public Image fadePanel;
    public float fadeDuration = 0.4f;

    [Header("=== LOGO ===")]
    public RectTransform logoRect;
    public CanvasGroup logoCG;

    [Header("=== BOTÃO OFFLINE ===")]
    public RectTransform btnOfflineRect;
    public CanvasGroup btnOfflineCG;

    [Header("=== BOTÕES GRANDES ===")]
    public RectTransform btnOnlineRect;
    public CanvasGroup btnOnlineCG;
    public RectTransform btnEventsRect;
    public CanvasGroup btnEventsCG;

    [Header("=== BOTÕES PEQUENOS ===")]
    public RectTransform btnSettingsRect;
    public CanvasGroup btnSettingsCG;
    public RectTransform btnStoreRect;
    public CanvasGroup btnStoreCG;
    public RectTransform btnLeaderboardRect;
    public CanvasGroup btnLeaderboardCG;
    public RectTransform btnProfileRect;
    public CanvasGroup btnProfileCG;

    [Header("=== NAVEGAÇÃO ===")]
    public string gameSceneName = "GameScene";

    // Posições originais guardadas no Start
    private Vector2 _logoFinalPos;
    private Vector2 _btnOfflineFinalPos;
    private Vector2 _btnOnlineFinalPos;
    private Vector2 _btnEventsFinalPos;

    void Start()
    {
        // Guarda posições finais
        _logoFinalPos = logoRect.anchoredPosition;
        _btnOfflineFinalPos = btnOfflineRect.anchoredPosition;
        _btnOnlineFinalPos = btnOnlineRect.anchoredPosition;
        _btnEventsFinalPos = btnEventsRect.anchoredPosition;

        InitializeElements();
        StartCoroutine(PlayEntryAnimation());
    }

    void InitializeElements()
    {
        // Fade panel a preto
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = new Color(0, 0, 0, 1f);
        }

        // Logo acima do ecrã
        logoRect.anchoredPosition = new Vector2(_logoFinalPos.x, _logoFinalPos.y + 300f);
        if (logoCG != null) logoCG.alpha = 0f;

        // BtnOffline invisível e pequeno
        btnOfflineCG.alpha = 0f;
        btnOfflineRect.localScale = Vector3.one * 0.5f;

        // Botões grandes — invisíveis e deslocados
        btnOnlineCG.alpha = 0f;
        btnOnlineRect.anchoredPosition += new Vector2(-50f, 0f);
        btnEventsCG.alpha = 0f;
        btnEventsRect.anchoredPosition += new Vector2(50f, 0f);

        // Botões pequenos — invisíveis e abaixo
        SetCGAlpha(btnSettingsCG, 0f);
        SetCGAlpha(btnStoreCG, 0f);
        SetCGAlpha(btnLeaderboardCG, 0f);
        SetCGAlpha(btnProfileCG, 0f);

        btnSettingsRect.anchoredPosition += new Vector2(0f, -30f);
        btnStoreRect.anchoredPosition += new Vector2(0f, -30f);
        btnLeaderboardRect.anchoredPosition += new Vector2(0f, -30f);
        btnProfileRect.anchoredPosition += new Vector2(0f, -30f);

        // Desativa interatividade
        SetInteractable(false);
    }

    IEnumerator PlayEntryAnimation()
    {
        // ─── Fade in ───
        fadePanel.DOFade(0f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        fadePanel.gameObject.SetActive(false);

        // ─── Logo desliza do topo ───
        if (logoCG != null) logoCG.DOFade(1f, 0.3f);
        logoRect.DOAnchorPos(_logoFinalPos, 0.6f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.5f);

        // ─── BtnOffline com bounce ───
        btnOfflineCG.DOFade(1f, 0.3f);
        btnOfflineRect.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.4f);

        // ─── Botões grandes em stagger ───
        btnOnlineCG.DOFade(1f, 0.35f);
        btnOnlineRect.DOAnchorPos(_btnOnlineFinalPos, 0.35f).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(0.1f);

        btnEventsCG.DOFade(1f, 0.35f);
        btnEventsRect.DOAnchorPos(_btnEventsFinalPos, 0.35f).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(0.35f);

        // ─── Botões pequenos em stagger ───
        RectTransform[] smallRects = { btnSettingsRect, btnStoreRect, btnLeaderboardRect, btnProfileRect };
        CanvasGroup[] smallCGs = { btnSettingsCG, btnStoreCG, btnLeaderboardCG, btnProfileCG };

        for (int i = 0; i < smallRects.Length; i++)
        {
            int idx = i;
            smallCGs[idx].DOFade(1f, 0.3f);
            smallRects[idx].DOAnchorPos(
                smallRects[idx].anchoredPosition + new Vector2(0f, 30f), 0.3f)
                .SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(0.07f);
        }

        yield return new WaitForSeconds(0.3f);

        // Ativa interatividade
        SetInteractable(true);
        Debug.Log("[MenuPrincipal] Animação completa — botões ativos.");
    }

    // ─── Navegação ───────────────────────────────

    public void OnBtnOfflineClick()
    {
        LoadScene(gameSceneName);
    }

    public void OnBtnOnlineClick()
    {
        Debug.Log("[Menu] Online — em breve!");
        // LoadScene("OnlineScene");
    }

    public void OnBtnEventsClick()
    {
        Debug.Log("[Menu] Eventos — em breve!");
    }

    public void OnBtnSettingsClick()
    {
        Debug.Log("[Menu] Definições — em breve!");
    }

    public void OnBtnStoreClick()
    {
        Debug.Log("[Menu] Loja — em breve!");
    }

    public void OnBtnLeaderboardClick()
    {
        Debug.Log("[Menu] Leaderboard — em breve!");
    }

    public void OnBtnProfileClick()
    {
        Debug.Log("[Menu] Perfil — em breve!");
    }

    // ─── Helpers ─────────────────────────────────

    void LoadScene(string sceneName)
    {
        SetInteractable(false);
        fadePanel.gameObject.SetActive(true);
        fadePanel.DOFade(1f, fadeDuration).OnComplete(() =>
            SceneManager.LoadScene(sceneName));
    }

    void SetInteractable(bool value)
    {
        CanvasGroup[] all = {
            btnOfflineCG, btnOnlineCG, btnEventsCG,
            btnSettingsCG, btnStoreCG, btnLeaderboardCG, btnProfileCG
        };
        foreach (var cg in all)
            if (cg != null) cg.interactable = value;
    }

    void SetCGAlpha(CanvasGroup cg, float alpha)
    {
        if (cg != null) cg.alpha = alpha;
    }

    void OnDestroy()
    {
        DOTween.KillAll();
    }
}
