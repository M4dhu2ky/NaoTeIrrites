using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/// <summary>
/// LoginScreenAnimator - Não te Irrites
/// Sanctum Nova © 2025
/// 
/// Sequência de animação:
/// 1. Ecrã branco
/// 2. Logo cai do céu (bounce)
/// 3. Ícone de raiva aparece a pulsar
/// 4. Olhos pequenos aparecem
/// 5. Olhos crescem
/// 6. Olhos franzem (angry)
/// 7. Fade in dos botões
/// </summary>
public class LoginScreenAnimator : MonoBehaviour
{
    [Header("=== LOGO ===")]
    public RectTransform logoRect;
    public CanvasGroup logoCG;

    [Header("=== ÍCONE DE RAIVA ===")]
    public RectTransform iconRaivaRect;
    public CanvasGroup iconRaivaCG;

    [Header("=== OLHOS ===")]
    public RectTransform eyesRect;         // Parent dos 3 estados de olhos
    public Image eyesSmall;
    public Image eyesBig;
    public Image eyesAngry;

    [Header("=== BOTÕES ===")]
    public CanvasGroup btnInscrever;
    public CanvasGroup btnEntrar;
    public CanvasGroup btnConvidado;

    [Header("=== BOTÕES RECT (para animação de slide) ===")]
    public RectTransform btnInscreverRect;
    public RectTransform btnEntrarRect;
    public RectTransform btnConvidadoRect;

    [Header("=== TIMINGS ===")]
    [Tooltip("Delay inicial antes de começar a animação")]
    public float initialDelay = 0.3f;

    [Tooltip("Duração da queda do logo")]
    public float logoFallDuration = 0.7f;

    [Tooltip("Duração do pulsar do ícone")]
    public float iconPulseDuration = 0.35f;

    [Tooltip("Duração da transição dos olhos")]
    public float eyeTransitionDuration = 0.25f;

    [Tooltip("Duração do fade in dos botões")]
    public float buttonFadeDuration = 0.5f;

    // Posição original do logo (centro do ecrã)
    private Vector2 _logoFinalPos;
    // Posição fora do ecrã (acima)
    private float _logoOffscreenY = 1400f;

    void Awake()
    {
        // Garante que DOTween está inicializado
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(50, 10);
    }

    void Start()
    {
        // Guarda posição final do logo
        _logoFinalPos = logoRect.anchoredPosition;

        InitializeElements();
        StartCoroutine(PlayLoginSequence());
    }

    /// <summary>
    /// Coloca todos os elementos no estado inicial (invisíveis/fora do ecrã)
    /// </summary>
    void InitializeElements()
    {
        // Logo — fora do ecrã acima, invisível
        logoRect.anchoredPosition = new Vector2(_logoFinalPos.x, _logoOffscreenY);
        if (logoCG != null) logoCG.alpha = 1f;

        // Ícone de raiva — invisível, escala 0
        iconRaivaCG.alpha = 0f;
        iconRaivaRect.localScale = Vector3.zero;

        // Olhos — todos invisíveis
        SetImageAlpha(eyesSmall, 0f);
        SetImageAlpha(eyesBig, 0f);
        SetImageAlpha(eyesAngry, 0f);
        eyesRect.localScale = Vector3.one * 0.3f;

        // Botões — invisíveis e ligeiramente abaixo
        btnInscrever.alpha = 0f;
        btnEntrar.alpha = 0f;
        btnConvidado.alpha = 0f;
        btnInscrever.interactable = false;
        btnEntrar.interactable = false;
        btnConvidado.interactable = false;

        // Slide inicial dos botões para baixo
        btnInscreverRect.anchoredPosition += new Vector2(0, -30f);
        btnEntrarRect.anchoredPosition += new Vector2(0, -30f);
        btnConvidadoRect.anchoredPosition += new Vector2(0, -30f);
    }

