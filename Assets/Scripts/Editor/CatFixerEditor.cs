using UnityEngine;
using UnityEditor;

namespace TapCat.Editor
{
    /// <summary>
    /// 鐚挭鏄剧ず淇缂栬緫鍣ㄥ伐鍏?
    /// </summary>
    public class CatFixerEditor : EditorWindow
    {
        private Color catColor = new Color(1f, 0.5f, 0f, 1f);
        private Vector3 catPosition = Vector3.zero;
        private Vector3 catScale = Vector3.one;
        private bool createUI = true;
        
        [MenuItem("Tools/TapCat/淇鐚挭鏄剧ず")]
        public static void ShowWindow()
        {
            GetWindow<CatFixerEditor>("鐚挭鏄剧ず淇宸ュ叿");
        }
        
        void OnGUI()
        {
            GUILayout.Label("鐚挭鏄剧ず淇宸ュ叿", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            // 鐚挭璁剧疆
            EditorGUILayout.LabelField("鐚挭璁剧疆", EditorStyles.boldLabel);
            catColor = EditorGUILayout.ColorField("鐚挭棰滆壊", catColor);
            catPosition = EditorGUILayout.Vector3Field("浣嶇疆", catPosition);
            catScale = EditorGUILayout.Vector3Field("缂╂斁", catScale);
            
            EditorGUILayout.Space();
            
            // UI璁剧疆
            EditorGUILayout.LabelField("UI璁剧疆", EditorStyles.boldLabel);
            createUI = EditorGUILayout.Toggle("鍒涘缓UI", createUI);
            
            EditorGUILayout.Space(20);
            
            // 淇鎸夐挳
            if (GUILayout.Button("Fix Cat Display", GUILayout.Height(40)))
            {
                FixCatDisplay();
            }
            
            EditorGUILayout.Space();
            
            // 蹇€熶慨澶嶆寜閽?
            if (GUILayout.Button("Quick Fix (Use Defaults)"))
            {
                QuickFix();
            }
            
            EditorGUILayout.Space();
            
            // 鐘舵€佷俊鎭?
            EditorGUILayout.HelpBox(
                "If you cannot see the cat, click the button above to fix it.\\n" +
                "A temporary placeholder cat will be created in the scene.",
                MessageType.Info);
        }
        
        /// <summary>
        /// 淇鐚挭鏄剧ず
        /// </summary>
        private void FixCatDisplay()
        {
            // 纭繚鍦ㄦ父鎴忚繍琛屾椂淇
            if (!Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Hint", "Please enter Play Mode before running this.", "OK");
                return;
            }
            
            // 鏌ユ壘鎴栧垱寤篊atFixer缁勪欢
            GameObject fixerObject = GameObject.Find("CatFixer");
            if (fixerObject == null)
            {
                fixerObject = new GameObject("CatFixer");
            }
            
            CatFixer catFixer = fixerObject.GetComponent<CatFixer>();
            if (catFixer == null)
            {
                catFixer = fixerObject.AddComponent<CatFixer>();
            }
            
            // 璁剧疆鍙傛暟锛堥€氳繃鍙嶅皠锛屽洜涓哄瓧娈垫槸绉佹湁鐨勶級
            System.Type type = catFixer.GetType();
            var colorField = type.GetField("catColor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var positionField = type.GetField("catPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var scaleField = type.GetField("catScale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var createUIField = type.GetField("createUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (colorField != null) colorField.SetValue(catFixer, catColor);
            if (positionField != null) positionField.SetValue(catFixer, catPosition);
            if (scaleField != null) scaleField.SetValue(catFixer, catScale);
            if (createUIField != null) createUIField.SetValue(catFixer, createUI);
            
            // 璋冪敤淇鏂规硶
            catFixer.FixCatDisplay();
            
            Debug.Log("Cat display fix complete.");
            EditorUtility.DisplayDialog("Success", "Cat display fix complete.", "OK");
        }
        
        /// <summary>
        /// 蹇€熶慨澶?
        /// </summary>
        private void QuickFix()
        {
            // 纭繚鍦ㄦ父鎴忚繍琛屾椂淇
            if (!Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Hint", "Please enter Play Mode before running this.", "OK");
                return;
            }
            
            // 鍒涘缓涓存椂淇瀵硅薄
            GameObject tempObject = new GameObject("TempCatFixer");
            CatFixer catFixer = tempObject.AddComponent<CatFixer>();
            catFixer.FixCatDisplay();
            
            // 閿€姣佷复鏃跺璞?
            Object.DestroyImmediate(tempObject);
            
            Debug.Log("蹇€熶慨澶嶅畬鎴愶紒");
            EditorUtility.DisplayDialog("鎴愬姛", "蹇€熶慨澶嶅畬鎴愶紒", "纭畾");
        }
    }
}
