using UnityEngine;

public class ReaccionQuimica : MonoBehaviour
{
    [Header("Configuración de Animación")]
    [Tooltip("Nombre del parámetro Trigger en el Animator o del clip de animación")]
    public string nombreTriggerOAnimacion = "Reaccionar";

    [Header("Efecto Opcional de Partículas (Espuma/Humo)")]
    public ParticleSystem efectoParticulas;

    private Animator componenteAnimator;
    private Animation componenteAnimation;

    private void Awake()
    {
        // Busca el componente Animator o Animation en este objeto o en sus objetos hijos
        componenteAnimator = GetComponentInChildren<Animator>();
        componenteAnimation = GetComponentInChildren<Animation>();
    }

    // Este método será llamado cuando el usuario haga tap sobre el modelo 3D
    public void IniciarReaccion()
    {
        Debug.Log("Reacción química activada en: " + gameObject.name);

        // Opción A: Si el modelo usa el sistema moderno Animator
        if (componenteAnimator != null)
        {
            // Si creaste un parámetro Trigger en el Animator Controller, lo activa;
            // de lo contrario, reproduce el estado de animación desde el inicio (tiempo 0)
            componenteAnimator.SetTrigger(nombreTriggerOAnimacion);
            componenteAnimator.Play(nombreTriggerOAnimacion, 0, 0f);
        }
        // Opción B: Si el modelo usa el sistema clásico Animation
        else if (componenteAnimation != null)
        {
            if (!string.IsNullOrEmpty(nombreTriggerOAnimacion) && componenteAnimation.GetClip(nombreTriggerOAnimacion) != null)
            {
                componenteAnimation.Play(nombreTriggerOAnimacion);
            }
            else
            {
                // Reproduce la animación por defecto que traiga el modelo
                componenteAnimation.Play();
            }
        }

        // Si el equipo agregó un sistema de partículas (burbujas o vapor), lo reproduce
        if (efectoParticulas != null)
        {
            efectoParticulas.Play();
        }
    }
}
