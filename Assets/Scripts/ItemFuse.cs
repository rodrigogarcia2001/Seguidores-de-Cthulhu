using UnityEngine;

public class ItemFuse : MonoBehaviour
{
    [Header("Configuración del Brillo")]
    public Renderer meshRendererRealista;       // El modelo realista (el hijo)
    public float distanciaVisualizacion = 7.0f;  // A cuántos metros empieza a brillar
    public float velocidadTitileo = 6f;          // Qué tan rápido parpadea
    [ColorUsage(true, true)] 
    public Color colorDelBrillo = Color.white;   // Color en HDR

    [Header("Configuración de Audio")]
    public AudioClip sonidoRecoger;              // ¡Arrastra aquí tu archivo de sonido (.mp3 o .wav)!

    [Header("Referencias Opcionales")]
    public Transform jugador;                    

    private bool isPlayerNearby;
    private Material materialFusible;

    private void Start()
    {
        if (meshRendererRealista != null)
        {
            materialFusible = meshRendererRealista.material;
            materialFusible.EnableKeyword("_EMISSION");
        }

        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) jugador = playerObj.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = false;
    }

    private void Update()
    {
        // --- CONTROL DEL BRILLO ---
        if (materialFusible != null && jugador != null)
        {
            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            if (distanciaAlJugador <= distanciaVisualizacion)
            {
                float intensidad = (Mathf.Sin(Time.time * velocidadTitileo) + 1f) / 2f;
                materialFusible.SetColor("_EmissionColor", colorDelBrillo * intensidad);
            }
            else
            {
                materialFusible.SetColor("_EmissionColor", Color.black);
            }
        }

        // --- LÓGICA DE INTERACCIÓN Y AUDIO ---
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // 1. Si asignaste un sonido, lo reproducimos en la posición actual del fusible
            if (sonidoRecoger != null)
            {
                AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
            }

            // 2. Sumamos el fusible al contador
            FuseCounter.Instance.AddFuse();
            
            // 3. Destruimos el objeto de la escena sin cortar el audio
            Destroy(gameObject);
        }
    }
}