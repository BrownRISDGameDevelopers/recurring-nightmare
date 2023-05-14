using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContainer : MonoBehaviour
{
    [SerializeField] private Sprite daySprite;
    [SerializeField] private Sprite nightSprite;

    private Image _image;
    void Start()
    {
        _image = gameObject.GetComponent<Image>();
    }
    
    public void ChangeSprite(bool isSwitchingToDay)
    {
        _image.sprite = isSwitchingToDay ? daySprite : nightSprite;
    }
}
