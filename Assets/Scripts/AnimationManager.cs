using UnityEngine;

namespace TapCat
{
    /// <summary>
    /// 动画管理器
    /// 负责管理猫咪的动画状态和过渡
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        [Header("动画引用")]
        [SerializeField] private Animator catAnimator;
        
        [Header("动画参数")]
        [SerializeField] private string idleStateName = "Idle";
        [SerializeField] private string tapDanceStateName = "TapDance";
        [SerializeField] private string tapTriggerName = "Tap";
        [SerializeField] private string isTapDancingParam = "IsTapDancing";
        
        [Header("动画设置")]
        [SerializeField] private float idleToDanceTransitionTime = 0.2f;
        [SerializeField] private float danceToIdleTransitionTime = 0.5f;
        
        private void Start()
        {
            if (catAnimator == null)
            {
                catAnimator = GetComponent<Animator>();
            }
            
            // 确保动画控制器存在
            if (catAnimator == null)
            {
                Debug.LogWarning("AnimationManager: 未找到Animator组件");
            }
            else
            {
                Debug.Log("AnimationManager: 动画系统初始化完成");
            }
        }
        
        /// <summary>
        /// 播放点击动画
        /// </summary>
        public void PlayTapAnimation()
        {
            if (catAnimator != null)
            {
                catAnimator.SetTrigger(tapTriggerName);
                Debug.Log("播放点击动画");
            }
        }
        
        /// <summary>
        /// 开始踢踏舞
        /// </summary>
        public void StartTapDance()
        {
            if (catAnimator != null)
            {
                catAnimator.SetBool(isTapDancingParam, true);
                Debug.Log("开始踢踏舞动画");
            }
        }
        
        /// <summary>
        /// 停止踢踏舞
        /// </summary>
        public void StopTapDance()
        {
            if (catAnimator != null)
            {
                catAnimator.SetBool(isTapDancingParam, false);
                Debug.Log("停止踢踏舞动画");
            }
        }
        
        /// <summary>
        /// 检查是否在播放踢踏舞动画
        /// </summary>
        public bool IsTapDancing()
        {
            if (catAnimator != null)
            {
                return catAnimator.GetBool(isTapDancingParam);
            }
            return false;
        }
        
        /// <summary>
        /// 获取当前动画状态
        /// </summary>
        public string GetCurrentState()
        {
            if (catAnimator != null)
            {
                var stateInfo = catAnimator.GetCurrentAnimatorStateInfo(0);
                
                if (stateInfo.IsName(idleStateName))
                    return "Idle";
                else if (stateInfo.IsName(tapDanceStateName))
                    return "TapDance";
                else
                    return "Unknown";
            }
            
            return "No Animator";
        }
        
        /// <summary>
        /// 设置动画速度
        /// </summary>
        public void SetAnimationSpeed(float speed)
        {
            if (catAnimator != null)
            {
                catAnimator.speed = speed;
                Debug.Log($"动画速度设置为: {speed}");
            }
        }
        
        /// <summary>
        /// 重置动画速度
        /// </summary>
        public void ResetAnimationSpeed()
        {
            SetAnimationSpeed(1.0f);
        }
    }
}