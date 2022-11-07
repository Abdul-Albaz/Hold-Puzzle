using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    public GameObject handIcon;
    private GridManager manager => GridManager.Instance;
    public TextMeshProUGUI textScore;

    void Start()
    {
        handIcon.transform.DOLocalMoveY(-1160f, 1.3f).SetEase(Ease.InOutBack);
    }

    void Update()
    {
        if(Input.GetMouseButton(0)) handIcon.transform.DOLocalMoveY(-2580f, 1.2f).SetEase(Ease.OutBack);

        textScore.text = "Score :  " + manager.score;

    }



    
}
