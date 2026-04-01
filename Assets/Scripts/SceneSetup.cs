using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapCat.Animation;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace TapCat
{
    /// <summary>
    /// 鍦烘櫙璁剧疆宸ュ叿 - 绠€鍖栫増鏈?
    /// 鍦ㄧ紪杈戝櫒涓竴閿缃甌apCat鍦烘櫙
    /// </summary>
    public class SceneSetup : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("Tools/TapCat/Quick Setup Scene")]
        public static void QuickSetupScene()
        {
            Debug.Log("寮€濮嬪揩閫熻缃甌apCat鍦烘櫙...");
            
            // 1. 鍒涘缓TapCat瀵硅薄
            GameObject tapCat = CreateTapCatObject();
            
            // 2. 鍒涘缓UI
            CreateUI();
            
            // 3. 淇濆瓨鍦烘櫙
            SaveScene();
            
            Debug.Log("鍦烘櫙蹇€熻缃畬鎴愶紒");
            Debug.Log("璇锋墜鍔ㄦ坊鍔燗nimator Controller鍒癟apCat瀵硅薄鐨凙nimator缁勪欢");
        }
        
        private static GameObject CreateTapCatObject()
        {
            // 鏌ユ壘鎴栧垱寤篢apCat瀵硅薄
            GameObject tapCat = GameObject.Find("TapCat");
            if (tapCat == null)
            {
                tapCat = new GameObject("TapCat");
                tapCat.transform.position = Vector3.zero;
                
                // 娣诲姞SpriteRenderer
                SpriteRenderer spriteRenderer = tapCat.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 1;
                spriteRenderer.color = Color.white;
                
                // 娣诲姞Animator
                Animator animator = tapCat.AddComponent<Animator>();
                
                
                // 灏濊瘯鍔犺浇鐚挭鍥剧墖
                try
                {
                    // 鏂规硶1锛氫粠Resources鍔犺浇
                    Sprite catSprite = Resources.Load<Sprite>("Sprites/PlaceholderCat");
                    if (catSprite != null)
                    {
                        spriteRenderer.sprite = catSprite;
                        Debug.Log("鎴愬姛鍔犺浇鐚挭鍥剧墖");
                    }
                    else
                    {
                        // 鏂规硶2锛氬垱寤轰复鏃跺僵鑹叉柟鍧?
                        Debug.LogWarning("鐚挭鍥剧墖鏈壘鍒帮紝鍒涘缓涓存椂褰╄壊鏂瑰潡");
                        spriteRenderer.color = new Color(1f, 0.5f, 0f, 1f); // 姗欒壊
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("鍔犺浇鐚挭鍥剧墖澶辫触: " + e.Message);
                    spriteRenderer.color = Color.red; // 绾㈣壊浣滀负閿欒鎻愮ず
                }
                
                // 娣诲姞鑴氭湰缁勪欢
                tapCat.AddComponent<TapCatController>();
                tapCat.AddComponent<AnimationManager>();
                tapCat.AddComponent<InputHandler>();
                
                Debug.Log("鍒涘缓TapCat娓告垙瀵硅薄骞惰缃甋priteRenderer");
            }
            else
            {
                Debug.Log("鎵惧埌宸插瓨鍦ㄧ殑TapCat瀵硅薄");
            }
            
            return tapCat;
        }
        
        private static void CreateUI()
        {
            // 鏌ユ壘鎴栧垱寤篊anvas
            Canvas canvas = FindOrCreateCanvas();
            
            // 鍒涘缓UI瀹瑰櫒
            GameObject uiContainer = new GameObject("CounterUI");
            uiContainer.transform.SetParent(canvas.transform);
            
            // 娣诲姞RectTransform
            RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);
            rectTransform.anchoredPosition = new Vector2(-20, -20);
            rectTransform.sizeDelta = new Vector2(200, 80);
            
            // 娣诲姞鑳屾櫙
            GameObject background = new GameObject("Background");
            background.transform.SetParent(uiContainer.transform);
            RectTransform bgRect = background.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            
            // 娣诲姞鐐瑰嚮娆℃暟鏂囨湰
            GameObject countTextObj = new GameObject("TapCountText");
            countTextObj.transform.SetParent(uiContainer.transform);
            RectTransform countRect = countTextObj.AddComponent<RectTransform>();
            countRect.anchorMin = new Vector2(0, 0.5f);
            countRect.anchorMax = new Vector2(1, 0.8f);
            countRect.anchoredPosition = Vector2.zero;
            countRect.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI countText = countTextObj.AddComponent<TextMeshProUGUI>();
            countText.text = "鐐瑰嚮娆℃暟: 0";
            countText.fontSize = 24;
            countText.color = Color.white;
            countText.alignment = TextAlignmentOptions.Center;
            
            // 娣诲姞鐘舵€佹枃鏈?
            GameObject statusTextObj = new GameObject("StatusText");
            statusTextObj.transform.SetParent(uiContainer.transform);
            RectTransform statusRect = statusTextObj.AddComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0, 0.2f);
            statusRect.anchorMax = new Vector2(1, 0.5f);
            statusRect.anchoredPosition = Vector2.zero;
            statusRect.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI statusText = statusTextObj.AddComponent<TextMeshProUGUI>();
            statusText.text = "鐘舵€? 绛夊緟杈撳叆...";
            statusText.fontSize = 18;
            statusText.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            statusText.alignment = TextAlignmentOptions.Center;
            
            // 娣诲姞CounterUI缁勪欢
            CounterUI counterUI = uiContainer.AddComponent<CounterUI>();
            
            Debug.Log("UI鍒涘缓瀹屾垚");
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
                
                Debug.Log("鍒涘缓Canvas");
            }
            else
            {
                Debug.Log("鎵惧埌宸插瓨鍦ㄧ殑Canvas");
            }
            
            return canvas;
        }
        
        private static void SaveScene()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                // 鏍囪鍦烘櫙涓鸿剰锛堝凡淇敼锛?
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                
                // 淇濆瓨鍦烘櫙
                bool saveSuccess = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                if (saveSuccess)
                {
                    Debug.Log("Scene saved.");
                }
                else
                {
                    Debug.LogWarning("鍦烘櫙淇濆瓨澶辫触锛岃鎵嬪姩淇濆瓨");
                }
            }
            #endif
        }
        
        [MenuItem("Tools/TapCat/杩涘叆娴嬭瘯妯″紡")]
        public static void EnterTestMode()
        {
            // 鍏堝揩閫熻缃満鏅?
            QuickSetupScene();
            
            // 鐒跺悗杩涘叆鎾斁妯″紡
            EditorApplication.ExecuteMenuItem("Edit/Play");
            
            Debug.Log("杩涘叆娴嬭瘯妯″紡...");
            Debug.Log("Test instructions:");
            Debug.Log("1. Press Space or click the cat to tap.");
            Debug.Log("2. Press R to reset the counter.");
            Debug.Log("3. Press D to toggle dance mode.");
        }
        
        [MenuItem("Tools/TapCat/鍒涘缓鍗犱綅绮剧伒")]
        public static void CreatePlaceholderSprite()
        {
            // 鍒涘缓绠€鍗曠殑鍗犱綅绮剧伒
            Texture2D texture = new Texture2D(64, 64);
            
            // 缁樺埗绠€鍗曠殑鐚挭杞粨
            Color[] pixels = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    // 绠€鍗曠殑鍦嗗舰杞粨
                    float centerX = 32;
                    float centerY = 32;
                    float radius = 30;
                    
                    float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                    
                    if (distance < radius)
                    {
                        pixels[y * 64 + x] = new Color(1, 0.5f, 0, 1); // 姗欒壊
                    }
                    else
                    {
                        pixels[y * 64 + x] = Color.clear;
                    }
                }
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            // 淇濆瓨涓鸿祫浜?
            #if UNITY_EDITOR
            string path = "Assets/Sprites/PlaceholderCat.png";
            System.IO.File.WriteAllBytes(path, texture.EncodeToPNG());
            AssetDatabase.Refresh();
            Debug.Log($"鍗犱綅绮剧伒宸插垱寤? {path}");
            #endif
        }
#endif
    }
}
