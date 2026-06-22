using UnityEngine;

public class HideRenderer : MonoBehaviour
{
    [Tooltip("Si está activo, oculta el render automáticamente al arrancar")]
    [SerializeField] private bool hideOnStart = true;

    private Renderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        if (hideOnStart)
        {
            Hide();
        }
    }

    public void Hide()
    {
        SetVisible(false);
    }

    public void Show()
    {
        SetVisible(true);
    }

    private void SetVisible(bool visible)
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }
}
