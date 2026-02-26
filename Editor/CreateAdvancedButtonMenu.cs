using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using JFramework.Unity;
using UnityEngine.EventSystems;

public static class CreateAdvancedButtonMenu
{
    [MenuItem("GameObject/UI/AdvancedButton", false, 10)]
    private static void CreateAdvancedButton(MenuCommand menuCommand)
    {
        // 查找或创建 Canvas
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas", typeof(Canvas));
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(canvasGO, "Create Canvas");
        }

        // 查找或创建 EventSystem
        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Undo.RegisterCreatedObjectUndo(es, "Create EventSystem");
        }

        // 创建 AdvancedButton 对象
        GameObject buttonGO = new GameObject("AdvancedButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(AdvancedButton));
        buttonGO.transform.SetParent(canvas.transform, false);

        // 设置默认图片
        Image image = buttonGO.GetComponent<Image>();
        image.color = Color.white;

        // 设置 RectTransform 大小
        RectTransform rect = buttonGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(160, 40);

        // 添加文本
        GameObject textGO = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textGO.transform.SetParent(buttonGO.transform, false);
        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text text = textGO.GetComponent<Text>();
        text.text = "AdvancedButton";
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // 选中新建的按钮
        Selection.activeGameObject = buttonGO;

        // 支持右键在指定对象下创建
        GameObject context = menuCommand.context as GameObject;
        if (context != null && context.transform is RectTransform)
        {
            buttonGO.transform.SetParent(context.transform, false);
        }

        Undo.RegisterCreatedObjectUndo(buttonGO, "Create AdvancedButton");
    }
}