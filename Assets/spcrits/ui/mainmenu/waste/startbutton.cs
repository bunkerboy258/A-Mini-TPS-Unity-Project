using UnityEngine;
using UnityEngine.UI;
public class StartGameButton : MonoBehaviour
{
    private Button startBtn; 

    void Start()
    {
        startBtn = GetComponent<Button>();
        startBtn.onClick.AddListener(OnStartButtonClick);
    }

    void OnStartButtonClick()
    {
        DisPlayControl.Instance.StartGame();
    }
}