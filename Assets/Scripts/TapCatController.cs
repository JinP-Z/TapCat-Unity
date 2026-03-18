using UnityEngine;

namespace TapCat
{
    /// <summary>
    /// TapCat主控制器
    /// 负责处理猫咪的点击响应和状态管理
    /// </summary>
    public class TapCatController : MonoBehaviour
    {
        [Header("动画引用")]
        [SerializeField] private Animator catAnimator;
        
        [Header("点击设置")]
        [SerializeField] private int tapCount = 0;
        [SerializeField] private float tapCooldown = 0.2f;
        
        private float lastTapTime = 0f;
        private bool isTapDancing = false;
        
        /// <summary>
        /// 当猫咪被点击时调用
        /// </summary>
        public void OnCatTapped()
        {
            // 检查冷却时间
            if (Time.time - lastTapTime < tapCooldown)
                return;
            
            lastTapTime = Time.time;
            tapCount++;
            
            // 切换舞蹈状态
            if (!isTapDancing)
            {
                StartTapDance();
            }
            
            // 触发点击动画
            if (catAnimator != null)
            {
                catAnimator.SetTrigger("Tap");
            }
            
            Debug.Log($"猫咪被点击！总次数: {tapCount}");
            
            // 通知UI更新
            CounterUI.Instance?.UpdateTapCount(tapCount);
        }
        
        /// <summary>
        /// 开始踢踏舞
        /// </summary>
        private void StartTapDance()
        {
            isTapDancing = true;
            
            if (catAnimator != null)
            {
                catAnimator.SetBool("IsTapDancing", true);
            }
            
            Debug.Log("猫咪开始踢踏舞！");
            CounterUI.Instance?.UpdateStatus("状态: 踢踏舞中...");
        }
        
        /// <summary>
        /// 停止踢踏舞
        /// </summary>
        public void StopTapDance()
        {
            isTapDancing = false;
            
            if (catAnimator != null)
            {
                catAnimator.SetBool("IsTapDancing", false);
            }
            
            Debug.Log("猫咪停止踢踏舞");
            CounterUI.Instance?.UpdateStatus("状态: 等待输入...");
        }
        
        /// <summary>
        /// 获取当前点击次数
        /// </summary>
        public int GetTapCount()
        {
            return tapCount;
        }
        
        /// <summary>
        /// 重置点击计数
        /// </summary>
        public void ResetTapCount()
        {
            tapCount = 0;
            Debug.Log("点击计数已重置");
            CounterUI.Instance?.UpdateTapCount(0);
        }
        
        /// <summary>
        /// 检查是否在跳舞
        /// </summary>
        public bool IsTapDancing()
        {
            return isTapDancing;
        }
    }
}