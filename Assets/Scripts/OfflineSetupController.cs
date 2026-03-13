using UnityEngine;
using UnityEngine.UI;

public class OfflineSetupController : MonoBehaviour
{
    [Header("Botões de Modo")]
    [SerializeField] private Button btn_PlayerVsPlayer;
    [SerializeField] private Button btn_PlayerVsCPU;

    [Header("Sections")]
    [SerializeField] private GameObject section_PvP;
    [SerializeField] private GameObject section_CPU;

    [Header("Toggles PvP")]
    [SerializeField] private Toggle toggle_2P;
    [SerializeField] private Toggle toggle_3P;
    [SerializeField] private Toggle toggle_4P;

    [Header("Toggles CPU - Players")]
    [SerializeField] private Toggle toggle_CPU_1P;
    [SerializeField] private Toggle toggle_CPU_2P;
    [SerializeField] private Toggle toggle_CPU_3P;

    [Header("Toggles CPU - CPUs")]
    [SerializeField] private Toggle toggle_1C;
    [SerializeField] private Toggle toggle_2C;
    [SerializeField] private Toggle toggle_3C;

    [Header("Latera")]
    [SerializeField] private Toggle toggle_Latera;

    [Header("GO")]
    [SerializeField] private Button btn_GO;

    // Estado interno
    private enum Modo { Nenhum, PvP, CPU }
    private Modo modoAtual = Modo.Nenhum;
    private int playersSeleccionados = 0;
    private int cpusSeleccionados = 0;

    // ---------------------------------------------------------------

    private void Start()
    {

        //Debug Start
        Debug.Log("[OfflineSetupController] Start() chamado!");
        //Debug End

        // Reset visual dos Toggles
        ResetSection_PvP();
        ResetSection_CPU();

        // Sections visíveis mas não clicáveis
        SetSectionInteractable(section_PvP, false);
        SetSectionInteractable(section_CPU, false);
                
        // GO não disponível
        btn_GO.interactable = false;

        // Listeners dos botões de modo
        btn_PlayerVsPlayer.onClick.AddListener(() => SelecionarModo(Modo.PvP));
        btn_PlayerVsCPU.onClick.AddListener(() => SelecionarModo(Modo.CPU));

        // Listeners Toggles PvP
        toggle_2P.onValueChanged.AddListener(_ => TogglePvPAlterado(2));
        toggle_3P.onValueChanged.AddListener(_ => TogglePvPAlterado(3));
        toggle_4P.onValueChanged.AddListener(_ => TogglePvPAlterado(4));

        // Listeners Toggles CPU Players
        toggle_CPU_1P.onValueChanged.AddListener(_ => ToggleCPUPlayersAlterado(1));
        toggle_CPU_2P.onValueChanged.AddListener(_ => ToggleCPUPlayersAlterado(2));
        toggle_CPU_3P.onValueChanged.AddListener(_ => ToggleCPUPlayersAlterado(3));

        // Listeners Toggles CPUs
        toggle_1C.onValueChanged.AddListener(isOn => { if (isOn) cpusSeleccionados = 1; VerificarGO(); });
        toggle_2C.onValueChanged.AddListener(isOn => { if (isOn) cpusSeleccionados = 2; VerificarGO(); });
        toggle_3C.onValueChanged.AddListener(isOn => { if (isOn) cpusSeleccionados = 3; VerificarGO(); });

        // Listener GO
        btn_GO.onClick.AddListener(IniciarJogo);
    }

    // ---------------------------------------------------------------
    // SELEÇÃO DE MODO
    // ---------------------------------------------------------------

    private void SelecionarModo(Modo modo)
    {
        Debug.Log($"[SelecionarModo] Modo selecionado: {modo}");

        modoAtual = modo;

        // Reset completo de ambas as sections
        ResetSection_PvP();
        ResetSection_CPU();

        // Ativar apenas a section do modo escolhido
        SetSectionInteractable(section_PvP, modo == Modo.PvP);
        SetSectionInteractable(section_CPU, modo == Modo.CPU);

        VerificarGO();
    }

    // ---------------------------------------------------------------
    // TOGGLES PVP
    // ---------------------------------------------------------------

    private void TogglePvPAlterado(int players)
    {
        // Comportamento de seleção única (radio)
        toggle_2P.SetIsOnWithoutNotify(players == 2 && toggle_2P.isOn);
        toggle_3P.SetIsOnWithoutNotify(players == 3 && toggle_3P.isOn);
        toggle_4P.SetIsOnWithoutNotify(players == 4 && toggle_4P.isOn);

        playersSeleccionados = ObterToggleAtivoPvP();
        VerificarGO();
    }

