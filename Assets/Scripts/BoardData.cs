using UnityEngine;

/// <summary>
/// BoardData - Não te Irrites
/// Sanctum Nova © 2025
/// Define todas as posições das casas do tabuleiro
/// </summary>
public class BoardData : MonoBehaviour
{
    [Header("=== PISTA EXTERNA ===")]
    [Tooltip("92 casas da pista externa, sentido anti-horário")]
    public Vector2[] pistaExterna = new Vector2[92];

    [Header("=== PISTAS INTERNAS ===")]
    [Tooltip("10 casas + centro da pista interna do Cinzento (canto sup. esq.)")]
    public Vector2[] pistaInterna_Cinzento = new Vector2[11];

    [Tooltip("10 casas + centro da pista interna do Azul (canto sup. dir.)")]
    public Vector2[] pistaInterna_Azul = new Vector2[11];

    [Tooltip("10 casas + centro da pista interna do Verde (canto inf. esq.)")]
    public Vector2[] pistaInterna_Verde = new Vector2[11];

    [Tooltip("10 casas + centro da pista interna do Vermelho (canto inf. dir.)")]
    public Vector2[] pistaInterna_Vermelho = new Vector2[11];

    [Header("=== BASES ===")]
    [Tooltip("3 posições da base do Cinzento")]
    public Vector2[] base_Cinzento = new Vector2[3];

    [Tooltip("3 posições da base do Azul")]
    public Vector2[] base_Azul = new Vector2[3];

    [Tooltip("3 posições da base do Verde")]
    public Vector2[] base_Verde = new Vector2[3];

    [Tooltip("3 posições da base do Vermelho")]
    public Vector2[] base_Vermelho = new Vector2[3];

    [Header("=== CENTRO ===")]
    public Vector2 centro;

    [Header("=== GIZMOS ===")]
    public bool mostrarPistaExterna = true;
    public bool mostrarPistasInternas = true;
    public bool mostrarBases = true;
    public bool mostrarCentro = true;
    public float tamanhoGizmo = 0.05f;

    // ---------------------------------------------------------------
    // Índices de entrada na pista externa por cor
    // ---------------------------------------------------------------
    public static readonly int EntradaVermelho  = 0;
    public static readonly int EntradaAzul      = 23;
    public static readonly int EntradaCinzento  = 46;
    public static readonly int EntradaVerde     = 69;

    // ---------------------------------------------------------------
    // Índices de chegada na pista interna por cor
    // ---------------------------------------------------------------
    public static readonly int ChegadaVermelho  = 86;
    public static readonly int ChegadaAzul      = 17;
    public static readonly int ChegadaCinzento  = 40;
    public static readonly int ChegadaVerde     = 63;

    // ---------------------------------------------------------------
    // Casas seguras (Fichas) — índices na pista externa
    // ---------------------------------------------------------------
    public static readonly int[] CasasSeguras = new int[]
    {
        0, 11, 17, 23, 34, 40, 46, 57, 63, 69, 80, 86
    };

    // ---------------------------------------------------------------
    // GIZMOS — visualização no editor
    // ---------------------------------------------------------------

    private void OnDrawGizmos()
    {
        if (mostrarPistaExterna)
            DesenharArray(pistaExterna, Color.yellow, "E");

        if (mostrarPistasInternas)
        {
            DesenharArray(pistaInterna_Cinzento, Color.gray, "C");
            DesenharArray(pistaInterna_Azul, Color.blue, "A");
            DesenharArray(pistaInterna_Verde, Color.green, "V");
            DesenharArray(pistaInterna_Vermelho, Color.red, "R");
        }

        if (mostrarBases)
        {
            DesenharArray(base_Cinzento, Color.gray, "BC");
            DesenharArray(base_Azul, Color.blue, "BA");
            DesenharArray(base_Verde, Color.green, "BV");
            DesenharArray(base_Vermelho, Color.red, "BR");
        }

        if (mostrarCentro)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(new Vector3(centro.x, centro.y, 0), tamanhoGizmo * 2);
        }
    }

    private void DesenharArray(Vector2[] array, Color cor, string prefixo)
    {
        if (array == null) return;
        Gizmos.color = cor;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == Vector2.zero) continue;
            Vector3 pos = new Vector3(array[i].x, array[i].y, 0);
            Gizmos.DrawSphere(pos, tamanhoGizmo);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(pos + Vector3.up * tamanhoGizmo * 1.5f,
                $"{prefixo}{i}");
#endif
        }
    }
}
