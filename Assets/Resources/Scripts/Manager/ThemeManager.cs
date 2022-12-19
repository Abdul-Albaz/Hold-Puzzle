using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : Singleton<ThemeManager>
{
    internal Theme theme
    {
        get => new((Themes)PlayerPrefs.GetInt("theme", 1));
        set => PlayerPrefs.SetInt("theme", (int) value.theme);
    }

    private void Start()
    {
        print((Themes)PlayerPrefs.GetInt("theme", 0));
        print((int)theme.theme);
    }

    public void toggle() {
        //theme = theme.theme == Themes.light ? Theme.dark : Theme.light;
        //Background.Instance.paint();
//        if (UIManager.Instance.view == Views.game) GameManager.Instance.paintTiles();
    }
}

public enum Themes
{
    dark, light
}

public class Theme
{
    public readonly static Theme light = new(Themes.light);
    public readonly static Theme dark = new(Themes.dark);

    public Themes theme;

    public Theme(Themes theme)
    {
        this.theme = theme;
    }

    public Color backgroundColor => theme switch
    {
        //Themes.dark => new(30, 30, 30, 255),
        //_ => new(255, 255, 255, 255)

        Themes.dark => new(0.12f, 0.12f, 0.12f, 1f),
        _ => new(1f, 1f, 1f, 1f)
    };

    public Color tileColor => theme switch
    {
        Themes.dark => new(0.16f, 0.16f, 0.16f, 1f),
        _ => new(0.97f, 0.97f, 0.97f, 1f)
    };

    internal void set() => PlayerPrefs.SetInt("theme", (int) theme);
}

