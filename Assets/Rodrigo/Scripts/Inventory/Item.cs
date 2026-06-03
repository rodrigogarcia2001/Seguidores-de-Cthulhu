using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string nameItem;
    public Sprite icon;
    public int id;
    public GameObject prefab;
}
