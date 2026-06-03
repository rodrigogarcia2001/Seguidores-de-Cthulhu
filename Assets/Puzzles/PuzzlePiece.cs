using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Vector2Int currentPos;
    public Vector2Int correctPos;

    public PuzzleManager manager;

    public Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            rend.material.color = Color.yellow;
            transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            rend.material.color = Color.white;
            transform.localScale = Vector3.one;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Pieza Seleccionada");
        manager.SelectPiece(this);
    }
    public void UpdateColor()
    {
        if (currentPos == correctPos)
        {
            rend.material.color = new Color(0.8f, 1f, 0.8f);
        }
        else
        {
            rend.material.color = Color.white;
        }
    }
}