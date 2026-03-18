using UnityEngine;
using UnityEditor;

namespace TapCat.Editor
{
    /// <summary>
    /// 猫咪显示修复编辑器工具
    /// </summary>
    public class CatFixerEditor : EditorWindow
    {
        private Color catColor = new Color(1f, 0.5f, 0f, 1f);
        private Vector3 catPosition = Vector3.zero;
        private Vector3 catScale = Vector3.one;
        private bool createUI = true;
        
        [MenuItem("Tools/TapCat/修复猫咪显示")]
        public static void ShowWindow()
        {
            GetWindow<CatFixerEditor>("猫咪显示修复工具");
        }
        
        void OnGUI()
        {
            GUILayout.Label("猫咪显示修复工具", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            // 猫咪设置
            EditorGUILayout.LabelField("猫咪设置", EditorStyles.boldLabel);
            catColor = EditorGUILayout.ColorField("猫咪颜色", catColor);
            catPosition = EditorGUILayout.Vector3Field("位置", catPosition);
            catScale = EditorGUILayout.Vector3Field("缩放", catScale);
            
            EditorGUILayout.Space();
            
            // UI设置
            EditorGUILayout.LabelField("UI设置", EditorStyles.boldLabel);
            createUI = EditorGUILayout.Toggle("创建UI", createUI);
            
            EditorGUILayout.Space(20);
            
            // 修复按钮
            if (GUILayout.Button("一键修复猫咪显示", GUILayout.Height(40)))
            {
                FixCatDisplay();
            }
            
            EditorGUILayout.Space();
            
            // 快速修复按钮
            if (GUILayout.Button("快速修复（使用默认设置）"))
            {
                QuickFix();
            }
            
            EditorGUILayout.Space();
            
            // 状态信息
            EditorGUILayout.HelpBox(
                "如果看不到猫咪，点击上面的按钮修复。\n" +
                "修复后会在场景中创建一个橙色方块作为猫咪。",
                MessageType.Info);
        }
        
        /// <summary>
        /// 修复猫咪显示
        /// </summary>
        private void FixCatDisplay()
        {
            // 确保在游戏运行时修复
            if (!Application.isPlaying)
            {
                EditorUtility.DisplayDialog("提示", "请先进入运行模式（点击Play按钮）", "确定");
                return;
            }
            
            // 查找或创建CatFixer组件
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
            
            // 设置参数（通过反射，因为字段是私有的）
            System.Type type = catFixer.GetType();
            var colorField = type.GetField("catColor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var positionField = type.GetField("catPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var scaleField = type.GetField("catScale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var createUIField = type.GetField("createUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (colorField != null) colorField.SetValue(catFixer, catColor);
            if (positionField != null) positionField.SetValue(catFixer, catPosition);
            if (scaleField != null) scaleField.SetValue(catFixer, catScale);
            if (createUIField != null) createUIField.SetValue(catFixer, createUI);
            
            // 调用修复方法
            catFixer.FixCatDisplay();
            
            Debug.Log("猫咪显示修复完成！");
            EditorUtility.DisplayDialog("成功", "猫咪显示修复完成！", "确定");
        }
        
        /// <summary>
        /// 快速修复
        /// </summary>
        private void QuickFix()
        {
            // 确保在游戏运行时修复
            if (!Application.isPlaying)
            {
                EditorUtility.DisplayDialog("提示", "请先进入运行模式（点击Play按钮）", "确定");
                return;
            }
            
            // 创建临时修复对象
            GameObject tempObject = new GameObject("TempCatFixer");
            CatFixer catFixer = tempObject.AddComponent<CatFixer>();
            catFixer.FixCatDisplay();
            
            // 销毁临时对象
            Object.DestroyImmediate(tempObject);
            
            Debug.Log("快速修复完成！");
            EditorUtility.DisplayDialog("成功", "快速修复完成！", "确定");
        }
    }
}