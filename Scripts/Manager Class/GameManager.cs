using UnityEngine;

public enum UIState
{
    InGame,
    Paused,
    GameOver,
    MainMenu
}
public class GameManager : Singleton<GameManager>
{
    public GameObject inGameUI;
    public GameObject pausedUI;
    public GameObject gameOverUI;
    public GameObject mainMenuUi;

    public bool GameIsPlaying { get; private set; }
    public bool paused;
    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowUI(UIState uiState)
    {
        GameObject[] allUI = { inGameUI, pausedUI, gameOverUI, mainMenuUi };

        foreach (GameObject ui in allUI)
        {
            ui.SetActive(false);
        }

        switch (uiState)
        {
            case UIState.InGame:
                inGameUI.SetActive(true);
                break;
            case UIState.Paused:
                pausedUI.SetActive(true);
                break;
            case UIState.GameOver:
                gameOverUI.SetActive(true);
                break;
            case UIState.MainMenu:
                mainMenuUi.SetActive(true);
                break;
        }
    }
    public void ShowMainMenu()
    {
        ShowUI(UIState.MainMenu);
        GameIsPlaying = false;
    }

    public void StartGame()
    {
        ShowUI(UIState.InGame);
        GameIsPlaying = true;
    }

    public void GameOver()
    {
        ShowUI(UIState.GameOver);
        GameIsPlaying = false;
    }

    public void SetPaused(bool paused)
    {
        inGameUI.SetActive(!paused);
        pausedUI.SetActive(paused);
        if(paused) { Time.timeScale = 0.0f; } else {  Time.timeScale = 1.0f; }
    }
}
