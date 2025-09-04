using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField] private Image image;

    public void Init()
    {
        image.color = Color.black;
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }
}
