using UnityEngine;
public class CameraRotate : MonoBehaviour
{
    [Tooltip("Velocidad de rotación. Valores positivos giran a la derecha, negativos a la izquierda.")]
    public float speed = 5f; 
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}