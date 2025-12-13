using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button startBtn;   // 进入游戏按钮
    public Button settingBtn;// 设置按钮
    public Button quitBtn;   // 退出游戏按钮

    void Start()
    {
        // 绑定按钮点击事件（新手无需手动绑定）
        startBtn?.onClick.AddListener(StartGame);
        settingBtn?.onClick.AddListener(OpenSetting);
        quitBtn?.onClick.AddListener(QuitGame);
    }

    // 进入游戏（替换场景名为你的游戏场景）
    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // 打开设置（后续可直接加设置面板逻辑）
    void OpenSetting()
    {
        Debug.Log("打开设置面板");
    }

    // 退出游戏（打包后生效，编辑器中仅打印日志）
    void QuitGame()
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }
}