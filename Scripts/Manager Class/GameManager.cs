using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static bool gameIsPlaying { get; private set; }
    static public bool paused;
    static public bool pausedForWating;
    static public int Stage { get; private set; }

    private static int remainingEnemys;
    private static int lastStage = 2;
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
        Stage = 0;
        Time.timeScale = 0.0f;
        ShowMainMenu();
        pausedForWating = false;
        StartCoroutine(WaitForLoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        remainingEnemys = GameObject.FindGameObjectsWithTag("Enemy").Length;
        static bool isLastStage(int stage) { return stage == lastStage; }    // 仮のメソッド;
        ManageStage(isLastStage);
    }

    delegate bool Predicate(int stage);
    void ManageStage(Predicate predicate)
    {
        if(remainingEnemys　== 0)
        {
            if (!predicate(Stage))
            {
                Stage++;
                // 敵の数を配列で保存しておくなら
                // 敵の数を次のステージのものに更新する.
            }
            else
            {
                // ゲームクリア!!!
                // おめでておうございます.
                SceneManager.UnloadSceneAsync("stage" +　lastStage.ToString());
                ShowMainMenu();
            }
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
        gameIsPlaying = false;
    }

    public void StartGame()
    {
        ShowUI(UIState.InGame);
        gameIsPlaying = true;
        Time.timeScale = 1.0f;
        Stage = 1;
    }

    public void GameOver()
    {
        ShowUI(UIState.GameOver);
        gameIsPlaying = false;
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
        gameIsPlaying = false;
        Time.timeScale = 0.0f;
    }

    static private void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //ゲームプレイ終了
#else
    Application.Quit();
#endif
    }
    static public bool IsWaiting()
    {
        return paused || pausedForWating;
    }
}
