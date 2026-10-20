using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class GestorNavegacion : MonoBehaviour
{
    // Variable estática: conserva su valor al cambiar de la escena del menú a la escena AR
    public static int ExperimentoSeleccionado = 1;

    [Header("Referencias en la Escena AR (Opcional en Menú)")]
    public ARTrackedImageManager gestorImagenesAR;
    public GameObject prefabExperimento1;
    public GameObject prefabExperimento2;

    [Header("Textos del Panel Lateral en Escena AR")]
    public TMP_Text textoNombre;
    public TMP_Text textoDescripcion;
    public TMP_Text textoMateriales;

    private void Awake()
    {
        // Si este script está en la Escena AR y tiene asignado el gestor de imágenes,
        // configura el prefab y los textos antes de que empiece a rastrear la cámara.
        if (gestorImagenesAR != null)
        {
            ConfigurarExperimentoActual();
        }
    }

    // Se asigna al botón "Experimento 1" del Menú Principal
    public void AbrirExperimento1()
    {
        ExperimentoSeleccionado = 1;
        SceneManager.LoadScene("EscenaAR");
    }

    // Se asigna al botón "Experimento 2" del Menú Principal
    public void AbrirExperimento2()
    {
        ExperimentoSeleccionado = 2;
        SceneManager.LoadScene("EscenaAR");
    }

    // Se asigna al botón "Volver al Menú" de la Escena AR
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
                textoNombre.text = "Experimento 1: Neutralización Ácido-Base";
            if (textoDescripcion != null)
                textoDescripcion.text = "Al mezclar ácido clorhídrico con hidróxido de sodio en presencia de fenolftaleína, la solución cambia de color indicando el equilibrio de pH. Toca el modelo 3D para iniciar la reacción.";
            if (textoMateriales != null)
                textoMateriales.text = "Materiales:\n• Matraz Erlenmeyer\n• Bureta graduada\n• Soporte universal\n• Solución HCl y NaOH";
        }
        else if (ExperimentoSeleccionado == 2)
        {
            gestorImagenesAR.trackedImagePrefab = prefabExperimento2;

            if (textoNombre != null)
                textoNombre.text = "Experimento 2: Descomposición Catalítica";
            if (textoDescripcion != null)
                textoDescripcion.text = "El peróxido de hidrógeno se descompone rápidamente al entrar en contacto con el yoduro de potasio, liberando oxígeno gaseoso y espuma exotérmica. Toca el modelo 3D para iniciar la reacción.";
            if (textoMateriales != null)
                textoMateriales.text = "Materiales:\n• Probeta de 500 ml\n• Vaso de precipitados\n• Peróxido de hidrógeno (H2O2)\n• Catalizador KI";
        }
    }
}