    /// <summary>
    /// Sequência principal de animação
    /// </summary>
    IEnumerator PlayLoginSequence()
    {
        yield return new WaitForSeconds(initialDelay);

        // ─────────────────────────────────────────
        // PASSO 1 & 2 — Logo cai do céu com bounce
        // ─────────────────────────────────────────
        logoRect.DOAnchorPos(_logoFinalPos, logoFallDuration)
            .SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(logoFallDuration + 0.2f);

        // ─────────────────────────────────────────
        // PASSO 3 — Olhos pequenos aparecem
        // ─────────────────────────────────────────
        SetImageAlpha(eyesSmall, 1f);
        eyesRect.DOScale(1f, 0.3f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.4f);

        // ─────────────────────────────────────────
        // PASSO 4 — Olhos crescem
        // ─────────────────────────────────────────
        eyesSmall.DOFade(0f, eyeTransitionDuration);
        eyesBig.DOFade(1f, eyeTransitionDuration);
        eyesRect.DOScale(1.15f, eyeTransitionDuration).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(eyeTransitionDuration + 0.3f);

        // ─────────────────────────────────────────
        // PASSO 5 — Olhos franzem (raiva)
        // ─────────────────────────────────────────
        eyesBig.DOFade(0f, eyeTransitionDuration);
        eyesAngry.DOFade(1f, eyeTransitionDuration);
        eyesRect.DOShakeAnchorPos(0.4f, new Vector2(8f, 0f), 10, 90f);
        eyesRect.DOScale(1f, eyeTransitionDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(0.6f);

        // ─────────────────────────────────────────
        // PASSO 6 — Ícone de raiva aparece a pulsar
        // ─────────────────────────────────────────
        iconRaivaCG.DOFade(1f, 0.3f);
        iconRaivaRect.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.3f);

        // Pulsação: cresce e encolhe em loop (3 vezes)
        Sequence pulseSeq = DOTween.Sequence();
        pulseSeq.Append(iconRaivaRect.DOScale(1.4f, iconPulseDuration).SetEase(Ease.InOutSine));
        pulseSeq.Append(iconRaivaRect.DOScale(1.0f, iconPulseDuration).SetEase(Ease.InOutSine));
        pulseSeq.SetLoops(3);
        yield return pulseSeq.WaitForCompletion();

        // Fica visível (sem remover)
        yield return new WaitForSeconds(0.2f);

        // ─────────────────────────────────────────
        // PASSO 7 — Fade in dos botões (staggered)
        // ─────────────────────────────────────────
        // Inscrever
        btnInscreverRect.DOAnchorPos(
            btnInscreverRect.anchoredPosition + new Vector2(0, 30f),
            buttonFadeDuration).SetEase(Ease.OutCubic);
        btnInscrever.DOFade(1f, buttonFadeDuration);

        yield return new WaitForSeconds(0.12f);

        // Entrar
        btnEntrarRect.DOAnchorPos(
            btnEntrarRect.anchoredPosition + new Vector2(0, 30f),
            buttonFadeDuration).SetEase(Ease.OutCubic);
        btnEntrar.DOFade(1f, buttonFadeDuration);

        yield return new WaitForSeconds(0.12f);

        // Convidado
        btnConvidadoRect.DOAnchorPos(
            btnConvidadoRect.anchoredPosition + new Vector2(0, 30f),
            buttonFadeDuration).SetEase(Ease.OutCubic);
        btnConvidado.DOFade(1f, buttonFadeDuration);

        yield return new WaitForSeconds(buttonFadeDuration);

        // Ativa interatividade dos botões
        btnInscrever.interactable = true;
        btnEntrar.interactable = true;
        btnConvidado.interactable = true;

        Debug.Log("[LoginScreen] Animação completa — botões ativos.");
    }

    /// <summary>
    /// Helper para definir alpha de uma Image diretamente
    /// </summary>
    void SetImageAlpha(Image img, float alpha)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    void OnDestroy()
    {
        // Limpa todos os tweens ao destruir o objeto
        DOTween.KillAll();
    }
}