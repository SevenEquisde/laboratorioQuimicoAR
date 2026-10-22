
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class TelemetriaPH : MonoBehaviour
{
    public TMP_Text textoPH;

    private string url =
        "https://api.thingspeak.com/channels/3545115/feeds/last.json";

    [Serializable]
    public class DatosPH
    {
        public string field1;
        public int entry_id;
    }

    IEnumerator Start()
    {
        if (textoPH == null)
        {
            Debug.LogError("Falta conectar TextoPH");
            yield break;
        }

        while (true)
        {
            using (UnityWebRequest peticion = UnityWebRequest.Get(url))
            {
                peticion.timeout = 10;
                yield return peticion.SendWebRequest();

                if (peticion.result == UnityWebRequest.Result.Success)
                {
                    DatosPH datos = JsonUtility.FromJson<DatosPH>(
                        peticion.downloadHandler.text
                    );

                    if (datos != null && !string.IsNullOrEmpty(datos.field1))
                    {
                        textoPH.text = "pH simulado: " + datos.field1;
                        Debug.Log("pH recibido: " + datos.field1);
                    }
                    else
                    {
                        textoPH.text = "pH: Sin datos";
                    }
                }
                else
                {
                    textoPH.text = "pH: Error de conexion";
                    Debug.LogWarning(peticion.error);
                }
            }

            yield return new WaitForSeconds(10f);
        }
    }
}
