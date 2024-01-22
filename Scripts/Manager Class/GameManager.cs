using System.Collections;
using UnityEngine;

public enum UIState
{
    InGame,
    Paused,
    GameOver,
    MainMenu,
    Setting
}
public class GameManager : Singleton<GameManager>
{
    public GameObject inGameUI;
    public GameObject pausedUI;
    public GameObject gameOverUI;
    public GameObject mainMenuUi;
    public GameObject settingUI;

    public bool GameIsPlaying { get; private set; }
    public bool paused;

    private static int stage;
    public bool pausedForWating;

    IEnumerator WaitForLoadScene()
    {
        while (true)
        {
            if (pausedForWating)
            {
                // showLoadingImage();
                yield return new WaitForSeconds(3.0f);
                pausedForWating = false;
            }
            else
            {
                yield return null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stage = 0;
        Time.timeScale = 0.0f;
        ShowMainMenu();
        pausedForWating = false;
        StartCoroutine(WaitForLoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (false)
        {

        }
    }

    void ShowUI(UIState uiState)
    {
        GameObject[] allUI = { inGameUI, pausedUI, gameOverUI, mainMenuUi, settingUI };

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
            case UIState.Setting:
                settingUI.SetActive(true);
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
        Time.timeScale = 1.0f;
        stage = 1;
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

    public void ShowSettingMenu()
    {
        ShowUI(UIState.Setting);
        GameIsPlaying = false;
        Time.timeScale = 0.0f;
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //ゲームプレイ終了
#else
    Application.Quit();
#endif
    }

    public int GetStage()
    {
        return stage;
    }
}
