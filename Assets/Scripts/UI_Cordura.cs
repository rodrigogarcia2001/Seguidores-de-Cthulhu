using UnityEngine;
using UnityEngine.UI;
public class UI_Cordura : MonoBehaviour
{
    public Slider barra;
    public SistemaCordura sistema;
    public Image fill;
    void Start()
    {
        barra.maxValue = sistema.CorduraMax;
    }

    void Update()
    {
        barra.value = sistema.CorduraActual;

        float porcentaje = sistema.CorduraActual / sistema.CorduraMax;

        //fill.color = Color.Lerp(Color.red, Color.green, porcentaje);
    }
}
