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
        Button[] todosLosBotones = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button boton in todosLosBotones)
        {
            AsignarEventoHover(boton);
        }
    }

    void AsignarEventoHover(Button boton)
    {
        EventTrigger trigger = boton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = boton.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entradaEvento = new EventTrigger.Entry();
        entradaEvento.eventID = EventTriggerType.PointerEnter;

        entradaEvento.callback.AddListener((eventData) => { ReproducirSonido(); });

        trigger.triggers.Add(entradaEvento);
    }

    public void ReproducirSonido()
    {
        if (audioSource != null && sonidoHover != null)
        {
            audioSource.PlayOneShot(sonidoHover);
        }
    }
}
