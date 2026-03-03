using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CometaController : MonoBehaviour
{
    [Header("Estrela (objeto 3D com Trail)")]
    public Transform estrela;
    public TrailRenderer trail;

    [Header("Imagem estática da cauda")]
    public Image imagemCauda;

    [Header("Curva Bezier — 5 pontos de controlo")]
    public Vector3 p0 = new Vector3(-5f, 8f, 0f);
    public Vector3 p1 = new Vector3(-2f, 6f, 0f);
    public Vector3 p2 = new Vector3(0f, 4f, 0f);
    public Vector3 p3 = new Vector3(3f, 0f, 0f);
    public Vector3 p4 = new Vector3(5f, -6f, 0f);

    [Header("Configuração")]
    public float duracaoMovimento = 1.4f;
    public float delayInicio = 0.5f;

    [Header("Audio")]
    public AudioClip somWhoosh;
    public AudioClip somSparkle;
    private AudioSource audioSource;

    [Header("Partículas")]
    public ParticleSystem posEstrela;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (estrela) estrela.position = p0;
        if (trail)
        {
            trail.enabled = false;
            trail.Clear();
        }
        if (imagemCauda)
        {
            Color c = imagemCauda.color;
            c.a = 0f;
            imagemCauda.color = c;
        }

        // Desativa o GameObject das partículas no início
        if (posEstrela) posEstrela.gameObject.SetActive(false);

        StartCoroutine(AnimarCometa());
    }

    IEnumerator AnimarCometa()
    {
        yield return new WaitForSeconds(delayInicio);

        if (audioSource && somWhoosh)
            audioSource.PlayOneShot(somWhoosh);

        if (trail)
        {
            trail.Clear();
            trail.enabled = true;
        }

        float tempo = 0f;
        while (tempo < duracaoMovimento)
        {
            tempo += Time.deltaTime;
            float t = Mathf.Clamp01(tempo / duracaoMovimento);
            estrela.position = Bezier5(t, p0, p1, p2, p3, p4);
            yield return null;
        }

        if (trail) trail.emitting = false;

        if (audioSource && somSparkle)
            audioSource.PlayOneShot(somSparkle);

        // Ativa e dispara as partículas
        if (posEstrela)
        {
            posEstrela.transform.position = p4;
            posEstrela.gameObject.SetActive(true);
            posEstrela.Play();
            Debug.Log("Partículas disparadas!");
        }
        else
        {
            Debug.LogError("PosEstrela não está ligado!");
        }

        yield return StartCoroutine(FadeInCauda(0.4f));
    }

    IEnumerator FadeInCauda(float duracao)
    {
        if (imagemCauda == null) yield break;

        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float alpha = Mathf.Clamp01(tempo / duracao);
            Color c = imagemCauda.color;
            c.a = alpha;
            imagemCauda.color = c;
            yield return null;
        }
    }

    Vector3 Bezier5(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float u = 1f - t;
        return (u * u * u * u * p0) +
               (4 * u * u * u * t * p1) +
               (6 * u * u * t * t * p2) +
               (4 * u * t * t * t * p3) +
               (t * t * t * t * p4);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < 40; i++)
        {
            float t1 = i / 40f;
            float t2 = (i + 1) / 40f;
            Gizmos.DrawLine(
                Bezier5(t1, p0, p1, p2, p3, p4),
                Bezier5(t2, p0, p1, p2, p3, p4)
            );
        }
    }
}