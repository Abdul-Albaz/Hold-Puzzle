using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MonoBehaviour
{
    [SerializeField] private Image avatarImage;
    [SerializeField] private Image frameImage;

    void Start()
    {
        
    }

    public void SetAvatar(Sprite sprite) => avatarImage.sprite = sprite;
    public void SetFrame(Sprite frameSprite) => frameImage.sprite = frameSprite;
}
