using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class gamemanager : MonoBehaviour
{
    public static gamemanager Instance { get; private set; }

    public enum GameState
    {
        Idle,
        Playing,
        Paused,
        GameOver
    }
    [Header("计时配置")]
    public float totalGameTime;
    private bool isTiming;
    [Header("当前游戏状态")]
    public GameState currentState = GameState.Idle;
    //
    public survival survival1;
    public survival survival2;
    public survival survival3;
    public survival survival4;
    //
    public int grade;
    public int zombienum;
    //
    private bool escin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateGameCoreLogic();
        updatesettingsui();
    }

    private void UpdateGameCoreLogic()
    {
        if (isTiming && currentState == GameState.Playing)
        {
            totalGameTime += Time.deltaTime;
        }
    }

    private void updatesettingsui()
    {
        escin = InputRecorder.Instance.ESCDOWN;
        if (currentState != GameState.Paused&&escin)
        {
            PauseGame();
            DisPlayControl.Instance.showsettingsui();
        }
    }

    #region 核心游戏控制方法
    public void StartGame()
    {
        if (currentState != GameState.Idle) return;
        currentState = GameState.Playing;
        totalGameTime = 0f;
        isTiming = true;
        DisPlayControl.Instance.StartGame();

        survival1.gameObject.SetActive(true); survival1.istiming = true;
        survival2.gameObject.SetActive(true); survival2.istiming = true;
        survival3.gameObject.SetActive(true); survival3.istiming = true;
        survival4.gameObject.SetActive(true); survival4.istiming = true;
    }

    public void EndGame()
    {
        if (currentState == GameState.GameOver) return;
        currentState = GameState.GameOver;
        isTiming = false;
        Debug.Log($"游戏结束！总时长：{totalGameTime:F2}秒");
    }

    public void PauseGame()
    {
        if (currentState != GameState.Playing) return;
        currentState = GameState.Paused;
        isTiming = false;
        Time.timeScale = 0f;
        Debug.Log("游戏暂停！计时暂停");
    }

    public void ResumeGame()
    {
        if (currentState != GameState.Paused) return;
        currentState = GameState.Playing;
        isTiming = true;
        Time.timeScale = 1f;
        Debug.Log("游戏恢复！计时继续");
    }

    public void QuitGame()
    {
        isTiming = false;
        Debug.Log("退出游戏");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void restatrt()
    {
        StartCoroutine(RestartSceneAfter5s());
    }
    #endregion
    public void addgrade(int x)
    {
        grade += x;
    }

    private IEnumerator RestartSceneAfter5s()
    {
        yield return new WaitForSeconds(5);
        string curScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(curScene);
    }
}
