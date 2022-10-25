using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject handIcon;


    void Start()
    {
        handIcon.transform.DOLocalMoveY(-1331.317f, 1.3f).SetEase(Ease.InOutBack);
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        handIcon.transform.DOLocalMoveY(-2478.317f, 1.2f).SetEase(Ease.OutBack);
    }


}
