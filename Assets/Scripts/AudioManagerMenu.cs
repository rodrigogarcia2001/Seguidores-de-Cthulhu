using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManagerMenu : MonoBehaviour
{
    [Header("Componentes de Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoHover;

    void Start()
    {
        // 1. Buscamos TODOS los botones activos en la escena de forma automática
        Button[] todosLosBotones = FindObjectsByType<Button>(FindObjectsSortMode.None);

        // 2. A cada uno le inyectamos la capacidad de escuchar el mouse
        foreach (Button boton in todosLosBotones)
        {
            AsignarEventoHover(boton);
        }
    }

    void AsignarEventoHover(Button boton)
    {
        // Intentamos obtener el EventTrigger, si no lo tiene, se lo agregamos dinámicamente
        EventTrigger trigger = boton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = boton.gameObject.AddComponent<EventTrigger>();
        }

        // Creamos el evento de "PointerEnter" (cuando el mouse entra al área del botón)
        EventTrigger.Entry entradaEvento = new EventTrigger.Entry();
        entradaEvento.eventID = EventTriggerType.PointerEnter;
        
        // Le decimos que ejecute nuestra función de reproducir sonido
        entradaEvento.callback.AddListener((eventData) => { ReproducirSonido(); });
        
        // Lo sumamos a la lista de triggers del botón
        trigger.triggers.Add(entradaEvento);
    }

    public void ReproducirSonido()
    {
        if (audioSource != null && sonidoHover != null)
        {
            // PlayOneShot permite que el sonido suene encima de otros sin cortarse
            audioSource.PlayOneShot(sonidoHover);
        }
    }
}
