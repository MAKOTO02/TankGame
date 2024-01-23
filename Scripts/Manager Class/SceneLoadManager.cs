using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private int presentStage;
    private int beforeStage;

    // Start is called before the first frame update
    protected override void Awake()
    {
        presentStage = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        presentStage = GameManager.Stage;
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
                GameManager.pausedForWating = true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
