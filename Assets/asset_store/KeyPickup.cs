using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public static bool hasKey = false;

    public void PickUp()
    {
        hasKey = true;
        Destroy(gameObject);
    }
}