    private int ObterToggleAtivoPvP()
    {
        if (toggle_2P.isOn) return 2;
        if (toggle_3P.isOn) return 3;
        if (toggle_4P.isOn) return 4;
        return 0;
    }

    // ---------------------------------------------------------------
    // TOGGLES CPU PLAYERS
    // ---------------------------------------------------------------

    private void ToggleCPUPlayersAlterado(int players)
    {
        // Comportamento de seleção única (radio)
        toggle_CPU_1P.SetIsOnWithoutNotify(players == 1 && toggle_CPU_1P.isOn);
        toggle_CPU_2P.SetIsOnWithoutNotify(players == 2 && toggle_CPU_2P.isOn);
        toggle_CPU_3P.SetIsOnWithoutNotify(players == 3 && toggle_CPU_3P.isOn);

        playersSeleccionados = ObterToggleAtivoCPUPlayers();

        // Atualizar quais CPUs são clicáveis
        AtualizarCPUsDisponiveis();

        VerificarGO();
    }

    private int ObterToggleAtivoCPUPlayers()
    {
        if (toggle_CPU_1P.isOn) return 1;
        if (toggle_CPU_2P.isOn) return 2;
        if (toggle_CPU_3P.isOn) return 3;
        return 0;
    }

    private void AtualizarCPUsDisponiveis()
    {
        // Máximo de CPUs = 4 - players (nunca exceder 4 no total)
        int maxCPU = 4 - playersSeleccionados;

        toggle_1C.interactable = maxCPU >= 1;
        toggle_2C.interactable = maxCPU >= 2;
        toggle_3C.interactable = maxCPU >= 3;

        // Se o CPU selecionado ficou fora do limite, reset
        if (cpusSeleccionados > maxCPU)
        {
            cpusSeleccionados = 0;
            toggle_1C.SetIsOnWithoutNotify(false);
            toggle_2C.SetIsOnWithoutNotify(false);
            toggle_3C.SetIsOnWithoutNotify(false);
        }
    }

    // ---------------------------------------------------------------
    // RESET
    // ---------------------------------------------------------------

    private void ResetSection_PvP()
    {
        toggle_2P.SetIsOnWithoutNotify(false);
        toggle_3P.SetIsOnWithoutNotify(false);
        toggle_4P.SetIsOnWithoutNotify(false);
        playersSeleccionados = 0;
    }

    private void ResetSection_CPU()
    {
        toggle_CPU_1P.SetIsOnWithoutNotify(false);
        toggle_CPU_2P.SetIsOnWithoutNotify(false);
        toggle_CPU_3P.SetIsOnWithoutNotify(false);
        toggle_1C.SetIsOnWithoutNotify(false);
        toggle_2C.SetIsOnWithoutNotify(false);
        toggle_3C.SetIsOnWithoutNotify(false);
        playersSeleccionados = 0;
        cpusSeleccionados = 0;

        // Reset interactable dos CPUs
        toggle_1C.interactable = false;
        toggle_2C.interactable = false;
        toggle_3C.interactable = false;
    }

    // ---------------------------------------------------------------
    // HELPERS
    // ---------------------------------------------------------------

    private void SetSectionInteractable(GameObject section, bool interactable)
    {
        Toggle[] toggles = section.GetComponentsInChildren<Toggle>(true);
        foreach (Toggle t in toggles)
            t.interactable = interactable;
    }

    // ---------------------------------------------------------------
    // VALIDAÇÃO DO GO
    // ---------------------------------------------------------------

    private void VerificarGO()
    {
        bool valido = false;

        if (modoAtual == Modo.PvP)
            valido = playersSeleccionados > 0;

        else if (modoAtual == Modo.CPU)
            valido = playersSeleccionados > 0 && cpusSeleccionados > 0;

        btn_GO.interactable = valido;
    }

    // ---------------------------------------------------------------
    // INICIAR JOGO
    // ---------------------------------------------------------------

    private void IniciarJogo()
    {
        // Guardar configuração no GameConfig
        GameConfig.Instance.SetConfig(
            isPvP: modoAtual == Modo.PvP,
            numPlayers: playersSeleccionados,
            numCPUs: modoAtual == Modo.CPU ? cpusSeleccionados : 0,
            lateraActive: toggle_Latera.isOn
        );

        // Avançar para a próxima scene
        SceneLoader.Instance.LoadScene("Game");
    }
}
