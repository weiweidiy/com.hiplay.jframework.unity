using UnityEngine.Events;

[System.Serializable]
public class AdvancedButtonEvents
{
    public UnityEvent onClick = new UnityEvent();
    public UnityEvent onLongPressStart = new UnityEvent();
    public UnityEvent onLongPressEnd = new UnityEvent();
    public UnityEvent onLongPressComplete = new UnityEvent(); // 新增：长按完成事件
}

//public AdvancedButton myButton;

//private void Start()
//{
//    myButton.advancedEvents.onClicked.AddListener(OnDeployUnitClick);
//    myButton.advancedEvents.onLongPressStart.AddListener(OnLongPressStart);
//    myButton.advancedEvents.onLongPressEnd.AddListener(OnLongPressEnd);
//    myButton.advancedEvents.onLongPressComplete.AddListener(OnLongPressComplete);
//}

//private void OnDeployUnitClick()
//{
//    Debug.Log("按钮被点击");
//}

//private void OnLongPressStart()
//{
//    Debug.Log("长按开始");
//}

//private void OnLongPressEnd()
//{
//    Debug.Log("长按结束");
//}

//private void OnLongPressComplete()
//{
//    Debug.Log("长按完成（达到指定时长）");
//}