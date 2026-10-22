
using System.Collections;
using UnityEngine;

public class ReaccionQuimica : MonoBehaviour
{
    [Header("Objetos")]
    public Renderer liquido;
    public Transform pivoteTubo;

    [Header("Colores")]
    public Color colorInicial =
        new Color(0.85f, 0.95f, 1f, 0.65f);

    public Color colorFinal =
        new Color(1f, 0.05f, 0.55f, 0.85f);

    [Header("Animacion")]
    public float angulo = 25f;
    public float duracionGiro = 0.65f;
    public float duracionColor = 2f;

    private Material materialLiquido;
    private Quaternion rotacionInicial;
    private bool reaccionIniciada = false;

    private void Awake()
    {
        if (liquido == null || pivoteTubo == null)
        {
            Debug.LogError(
                "Faltan referencias en ReaccionQuimica."
            );

            enabled = false;
            return;
        }

        materialLiquido = liquido.material;

        materialLiquido.SetColor(
            "_BaseColor", colorInicial
        );

        rotacionInicial = pivoteTubo.localRotation;
    }

    [ContextMenu("Probar reaccion")]
    public void IniciarReaccion()
    {
        if (!enabled || reaccionIniciada)
            return;

        reaccionIniciada = true;

        // Activar telemetria al tocar el experimento.
        TelemetriaPH telemetria =
            FindFirstObjectByType<TelemetriaPH>();

        if (telemetria != null)
            telemetria.ActivarTelemetria();

        StartCoroutine(AnimarReaccion());
    }

    private IEnumerator AnimarReaccion()
    {
        Quaternion rotacionInclinada =
            rotacionInicial *
            Quaternion.Euler(0f, 0f, angulo);

        // 1. Inclinar el tubo suavemente.
        yield return Girar(
            rotacionInicial,
            rotacionInclinada,
            duracionGiro
        );

        // 2. Simular el momento del vertido.
        yield return new WaitForSeconds(0.3f);

        // 3. Cambiar el color del liquido.
        float tiempo = 0f;

        while (tiempo < duracionColor)
        {
            tiempo += Time.deltaTime;

            float t = Mathf.Clamp01(
                tiempo / Mathf.Max(0.1f, duracionColor)
            );

            float suave = Mathf.SmoothStep(0f, 1f, t);

            materialLiquido.SetColor(
                "_BaseColor",
                Color.Lerp(
                    colorInicial,
                    colorFinal,
                    suave
                )
            );

            yield return null;
        }

        materialLiquido.SetColor(
            "_BaseColor", colorFinal
        );

        // 4. Regresar el tubo suavemente.
        yield return Girar(
            rotacionInclinada,
            rotacionInicial,
            duracionGiro
        );

        Debug.Log("Reaccion quimica completada.");
    }

    private IEnumerator Girar(
        Quaternion desde,
        Quaternion hasta,
        float segundos)
    {
        float tiempo = 0f;

        while (tiempo < segundos)
        {
            tiempo += Time.deltaTime;

            float t = Mathf.Clamp01(
                tiempo / Mathf.Max(0.1f, segundos)
            );

            float suave = Mathf.SmoothStep(0f, 1f, t);

            pivoteTubo.localRotation =
                Quaternion.Slerp(desde, hasta, suave);

            yield return null;
        }

        pivoteTubo.localRotation = hasta;
    }

    private void OnDestroy()
    {
        if (materialLiquido != null)
            Destroy(materialLiquido);
    }
}