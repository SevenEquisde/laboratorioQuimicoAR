
using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class TelemetriaPH : MonoBehaviour
{
    [Header("Interfaz")]
    public TMP_Text textoPH;

    [Header("Telemetria IoT")]
    public float intervaloSegundos = 10f;

    private const string URL =
        "https://api.thingspeak.com/channels/3545115/feeds/last.json";

    private bool telemetriaActivada = false;

    [Serializable]
    public class DatosPH
    {
        public string field1;
        public string created_at;
        public int entry_id;
    }

    private IEnumerator Start()
    {
        if (textoPH == null)
        {
            Debug.LogError("Falta asignar TextoPH.");
            yield break;
        }

        textoPH.text = "pH simulado:\nEsperando reaccion...";

        // Esperar a que el usuario toque el experimento.
        yield return new WaitUntil(() => telemetriaActivada);

        while (true)
        {
            using (UnityWebRequest peticion =
                UnityWebRequest.Get(URL))
            {
                peticion.timeout = 10;

                yield return peticion.SendWebRequest();

                if (peticion.result ==
                    UnityWebRequest.Result.Success)
                {
                    DatosPH datos = JsonUtility.FromJson<DatosPH>(
                        peticion.downloadHandler.text
                    );

                    if (datos != null &&
                        float.TryParse(
                            datos.field1,
                            NumberStyles.Float,
                            CultureInfo.InvariantCulture,
                            out float ph))
                    {
                        bool datoAntiguo = false;

                        if (DateTimeOffset.TryParse(
                            datos.created_at,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeUniversal,
                            out DateTimeOffset fecha))
                        {
                            datoAntiguo =
                                (DateTimeOffset.UtcNow - fecha)
                                .TotalSeconds > 90;
                        }

                        string valor = ph.ToString(
                            "F2",
                            CultureInfo.InvariantCulture
                        );

                        textoPH.text =
                            "pH simulado: " + valor +
                            (datoAntiguo
                                ? "\nUltimo dato historico"
                                : "\nDatos recientes");

                        Debug.Log("pH recibido: " + valor);
                    }
                    else
                    {
                        textoPH.text =
                            "pH: Sin datos disponibles";
                    }
                }
                else
                {
                    textoPH.text =
                        "pH: Error de conexion";

                    Debug.LogWarning(
                        "ThingSpeak: " + peticion.error
                    );
                }
            }

            yield return new WaitForSeconds(
                Mathf.Max(5f, intervaloSegundos)
            );
        }
    }

    // Lo llama ReaccionQuimica cuando el usuario toca.
    public void ActivarTelemetria()
    {
        if (telemetriaActivada)
            return;

        telemetriaActivada = true;

        if (textoPH != null)
            textoPH.text =
                "pH simulado:\nConectando con ThingSpeak...";

        Debug.Log("Telemetria IoT activada.");
    }
}
