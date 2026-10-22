using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class GestorNavegacion : MonoBehaviour
{
    public static int ExperimentoSeleccionado = 1;

    [Header("Referencias en la Escena AR")]
    public ARTrackedImageManager gestorImagenesAR;
    public GameObject prefabExperimento1;
    public GameObject prefabExperimento2;

    [Header("Textos del Panel Lateral en Escena AR")]
    public TMP_Text textoNombre;
    public TMP_Text textoDescripcion;
    public TMP_Text textoMateriales;

    private void Awake()
    {
        if (gestorImagenesAR != null)
        {
            ConfigurarExperimentoActual();
        }
    }

    public void AbrirExperimento1()
    {
        ExperimentoSeleccionado = 1;
        SceneManager.LoadScene("EscenaAR");
    }

    public void AbrirExperimento2()
    {
        ExperimentoSeleccionado = 2;
        SceneManager.LoadScene("EscenaAR");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void ConfigurarExperimentoActual()
    {
        if (ExperimentoSeleccionado == 1)
        {
            gestorImagenesAR.trackedImagePrefab = prefabExperimento1;

            if (textoNombre != null)
                textoNombre.text = "Experimento 1: Indicador de pH (Fenolftaleína)";
            if (textoDescripcion != null)
                textoDescripcion.text = "Observa cómo una solución alcalina incolora revela su pH básico al entrar en contacto con el indicador del tubo de ensayo.";
            if (textoMateriales != null)
                textoMateriales.text = "Materiales:\n• Mesa de trabajo\n• Beaker con solución NaOH\n• Tubo de ensayo con fenolftaleína";
        }
        else if (ExperimentoSeleccionado == 2)
        {
            gestorImagenesAR.trackedImagePrefab = prefabExperimento2;

            if (textoNombre != null)
                textoNombre.text = "Experimento 2: El Camaleón Químico (Redox)";
            if (textoDescripcion != null)
                textoDescripcion.text = "El permanganato de potasio cambia por tres colores distintos (violeta, verde y amarillo) a medida que sus átomos ganan electrones.";
            if (textoMateriales != null)
                textoMateriales.text = "Materiales:\n• Mesa y Bandeja de contención\n• Probeta con KMnO4 (Violeta)\n• Gradilla y Tubo de ensayo con glucosa";
        }
    }
}