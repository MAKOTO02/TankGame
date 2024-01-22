using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private GameManager gameManager;
    private int presentStage;
    private int beforeStage;

    // Start is called before the first frame update
    protected override void Awake()
    {
        gameManager = GameManager.Instance;
        presentStage = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        presentStage = gameManager.GetStage();
        if(presentStage != beforeStage)
        {
            try
            {
                SceneManager.LoadScene("stage" + presentStage.ToString(),LoadSceneMode.Additive);
                if(beforeStage != 0)
                {
                    SceneManager.UnloadSceneAsync("stage" + beforeStage.ToString());
                }
                beforeStage = presentStage;
                gameManager.pausedForWating = true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
