using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager> {

    [Header("UI")]
   
  
    [SerializeField] private GameObject levelEndUI;
    [SerializeField] private GameObject settingUI;

    [SerializeField] public GameObject MovesBox;
    [SerializeField] public GameObject topPanel;

    private Text moveBoxtext;
    public Views views;

    public GameObject handIcon;
    private GridManager manager => GridManager.Instance;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI move;

    void Start()
    {
        handIcon.transform.DOLocalMoveY(-1160f, 1.3f).SetEase(Ease.InOutBack);
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) handIcon.transform.DOLocalMoveY(-2580f, 1.2f).SetEase(Ease.OutBack);

        textScore.text = " " + manager.score;
        move.text = " " + PlayerManager.Instance.moveCounter;

    }

    public void tappedPlay()
    {
        setTransition(Views.game); 
    }


    public void Setting()
    {
        settingUI.SetActive(true);
    }

    public void CloseSetting()
    {
        settingUI.SetActive(false);
    }

    public void RestartGames()
    {
        SceneManager.LoadScene(0);
    }

    public void tappedMainMenu()
    {
       
        Taptic.Medium();
        setTransition(Views.mainMenu);

    }

    private void goToMainMenu()
    {
      
       // gameplayUI.SetActive(false);
    
    }

    public void toggleGameView()
    {
        setTransition(Views.game);  
    }

    public void setTransition(Views transition)
    {

      //    gameplayUI.SetActive(false);
            levelEndUI.SetActive(false);

            switch (transition)
            {
                case Views.mainMenu:
                    goToMainMenu();
                    break;
                case Views.game:
                    StartGame();
                    break;
                case Views.map:
                    //ShowMap();
                    break;
                case Views.leaderboard:
                    ShowLevelEnd();
                    break;
                case Views.tutorial:
                    StartTutorial();
                    break;
            }
           
    }

    private void StartGame()
    {
        //state = LevelState.playing;

        GridManager.Instance.gameObject.SetActive(true);
        PlayerManager.Instance.gameObject.SetActive(true);
        PlayerManager.Instance.transform.position = new Vector3(-1f,-1f,0f);

        GridManager.Instance.IntGrid();
        
        UIManager.Instance.topPanel.SetActive(true);
        ScoreManager.Instance.Reset();
       
        

        //GridManager.Instance.Reset();
    }

    private void StartTutorial()
    {
        
    }

    private void ShowLevelEnd()
    {
        levelEndUI.SetActive(true);
        LeaderboardManager.Instance.Enter();
    }


    public void MovesBoxEnter()
    {
        //if (GameManager.Instance.level.level <= 3) return;
        MovesBox.transform.DOLocalMoveY(GameManager.Instance.aspect ? -7 : 10, 0.5f).SetEase(Ease.OutSine);
        
    }

    public void MovesBoxExit()
    {
        MovesBox.transform.DOLocalMoveY(800, 0.5f).SetEase(Ease.InSine);
    }
}


public enum Views { mainMenu, game, map, leaderboard, tutorial }