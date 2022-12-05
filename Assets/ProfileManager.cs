using System;
using UnityEngine;

public class ProfileManager : Singleton<ProfileManager>
{
    [SerializeField] private GameObject flagPrefab;
    [SerializeField] private GameObject framePrefab;
    [SerializeField] private Transform scrollWheel;
    [SerializeField] private Avatar avatar;

    internal ProfileFlag activeFlag;
    internal ProfileFrame activeFrame;

    private void Awake()
    {
       // GameManager.Instance.isSelectable = false;
    }

    public Countries country
    {
        get => activeFlag.country;
        set => activeFlag.country = value;
    }

    public Sprite avatarSprite => Resources.Load<Sprite>($"avatars/avatar{avatarNumber}");

    public int avatarNumber
    {
        get => PlayerPrefs.GetInt("avatar", -1);
        set => PlayerPrefs.SetInt("avatar", value);
    }

    public string username
    {
        get => PlayerPrefs.GetString("username", "You");
        set => PlayerPrefs.SetString("username", value);
    }

    public void SetUsername(string name) => username = name;

    void Start()
    {
       // PopulateContainer(1);
    }

    public void SetCountry(Countries country, ProfileFlag flag)
    {
        if (activeFlag != null) activeFlag.Deselect();
        activeFlag = flag;
        avatar.SetAvatar(flag.flagSprite);
    }

    public void SetFrame(Frames frame, ProfileFrame profileFrame)
    {
        if (activeFrame != null) activeFrame.Deselect();
        activeFrame = profileFrame;
        avatar.SetFrame(profileFrame.frameSprite);
    }


    public void SetView(int index)
    {
        ClearContainer();
        PopulateContainer(index);
    }

    private void ClearContainer()
    {
        foreach (Transform child in scrollWheel) Destroy(child.gameObject);
    }

    private void PopulateContainer(int index)
    {
        switch (index)
        {
            case 0:
                foreach (Countries country in Enum.GetValues(typeof(Countries)))
                {
                    ProfileFlag.Init(flagPrefab, scrollWheel, country);
                }
                break;
            case 1:
                foreach (Frames frame in Enum.GetValues(typeof(Frames)))
                {
                    ProfileFrame.Init(framePrefab, scrollWheel, frame);
                }
                break;
            default:
                break;
        }
    }
}

public enum Frames
{
    red, blue
}
