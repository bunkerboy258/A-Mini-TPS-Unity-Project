using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("³¡¾°ÅäÖÃ")]
    public string targetSceneName;
    [Header("UIÒýÓÃ")]
    public Slider loadProgressSlider;
    public TextMeshProUGUI text;

    private AsyncOperation asyncOperation;

    [System.Serializable]
    public class UIElementPosition
    {
        public RectTransform element;
        public float xOffset;
        public float yOffset;
        public bool updateInRealTime = true;
    }

    public UIElementPosition[] uiElements;
    private void Start()
    {
        text.text="Loading.......";
        loadProgressSlider.value = 0;
        StartCoroutine(LoadSceneAsyncCoroutine());
    }

    private void Update()
    {
        UpdateUIPositions();
    }
    private IEnumerator LoadSceneAsyncCoroutine()
    {
        asyncOperation = SceneManager.LoadSceneAsync(targetSceneName);
        asyncOperation.allowSceneActivation = false; 

        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f;
            progress = Mathf.Clamp01(progress);

            if (loadProgressSlider != null) loadProgressSlider.value = progress;

            if (progress >= 1f)
            {
                text.text = "Click to start";
                if (InputRecorder.Instance.MouseLeft) asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void UpdateUIPositions()
    {
        foreach (var uiElement in uiElements)
        {
            if (uiElement.element != null && uiElement.updateInRealTime)
            {
                float fixedX = Screen.width * uiElement.xOffset;
                float fixedY = Screen.height * uiElement.yOffset;
                uiElement.element.anchoredPosition = new Vector2(fixedX, fixedY);
            }
        }
    }
}