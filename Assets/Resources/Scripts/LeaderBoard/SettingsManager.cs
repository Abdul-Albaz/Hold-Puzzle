using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    [Header("Settings")]
    [SerializeField] private GameObject settingsButtonIcon;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private Image settingsMenuOverlay;

    

    internal bool isVisible = false;

    private void Start()
    {
       
    }

    public void toggleWindow(float duration = 0.5f)
    {
        if (isVisible)
        {
            isVisible = false;
            settingsMenuOverlay.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), duration).OnComplete(() => settingsMenuOverlay.gameObject.SetActive(false));
            settingsButtonIcon.transform.DORotate(Vector3.zero, duration).SetEase(Ease.OutBack);
            settingsWindow.transform.DOLocalMoveY(1500, duration / 2).SetEase(Ease.InBack).OnComplete(() => settingsWindow.SetActive(false));
            return;
        }

       

        isVisible = true;
        settingsMenuOverlay.gameObject.SetActive(true);
        settingsMenuOverlay.DOColor(new Color(0, 0, 0, 0.5f), duration);
        settingsButtonIcon.transform.DORotate(Vector3.forward * -90, duration / 2).SetEase(Ease.OutBack);
        settingsWindow.SetActive(true);
        settingsWindow.transform.DOLocalMoveY(0, duration).SetEase(Ease.OutBack);
    }

    public void tappedSettings()
    {
        Taptic.Medium();
        toggleWindow();
    }

    public void toggletheme()
    {

        Taptic.Medium();
        Toggle(Setting.theme);
        ThemeManager.Instance.toggle();
    }

    public void ToggleSound()
    {
        Taptic.Medium();
        Toggle(Setting.audio);
        AudioListener.volume = Setting.audio.number;
    }

    private void Toggle(Setting setting)
    {
        setting.toggle();
       
    }

  
}

public enum Settings
{
    audio,
    theme
}

public class Setting
{
    public readonly static Setting audio = new(Settings.audio);
    public readonly static Setting theme = new(Settings.theme);

    internal readonly Settings setting;
    private readonly Sprite[] sprites;

    public Setting(Settings setting)
    {
        this.setting = setting;
        if (!hasSprite) return;
        sprites = new Sprite[2];
        sprites[0] = Resources.Load<Sprite>($"{setting}Off");
        sprites[1] = Resources.Load<Sprite>($"{setting}On");
    }

    public void toggle() => PlayerPrefs.SetInt(setting.ToString(), isOn ? 0 : 1);
    internal bool isOn => PlayerPrefs.GetInt(setting.ToString(), 1) == 1;
    internal int number => PlayerPrefs.GetInt(setting.ToString(), 1);

    internal bool hasSprite => new List<Settings>() { Settings.audio, Settings.theme }.Contains(setting);
    internal Sprite sprite => sprites[number];
    internal string text => setting switch
    {
        Settings.audio => isOn ? "Sound: On" : "Sound: Off",
        Settings.theme => isOn ? "Theme: Light" : "Theme: Dark",
        _ => ""
    };
}
