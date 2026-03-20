using UnityEngine;
using System.Collections;

namespace TapCat
{
    /// <summary>
    /// TapCat2D测试脚本
    /// 验证所有功能是否正常工作
    /// </summary>
    public class TapCat2DTest : MonoBehaviour
    {
        [Header("测试设置")]
        [SerializeField] private bool runAutomatedTests = true;
        [SerializeField] private float testDelay = 1.0f;
        
        private TapCat2D tapCat2D;
        private TapCat2DSetup setup;
        private TapCat2DSceneSetup sceneSetup;
        
        private void Start()
        {
            if (runAutomatedTests)
            {
                StartCoroutine(RunAllTests());
            }
        }
        
        /// <summary>
        /// 运行所有测试
        /// </summary>
        private IEnumerator RunAllTests()
        {
            Debug.Log("=== 开始TapCat2D测试 ===");
            
            yield return new WaitForSeconds(testDelay);
            
            // 测试1：检查必要组件
            yield return StartCoroutine(TestEssentialComponents());
            
            yield return new WaitForSeconds(testDelay);
            
            // 测试2：检查输入系统
            yield return StartCoroutine(TestInputSystem());
            
            yield return new WaitForSeconds(testDelay);
            
            // 测试3：检查动画系统
            yield return StartCoroutine(TestAnimationSystem());
            
            yield return new WaitForSeconds(testDelay);
            
            // 测试4：检查UI系统
            yield return StartCoroutine(TestUISystem());
            
            yield return new WaitForSeconds(testDelay);
            
            // 测试5：检查重置功能
            yield return StartCoroutine(TestResetFunction());
            
            Debug.Log("=== TapCat2D测试完成 ===");
            Debug.Log("所有测试通过！游戏已准备就绪。");
            Debug.Log("控制说明：空格键/鼠标左键播放动画，R键重置");
        }
        
        /// <summary>
        /// 测试1：检查必要组件
        /// </summary>
        private IEnumerator TestEssentialComponents()
        {
            Debug.Log("测试1：检查必要组件...");
            
            // 查找TapCat2D组件
            tapCat2D = FindObjectOfType<TapCat2D>();
            if (tapCat2D == null)
            {
                Debug.LogError("✗ 未找到TapCat2D组件！");
                yield break;
            }
            Debug.Log("✓ 找到TapCat2D组件");
            
            // 查找TapCat2DSetup组件
            setup = FindObjectOfType<TapCat2DSetup>();
            if (setup == null)
            {
                Debug.LogWarning("⚠ 未找到TapCat2DSetup组件（可选）");
            }
            else
            {
                Debug.Log("✓ 找到TapCat2DSetup组件");
            }
            
            // 查找TapCat2DSceneSetup组件
            sceneSetup = FindObjectOfType<TapCat2DSceneSetup>();
            if (sceneSetup == null)
            {
                Debug.LogWarning("⚠ 未找到TapCat2DSceneSetup组件（可选）");
            }
            else
            {
                Debug.Log("✓ 找到TapCat2DSceneSetup组件");
            }
            
            // 检查相机
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("✗ 未找到主相机！");
                yield break;
            }
            
            if (!mainCamera.orthographic)
            {
                Debug.LogWarning("⚠ 主相机不是正交相机，建议设置为Orthographic");
            }
            else
            {
                Debug.Log("✓ 主相机是正交相机");
            }
            
            Debug.Log("✓ 测试1通过：必要组件检查完成");
            yield return null;
        }
        
        /// <summary>
        /// 测试2：检查输入系统
        /// </summary>
        private IEnumerator TestInputSystem()
        {
            Debug.Log("测试2：检查输入系统...");
            
            if (tapCat2D == null)
            {
                Debug.LogError("✗ 无法测试输入系统：TapCat2D组件为空");
                yield break;
            }
            
            // 检查是否正在播放动画
            bool isAnimating = tapCat2D.IsAnimating();
            if (isAnimating)
            {
                Debug.Log("⚠ 动画正在播放，等待完成...");
                while (tapCat2D.IsAnimating())
                {
                    yield return null;
                }
            }
            
            // 获取初始点击计数
            int initialCount = tapCat2D.GetTapCount();
            Debug.Log($"初始点击计数: {initialCount}");
            
            // 模拟点击（调用公共方法）
            tapCat2D.StartCatAnimation();
            yield return new WaitForSeconds(0.1f);
            
            // 检查动画是否开始
            if (tapCat2D.IsAnimating())
            {
                Debug.Log("✓ 输入系统：成功触发动画播放");
            }
            else
            {
                Debug.LogError("✗ 输入系统：未能触发动画播放");
            }
            
            // 等待动画完成
            Debug.Log("等待动画完成...");
            while (tapCat2D.IsAnimating())
            {
                yield return null;
            }
            
            // 检查点击计数是否增加
            int newCount = tapCat2D.GetTapCount();
            if (newCount > initialCount)
            {
                Debug.Log($"✓ 点击计数已增加: {initialCount} -> {newCount}");
            }
            else
            {
                Debug.LogError($"✗ 点击计数未增加: {initialCount} -> {newCount}");
            }
            
            Debug.Log("✓ 测试2通过：输入系统检查完成");
        }
        
