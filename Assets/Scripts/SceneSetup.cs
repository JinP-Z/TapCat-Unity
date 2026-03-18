using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace TapCat
{
    /// <summary>
    /// 场景设置工具 - 简化版本
    /// 在编辑器中一键设置TapCat场景
    /// </summary>
    public class SceneSetup : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("Tools/TapCat/快速设置场景")]
        public static void QuickSetupScene()
        {
            Debug.Log("开始快速设置TapCat场景...");
            
            // 1. 创建TapCat对象
            GameObject tapCat = CreateTapCatObject();
            
            // 2. 创建UI
            CreateUI();
            
            // 3. 保存场景
            SaveScene();
            
            Debug.Log("场景快速设置完成！");
            Debug.Log("请手动添加Animator Controller到TapCat对象的Animator组件");
        }
        
        private static GameObject CreateTapCatObject()
        {
            // 查找或创建TapCat对象
            GameObject tapCat = GameObject.Find("TapCat");
            if (tapCat == null)
            {
                tapCat = new GameObject("TapCat");
                tapCat.transform.position = Vector3.zero;
                
                // 添加SpriteRenderer
                SpriteRenderer spriteRenderer = tapCat.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 1;
                spriteRenderer.color = Color.white;
                
                // 添加Animator
                Animator animator = tapCat.AddComponent<Animator>();
                
                // 添加脚本组件
                tapCat.AddComponent<TapCatController>();
                tapCat.AddComponent<AnimationManager>();
                tapCat.AddComponent<InputHandler>();
                
                Debug.Log("创建TapCat游戏对象");
            }
            else
            {
                Debug.Log("找到已存在的TapCat对象");
            }
            
            return tapCat;
        }
        
        private static void CreateUI()
        {
            // 查找或创建Canvas
            Canvas canvas = FindOrCreateCanvas();
            
            // 创建UI容器
            GameObject uiContainer = new GameObject("CounterUI");
            uiContainer.transform.SetParent(canvas.transform);
            
            // 添加RectTransform
            RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);
            rectTransform.anchoredPosition = new Vector2(-20, -20);
            rectTransform.sizeDelta = new Vector2(200, 80);
            
            // 添加背景
            GameObject background = new GameObject("Background");
            background.transform.SetParent(uiContainer.transform);
            RectTransform bgRect = background.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            
            // 添加点击次数文本
            GameObject countTextObj = new GameObject("TapCountText");
            countTextObj.transform.SetParent(uiContainer.transform);
            RectTransform countRect = countTextObj.AddComponent<RectTransform>();
            countRect.anchorMin = new Vector2(0, 0.5f);
            countRect.anchorMax = new Vector2(1, 0.8f);
            countRect.anchoredPosition = Vector2.zero;
            countRect.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI countText = countTextObj.AddComponent<TextMeshProUGUI>();
            countText.text = "点击次数: 0";
            countText.fontSize = 24;
            countText.color = Color.white;
            countText.alignment = TextAlignmentOptions.Center;
            
            // 添加状态文本
            GameObject statusTextObj = new GameObject("StatusText");
            statusTextObj.transform.SetParent(uiContainer.transform);
            RectTransform statusRect = statusTextObj.AddComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0, 0.2f);
            statusRect.anchorMax = new Vector2(1, 0.5f);
            statusRect.anchoredPosition = Vector2.zero;
            statusRect.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI statusText = statusTextObj.AddComponent<TextMeshProUGUI>();
            statusText.text = "状态: 等待输入...";
            statusText.fontSize = 18;
            statusText.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            statusText.alignment = TextAlignmentOptions.Center;
            
            // 添加CounterUI组件
            CounterUI counterUI = uiContainer.AddComponent<CounterUI>();
            
            Debug.Log("UI创建完成");
        }
        
        private static Canvas FindOrCreateCanvas()
        {
            Canvas canvas = Object.FindObjectOfType<Canvas>();
            
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                
                Debug.Log("创建Canvas");
            }
            else
            {
                Debug.Log("找到已存在的Canvas");
            }
            
            return canvas;
        }
        
        private static void SaveScene()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                // 标记场景为脏（已修改）
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                
                // 保存场景
                bool saveSuccess = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                if (saveSuccess)
                {
                    Debug.Log("场景已保存");
                }
                else
                {
                    Debug.LogWarning("场景保存失败，请手动保存");
                }
            }
            #endif
        }
        
        [MenuItem("Tools/TapCat/进入测试模式")]
        public static void EnterTestMode()
        {
            // 先快速设置场景
            QuickSetupScene();
            
            // 然后进入播放模式
            EditorApplication.ExecuteMenuItem("Edit/Play");
            
            Debug.Log("进入测试模式...");
            Debug.Log("测试说明：");
            Debug.Log("1. 按空格键或点击猫咪进行点击");
            Debug.Log("2. 按R键重置计数");
            Debug.Log("3. 按D键切换舞蹈状态");
        }
        
        [MenuItem("Tools/TapCat/创建占位精灵")]
        public static void CreatePlaceholderSprite()
        {
            // 创建简单的占位精灵
            Texture2D texture = new Texture2D(64, 64);
            
            // 绘制简单的猫咪轮廓
            Color[] pixels = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    // 简单的圆形轮廓
                    float centerX = 32;
                    float centerY = 32;
                    float radius = 30;
                    
                    float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                    
                    if (distance < radius)
                    {
                        pixels[y * 64 + x] = new Color(1, 0.5f, 0, 1); // 橙色
                    }
                    else
                    {
                        pixels[y * 64 + x] = Color.clear;
                    }
                }
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            // 保存为资产
            #if UNITY_EDITOR
            string path = "Assets/Sprites/PlaceholderCat.png";
            System.IO.File.WriteAllBytes(path, texture.EncodeToPNG());
            AssetDatabase.Refresh();
            Debug.Log($"占位精灵已创建: {path}");
            #endif
        }
#endif
    }
}