using UnityEngine;

public class AnimationsCharacter : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float velocidad = controller.velocity.magnitude;
        anim.SetFloat("Speed", velocidad);
    }
}