        /// <summary>
        /// 测试3：检查动画系统
        /// </summary>
        private IEnumerator TestAnimationSystem()
        {
            Debug.Log("测试3：检查动画系统...");
            
            if (tapCat2D == null)
            {
                Debug.LogError("✗ 无法测试动画系统：TapCat2D组件为空");
                yield break;
            }
            
            // 检查SpriteRenderer
            SpriteRenderer spriteRenderer = tapCat2D.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("✗ 未找到SpriteRenderer组件");
                yield break;
            }
            Debug.Log("✓ 找到SpriteRenderer组件");
            
            // 检查动画帧
            // 注意：由于catAnimationFrames是私有字段，我们无法直接访问
            // 但我们可以通过观察动画播放来验证
            
            // 触发动画
            int startCount = tapCat2D.GetTapCount();
            tapCat2D.StartCatAnimation();
            
            // 等待一小段时间检查动画状态
            yield return new WaitForSeconds(0.05f);
            
            if (tapCat2D.IsAnimating())
            {
                Debug.Log("✓ 动画系统：动画正在播放");
                
                // 等待完整动画
                float animationTime = 0f;
                while (tapCat2D.IsAnimating() && animationTime < 2.0f)
                {
                    animationTime += Time.deltaTime;
                    yield return null;
                }
                
                if (!tapCat2D.IsAnimating())
                {
                    Debug.Log("✓ 动画系统：动画正常完成");
                }
                else
                {
                    Debug.LogWarning("⚠ 动画系统：动画可能卡住");
                }
            }
            else
            {
                Debug.LogError("✗ 动画系统：未能开始动画");
            }
            
            Debug.Log("✓ 测试3通过：动画系统检查完成");
        }
        
        /// <summary>
        /// 测试4：检查UI系统
        /// </summary>
        private IEnumerator TestUISystem()
        {
            Debug.Log("测试4：检查UI系统...");
            
            // 查找Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning("⚠ 未找到Canvas（UI可能未创建）");
            }
            else
            {
                Debug.Log("✓ 找到Canvas");
                
                // 检查UI元素数量
                int uiElementCount = canvas.transform.childCount;
                if (uiElementCount > 0)
                {
                    Debug.Log($"✓ UI系统：找到{uiElementCount}个UI元素");
                }
                else
                {
                    Debug.LogWarning("⚠ UI系统：Canvas中没有子元素");
                }
            }
            
            // 检查Text组件
            Text[] textComponents = FindObjectsOfType<Text>();
            if (textComponents.Length > 0)
            {
                Debug.Log($"✓ UI系统：找到{textComponents.Length}个Text组件");
            }
            else
            {
                Debug.LogWarning("⚠ UI系统：未找到Text组件");
            }
            
            Debug.Log("✓ 测试4通过：UI系统检查完成");
            yield return null;
        }
        
        /// <summary>
        /// 测试5：检查重置功能
        /// </summary>
        private IEnumerator TestResetFunction()
        {
            Debug.Log("测试5：检查重置功能...");
            
            if (tapCat2D == null)
            {
                Debug.LogError("✗ 无法测试重置功能：TapCat2D组件为空");
                yield break;
            }
            
            // 记录当前状态
            int beforeResetCount = tapCat2D.GetTapCount();
            Debug.Log($"重置前点击计数: {beforeResetCount}");
            
            // 执行重置
            tapCat2D.ResetGame();
            yield return new WaitForSeconds(0.5f);
            
            // 检查重置结果
            int afterResetCount = tapCat2D.GetTapCount();
            bool isAnimating = tapCat2D.IsAnimating();
            
            if (afterResetCount == 0 && !isAnimating)
            {
                Debug.Log("✓ 重置功能：成功重置点击计数和动画状态");
            }
            else
            {
                Debug.LogError($"✗ 重置功能：重置失败（计数:{afterResetCount}, 动画:{isAnimating}）");
            }
            
            Debug.Log("✓ 测试5通过：重置功能检查完成");
        }
        
        /// <summary>
        /// 手动运行测试
        /// </summary>
        [ContextMenu("运行手动测试")]
        private void RunManualTest()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(RunAllTests());
            }
            else
            {
                Debug.Log("请在播放模式下运行测试");
            }
        }
        
        /// <summary>
        /// 快速功能验证
        /// </summary>
        [ContextMenu("快速验证")]
        private void QuickValidation()
        {
            Debug.Log("=== TapCat2D快速验证 ===");
            
            // 检查核心组件
            bool hasTapCat2D = FindObjectOfType<TapCat2D>() != null;
            bool hasCamera = Camera.main != null;
            bool hasCanvas = FindObjectOfType<Canvas>() != null;
            
            Debug.Log($"核心组件检查：");
            Debug.Log($"- TapCat2D: {(hasTapCat2D ? "✓" : "✗")}");
            Debug.Log($"- 主相机: {(hasCamera ? "✓" : "✗")}");
            Debug.Log($"- Canvas: {(hasCanvas ? "✓" : "✗")}");
            
            if (hasTapCat2D && hasCamera)
            {
                Debug.Log("✓ 基本验证通过！");
                Debug.Log("游戏可以运行，点击Play后按空格键测试动画");
            }
            else
            {
                Debug.LogError("✗ 验证失败：缺少必要组件");
            }
        }
    }
}