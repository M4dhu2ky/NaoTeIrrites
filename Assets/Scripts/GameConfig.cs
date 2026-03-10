using UnityEngine;

/// <summary>
/// GameConfig - Não te Irrites
/// Sanctum Nova © 2025
///
/// Singleton persistente que guarda a configuração
/// selecionada no OfflineSetup e passa para a GameScene
/// </summary>
public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance { get; private set; }

    // ─── Configuração ─────────────────────────────
    public bool IsPvP { get; private set; }
    public int NumPlayers { get; private set; }
    public int NumCPUs { get; private set; }
    public bool LateraActive { get; private set; }

    // Total de participantes
    public int TotalParticipants => NumPlayers + NumCPUs;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetConfig(bool isPvP, int numPlayers, int numCPUs, bool lateraActive)
    {
        IsPvP = isPvP;
        NumPlayers = numPlayers;
        NumCPUs = numCPUs;
        LateraActive = lateraActive;

        Debug.Log($"[GameConfig] isPvP:{isPvP} | Players:{numPlayers} | CPUs:{numCPUs} | Latéra:{lateraActive}");
    }
}
