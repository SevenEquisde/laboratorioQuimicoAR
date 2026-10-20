using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectorTapAR : MonoBehaviour
{
    [Tooltip("Arrastra aquí la Main Camera que está dentro de XR Origin")]
    public Camera camaraAR;

    private void Start()
    {
        // Si olvidaste arrastrar la cámara en el Inspector, busca la cámara principal automáticamente
        if (camaraAR == null)
        {
            camaraAR = Camera.main;
        }
    }

    private void Update()
    {
        // 1. Detectar toque con el dedo en pantalla Android
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);

            // Solo actuamos en el instante exacto en que el dedo toca el cristal (Began)
            if (toque.phase == TouchPhase.Began)
            {
                // Evita lanzar el rayo si el usuario tocó un botón de la interfaz (UI)
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(toque.fingerId))
                {
                    return;
                }

                LanzarRayo(toque.position);
            }
        }
        // 2. Detectar clic izquierdo del mouse (útil para probar en la PC dentro del Editor)
        else if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            LanzarRayo(Input.mousePosition);
        }
    }

    private void LanzarRayo(Vector2 posicionPantalla)
    {
        if (camaraAR == null) return;

        // Crea un rayo desde la cámara AR hacia el punto tocado en la pantalla
        Ray rayo = camaraAR.ScreenPointToRay(posicionPantalla);
        RaycastHit impacto;

        // Si el rayo golpea cualquier objeto 3D con un Collider en la escena:
        if (Physics.Raycast(rayo, out impacto))
        {
            // Busca el script ReaccionQuimica en el objeto tocado o en su contenedor padre
            ReaccionQuimica experimento = impacto.collider.GetComponentInParent<ReaccionQuimica>();

            if (experimento != null)
            {
                experimento.IniciarReaccion();
            }
        }
    }
}