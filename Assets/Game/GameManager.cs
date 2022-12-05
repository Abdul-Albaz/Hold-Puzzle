using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public enum LevelState { playing, won, lost, waiting }


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject platePrefab, holePrefab;
    [SerializeField] private Transform[] initialPlatePositions;
    [SerializeField] private Transform holeParent;
    [SerializeField] internal int numberOfCols, numberOfRows;
    [SerializeField] public static int moveCounter = 0;
    [SerializeField] public TextMeshProUGUI movesLeft;

    public LevelState state = LevelState.playing;


    
    public float radius = 1.05f, margin = 0.5f, minX, maxX, minZ, maxZ, scaleFactor;
    public int moves = 0;
    public int step = 0;

    public bool aspect => (Screen.height / Screen.width) > 1.78f;


    private void Start()
    {
        scaleFactor = 4f / numberOfCols;
        
        GenerateHoles();
        GeneratePlates();


        StarManager.Instance.enter();
        CoinManager.Instance.enter();
        UIManager.Instance.MovesBoxEnter();



    }

    private void GenerateHoles()
    {
        minX = -(numberOfCols * radius * scaleFactor + (numberOfCols+1f) / 2 * margin);
        minZ = -(numberOfRows * radius * scaleFactor + (numberOfRows + 1f) / 2 * margin);
        maxX = -minX;
        maxZ = -minZ;
        
        for (var z = 0; z < numberOfRows; z++)
        {
            for (var x = 0; x < numberOfCols; x++)
            {
                var xPos = minX + (2 * x + 1) * radius * scaleFactor + (x + 1) * margin * scaleFactor;
                var zPos = minZ + (2 * z ) * radius * scaleFactor + (z + 1) * margin * scaleFactor;
                var holeObject = Instantiate(holePrefab, new (xPos, 0.35f, zPos), Quaternion.identity);
                holeObject.transform.parent = holeParent;
                holeObject.transform.localScale = Vector3.zero;
                holeObject.transform.DOScale(Vector3.one * scaleFactor, 0.25f).SetEase(Ease.OutBack)
                    .SetDelay((z * numberOfCols + x) * 0.075f);
                //var hole = holeObject.GetComponent<Hole>();
                //hole.row = z;
                //hole.col = x;
                //holes[x, z] = hole;
            }
        }

        //holeParent.transform.localScale = Vector3.one * scaleFactor;
    }
    
    private void GeneratePlates()
    {

        for (var index = 0; index < initialPlatePositions.Length; index++)
        {
            
//            var plate = Instantiate(platePrefab, initialPlatePositions[index]).GetComponent<Plate>();
        //    plate.Enter(index);
        }

        
    }

    public void moveCompleted()
    {
        moves++;
        moveCounter++;

        if (moves % 3 == 0)
        {
            GeneratePlates();
            //StarManager.Instance.add(moveCounter);
            //UIManager.Instance.setTransition(Views.leaderboard);0
        }

        movesLeft.text = "" + moveCounter;


    }


}
