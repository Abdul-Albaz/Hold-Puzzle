using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProfileFrame : MonoBehaviour
{
    public Frames frame;
    [SerializeField] private Image image;
    [SerializeField] private Transform selectionBox;
    [SerializeField] private Transform check;

    internal Sprite frameSprite => Resources.Load<Sprite>($"frames/{Enum.GetName(typeof(Frames), frame)}".ToLower());

    public static void Init(GameObject prefab, Transform parent, Frames frame)
    {
        ProfileFrame profileFrame = Instantiate(prefab, parent).GetComponent<ProfileFrame>();
        profileFrame.frame = frame;
        profileFrame.image.sprite = profileFrame.frameSprite;
    }

    public void Tapped()
    {
        Taptic.Medium();
        Select();
        ProfileManager.Instance.SetFrame(frame, this);
    }

    private void Select()
    {
        selectionBox.DOScale(1, 0.25f).SetEase(Ease.OutSine);
        check.DOScale(1, 0.25f).SetEase(Ease.OutSine);
    }

    public void Deselect()
    {
        selectionBox.DOScale(0, 0.25f).SetEase(Ease.OutSine);
        check.DOScale(0, 0.25f).SetEase(Ease.OutSine);
    }
}