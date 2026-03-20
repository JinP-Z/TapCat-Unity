using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace TapCat
{
    /// <summary>
    /// TapCat2D场景自动设置
    /// 确保场景在打开时自动配置为可运行状态
    /// </summary>
    [ExecuteInEditMode]
    public class TapCat2DSceneSetup : MonoBehaviour
    {
        [Header("场景设置")]
        [SerializeField] private bool autoSetupOnPlay = true;
        [SerializeField] private bool createPlaceholderSprites = true;
        
        [Header("猫咪动画帧占位图")]
        [SerializeField] private Texture2D[] placeholderTextures;
        
        private void Start()
        {
            if (Application.isPlaying && autoSetupOnPlay)
            {
                SetupSceneForPlay();
            }
        }
        
        /// <summary>
        /// 为播放设置场景
        /// </summary>
        private void SetupSceneForPlay()
        {
            Debug.Log("开始设置TapCat2D场景...");
            
            // 检查是否已有TapCat2DSetup组件
            TapCat2DSetup existingSetup = FindObjectOfType<TapCat2DSetup>();
            if (existingSetup == null)
            {
                // 创建设置对象
                GameObject setupObj = new GameObject("TapCat2D_Setup");
                setupObj.AddComponent<TapCat2DSetup>();
                Debug.Log("已创建TapCat2D设置对象");
            }
            
            // 检查是否已有猫咪动画帧
            CheckForCatAnimationFrames();
            
            Debug.Log("TapCat2D场景设置完成！");
            Debug.Log("控制说明：");
            Debug.Log("1. 按空格键或鼠标左键：播放猫咪动画");
            Debug.Log("2. 按R键：重置点击计数");
            Debug.Log("3. 帧率：0.1秒/帧 (10 FPS)");
            Debug.Log("4. 完全2D Sprite系统，无3D元素");
        }
        
        /// <summary>
        /// 检查猫咪动画帧
        /// </summary>
        private void CheckForCatAnimationFrames()
        {
            // 查找所有SpriteRenderer
            SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();
            bool hasCatSprites = false;
            
            foreach (var renderer in spriteRenderers)
            {
                if (renderer.sprite != null && 
                    (renderer.sprite.name.Contains("cat_anim") || 
                     renderer.sprite.name.Contains("CatAnimation")))
                {
                    hasCatSprites = true;
                    break;
                }
            }
            
            if (!hasCatSprites && createPlaceholderSprites)
            {
                Debug.LogWarning("未找到猫咪动画帧，将创建占位图");
                CreatePlaceholderSprites();
            }
            else if (hasCatSprites)
            {
                Debug.Log("已检测到猫咪动画帧");
            }
        }
        
        /// <summary>
        /// 创建占位图Sprite
        /// </summary>
        private void CreatePlaceholderSprites()
        {
            // 创建占位图对象
            GameObject placeholderObj = new GameObject("CatAnimation_Placeholder");
            SpriteRenderer renderer = placeholderObj.AddComponent<SpriteRenderer>();
            
            // 创建简单的占位图Sprite
            Texture2D tex = new Texture2D(64, 64);
            Color[] colors = new Color[64 * 64];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.cyan;
            }
            tex.SetPixels(colors);
            tex.Apply();
            
            Sprite placeholderSprite = Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
            renderer.sprite = placeholderSprite;
            
            // 添加TapCat2D组件
            TapCat2D tapCat2D = placeholderObj.AddComponent<TapCat2D>();
            
            // 创建占位动画帧数组
            Sprite[] placeholderSprites = new Sprite[10];
            for (int i = 0; i < 10; i++)
            {
                Texture2D frameTex = new Texture2D(64, 64);
                Color frameColor = Color.HSVToRGB(i / 10f, 0.8f, 1f);
                Color[] frameColors = new Color[64 * 64];
                for (int j = 0; j < frameColors.Length; j++)
                {
                    frameColors[j] = frameColor;
                }
                frameTex.SetPixels(frameColors);
                frameTex.Apply();
                
                placeholderSprites[i] = Sprite.Create(frameTex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
                placeholderSprites[i].name = $"cat_anim_{i:00}_placeholder";
            }
            
            tapCat2D.SetAnimationFrames(placeholderSprites);
            
            Debug.Log("已创建猫咪动画占位图，请替换为实际的cat_anim_00.png到cat_anim_09.png");
        }
        
        /// <summary>
        /// 在编辑器中验证场景
        /// </summary>
        [ContextMenu("验证场景设置")]
        private void ValidateSceneSetup()
        {
            Debug.Log("开始验证TapCat2D场景设置...");
            
            // 检查必要组件
            bool hasTapCat2D = FindObjectOfType<TapCat2D>() != null;
            bool hasTapCat2DSetup = FindObjectOfType<TapCat2DSetup>() != null;
            bool hasCamera = Camera.main != null;
            
            Debug.Log($"场景检查结果：");
            Debug.Log($"- TapCat2D组件: {(hasTapCat2D ? "✓" : "✗")}");
            Debug.Log($"- TapCat2DSetup组件: {(hasTapCat2DSetup ? "✓" : "✗")}");
            Debug.Log($"- 主相机: {(hasCamera ? "✓" : "✗")}");
            
            if (!hasTapCat2D || !hasTapCat2DSetup)
            {
                Debug.LogWarning("场景缺少必要组件，建议运行自动设置");
            }
            else
            {
                Debug.Log("场景设置验证通过！");
            }
        }
        
        /// <summary>
        /// 在编辑器中运行自动设置
        /// </summary>
        [ContextMenu("运行自动设置")]
        private void RunAutoSetup()
        {
            if (Application.isPlaying)
            {
                SetupSceneForPlay();
            }
            else
            {
                Debug.Log("自动设置将在播放模式下运行");
                #if UNITY_EDITOR
                EditorUtility.DisplayDialog("TapCat2D 设置", 
                    "自动设置已准备就绪。点击Play按钮后，场景将自动配置为可运行状态。\n\n" +
                    "确保将cat_anim_00.png到cat_anim_09.png放入Assets/Sprites/CatAnimation/目录。", 
                    "确定");
                #endif
            }
        }
        
        /// <summary>
        /// 创建示例场景
        /// </summary>
        [ContextMenu("创建完整示例场景")]
        private void CreateExampleScene()
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                // 保存当前场景
                if (EditorSceneManager.GetActiveScene().isDirty)
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }
                
                // 创建新场景
                EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                
                // 添加场景设置组件
                GameObject sceneSetupObj = new GameObject("SceneSetup");
                sceneSetupObj.AddComponent<TapCat2DSceneSetup>();
                
                // 添加游戏设置组件
                GameObject gameSetupObj = new GameObject("GameSetup");
                gameSetupObj.AddComponent<TapCat2DSetup>();
                
                Debug.Log("示例场景创建完成！");
                Debug.Log("请将cat_anim_00.png到cat_anim_09.png放入Assets/Sprites/CatAnimation/目录");
                Debug.Log("然后点击Play按钮即可运行游戏");
            }
            else
            {
                Debug.Log("请在编辑模式下创建示例场景");
            }
            #endif
        }
        
        /// <summary>
        /// 显示控制说明
        /// </summary>
        [ContextMenu("显示控制说明")]
        private void ShowControls()
        {
            string controls = @"
            TapCat 2D 动画游戏控制说明：
            ==============================
            1. 播放动画：
               - 按空格键
               - 或点击鼠标左键
               - 每次点击播放完整10帧动画
            
            2. 重置游戏：
               - 按R键
               - 重置点击计数和动画状态
            
            3. 技术规格：
               - 帧率：0.1秒/帧 (10 FPS)
               - 动画：10帧序列帧
               - 系统：纯2D Sprite，无3D
            
            4. 资源要求：
               - 10张PNG图片：cat_anim_00.png 到 cat_anim_09.png
               - 放置位置：Assets/Sprites/CatAnimation/
            
            5. 用户操作：
               - 零配置：点击Play直接运行
               - 自动设置：场景自动配置
            ";
            
            Debug.Log(controls);
        }
    }
}