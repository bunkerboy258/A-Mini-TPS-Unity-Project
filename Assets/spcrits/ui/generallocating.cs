using UnityEngine;
using UnityEngine.UI;

public class generallocating : MonoBehaviour
{
    public float xoffect;
    public float yoffect;
    private RectTransform uiobject;

    void OnEnable()
    {
        uiobject = GetComponent<RectTransform>();
        float fixedX = Screen.width * xoffect;
        float fixedY = Screen.height * yoffect;
        //
        uiobject.anchoredPosition = new Vector2(fixedX, fixedY);
    }

    void Update()
    {
    }
}