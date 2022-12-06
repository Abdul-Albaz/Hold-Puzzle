using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProfileFlag : MonoBehaviour
{
    public Countries country;
    [SerializeField] private Image image;
    [SerializeField] private Transform selectionBox;
    [SerializeField] private Transform check;

    internal Sprite flagSprite => Resources.Load<Sprite>($"flags/{Enum.GetName(typeof(Countries), country)}".ToLower());

    public static void Init(GameObject prefab, Transform parent, Countries country)
    {
        ProfileFlag flag = Instantiate(prefab, parent).GetComponent<ProfileFlag>();
        flag.country = country;
        flag.image.sprite = flag.flagSprite;
    }

    public void Tapped()
    {
        Taptic.Medium();
        Select();
        ProfileManager.Instance.SetCountry(country, this);
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
