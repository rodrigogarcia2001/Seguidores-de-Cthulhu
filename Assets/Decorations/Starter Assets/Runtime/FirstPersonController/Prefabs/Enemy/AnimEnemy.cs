using UnityEngine;
using UnityEngine.AI;

public class AnimacionesEnemigo : MonoBehaviour
{
    private Animator anim;
    private Vector3 ultimaPosicion;

    void Start()
    {
        anim = GetComponent<Animator>();
        ultimaPosicion = transform.position;
    }

    void Update()
    {
        if (anim == null) return;

        Vector3 velocidadActual = (transform.position - ultimaPosicion) / Time.deltaTime;
        float velocidad = velocidadActual.magnitude;
        ultimaPosicion = transform.position;

        anim.SetFloat("Speed", velocidad);
    }
}