using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private Image _crosshairImage;

    private void Awake()
    {
        if (_crosshairImage == null)
        {
            _crosshairImage = GetComponent<Image>();
        }
    }
}