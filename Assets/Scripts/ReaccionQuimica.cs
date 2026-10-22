
using System.Collections;
using UnityEngine;

public class ReaccionQuimica : MonoBehaviour
{
    [Header("Objetos")]
    public Renderer liquido;
    public Transform pivoteTubo;

    [Header("Colores")]
    public Color colorInicial = new Color(0.85f, 0.95f, 1f, 0.65f);
    public Color colorFinal = new Color(1f, 0.05f, 0.55f, 0.85f);

    [Header("Animacion")]
    public float angulo = -50f;
    public float duracionColor = 2f;

    private Material materialLiquido;
    private Quaternion rotacionInicial;
    private bool reaccionIniciada = false;

    void Awake()
    {
        if (liquido == null || pivoteTubo == null)
        {
            Debug.LogError("Faltan referencias en ReaccionQuimica");
            enabled = false;
            return;
        }

        materialLiquido = liquido.material;
        materialLiquido.SetColor("_BaseColor", colorInicial);
        rotacionInicial = pivoteTubo.localRotation;
    }

    [ContextMenu("Probar reaccion")]
    public void IniciarReaccion()
    {
        if (reaccionIniciada || !enabled) return;

        StartCoroutine(AnimarReaccion());
    }

    IEnumerator AnimarReaccion()
    {
        reaccionIniciada = true;

        Quaternion inclinada = rotacionInicial *
            Quaternion.Euler(0f, 0f, angulo);

        // Inclinar tubo
        yield return Girar(rotacionInicial, inclinada, 0.6f);

        // Cambiar color del liquido
        float tiempo = 0f;

        while (tiempo < duracionColor)
        {
            tiempo += Time.deltaTime;

            float progreso = Mathf.Clamp01(
                tiempo / Mathf.Max(0.1f, duracionColor)
            );

            materialLiquido.SetColor(
                "_BaseColor",
                Color.Lerp(colorInicial, colorFinal, progreso)
            );

            yield return null;
        }

        materialLiquido.SetColor("_BaseColor", colorFinal);

        // Regresar tubo
        yield return Girar(inclinada, rotacionInicial, 0.6f);

        Debug.Log("Reaccion quimica completada");
    }

    IEnumerator Girar(Quaternion desde, Quaternion hasta, float segundos)
    {
        float t = 0f;

        while (t < 1f)
        {
            t = Mathf.Min(1f, t + Time.deltaTime / segundos);

            pivoteTubo.localRotation =
                Quaternion.Slerp(desde, hasta, t);

            yield return null;
        }
    }

    void OnDestroy()
    {
        if (materialLiquido != null)
            Destroy(materialLiquido);
    }
}

