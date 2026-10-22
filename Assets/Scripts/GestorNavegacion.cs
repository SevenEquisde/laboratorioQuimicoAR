
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class GestorNavegacion : MonoBehaviour
{
    public static int ExperimentoSeleccionado = 1;

    [Header("Referencias AR")]
    public ARTrackedImageManager gestorImagenesAR;
    public GameObject prefabExperimento1;
    public GameObject prefabExperimento2;

    [Header("Configuracion del Marcador")]
    public string nombreMarcador = "Marcador_1";

    [Tooltip("Escala inicial para los modelos importados de Blender")]
    public float escalaAR = 0.003f;

    [Tooltip("Altura del laboratorio sobre el marcador, en metros")]
    public float alturaAR = 0f;

    [Header("Interfaz")]
    public TMP_Text textoNombre;
    public TMP_Text textoDescripcion;
    public TMP_Text textoMateriales;

    private void Awake()
    {
        if (gestorImagenesAR == null)
            gestorImagenesAR = GetComponent<ARTrackedImageManager>();

        if (gestorImagenesAR != null)
        {
            // AR Foundation crea el objeto de seguimiento.
            // Nosotros agregamos el laboratorio como hijo.
            gestorImagenesAR.trackedImagePrefab = null;

            ConfigurarExperimentoActual();
        }
    }

    private void OnEnable()
    {
        if (gestorImagenesAR != null)
        {
            gestorImagenesAR.trackablesChanged.AddListener(
                AlCambiarImagenes
            );
        }
    }

    private void OnDisable()
    {
        if (gestorImagenesAR != null)
        {
            gestorImagenesAR.trackablesChanged.RemoveListener(
                AlCambiarImagenes
            );
        }
    }

    // NAVEGACION

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

    // INFORMACION DIDACTICA

    private void ConfigurarExperimentoActual()
    {
        if (ExperimentoSeleccionado == 1)
        {
            if (textoNombre != null)
                textoNombre.text =
                    "Experimento 1: Indicador de pH";

            if (textoDescripcion != null)
                textoDescripcion.text =
                    "La fenolftaleina cambia a rosa al " +
                    "entrar en contacto con una solucion alcalina.";

            if (textoMateriales != null)
                textoMateriales.text =
                    "Materiales:\n" +
                    "- Beaker con solucion de NaOH\n" +
                    "- Tubo de ensayo\n" +
                    "- Fenolftaleina";
        }
        else
        {
            if (textoNombre != null)
                textoNombre.text = "Experimento 2";

            if (textoDescripcion != null)
                textoDescripcion.text =
                    "Experimento en desarrollo.";
        }
    }

    // DETECCION DEL MARCADOR

    private void AlCambiarImagenes(
        ARTrackablesChangedEventArgs<ARTrackedImage> cambios)
    {
        foreach (ARTrackedImage imagen in cambios.added)
        {
            ActualizarExperimento(imagen);
        }

        foreach (ARTrackedImage imagen in cambios.updated)
        {
            ActualizarExperimento(imagen);
        }
    }

    private void ActualizarExperimento(ARTrackedImage imagen)
    {
        if (imagen.referenceImage.name != nombreMarcador)
            return;

        // Buscar si el experimento ya fue creado.
        ReaccionQuimica reaccion =
            imagen.GetComponentInChildren<ReaccionQuimica>(true);

        // Si se pierde el seguimiento, ocultar el modelo.
        if (imagen.trackingState != TrackingState.Tracking)
        {
            if (reaccion != null)
                reaccion.gameObject.SetActive(false);

            return;
        }

        // Si ya existe, simplemente mostrarlo otra vez.
        if (reaccion != null)
        {
            reaccion.gameObject.SetActive(true);
            return;
        }

        GameObject prefab = ExperimentoSeleccionado == 1
            ? prefabExperimento1
            : prefabExperimento2;

        if (prefab == null)
        {
            Debug.LogWarning("No se asigno el prefab.");
            return;
        }

        // Crear laboratorio como hijo del marcador.
        GameObject laboratorio = Instantiate(
            prefab,
            imagen.transform,
            false
        );

        laboratorio.transform.localPosition =
            new Vector3(0f, alturaAR, 0f);

        laboratorio.transform.localRotation =
            Quaternion.identity;

        laboratorio.transform.localScale =
            Vector3.one * Mathf.Max(0.0001f, escalaAR);

        Debug.Log("Experimento AR detectado y creado.");
    }
}
