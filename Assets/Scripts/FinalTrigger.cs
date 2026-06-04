using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
public EndingCamera endingCamera;

private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        endingCamera.StartEnding();
    }
}
}
