using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Sprite defaultCrosshair;

    [HideInInspector] public Image UIImage;

    public void Start()
    {
        UIImage = GetComponent<Image>();
        SetDefaultCrosshair();
    }

    public void SetNewCrosshair(Sprite newCrosshair)
    {
        UIImage.sprite = newCrosshair;
    }

    public void SetDefaultCrosshair()
    {
        SetNewCrosshair(defaultCrosshair);
    }
}