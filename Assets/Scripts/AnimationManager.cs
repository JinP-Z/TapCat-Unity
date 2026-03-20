using UnityEngine;

namespace TapCat
{
    /// <summary>
    /// 鍔ㄧ敾绠＄悊鍣?
    /// 璐熻矗绠＄悊鐚挭鐨勫姩鐢荤姸鎬佸拰杩囨浮
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        [Header("鍔ㄧ敾寮曠敤")]
        [SerializeField] private Animator catAnimator;
        
        [Header("鍔ㄧ敾鍙傛暟")]
        [SerializeField] private string idleStateName = "Idle";
        [SerializeField] private string tapDanceStateName = "TapDance";
        [SerializeField] private string tapTriggerName = "Tap";
        [SerializeField] private string isTapDancingParam = "IsTapDancing";
        
        [Header("鍔ㄧ敾璁剧疆")]
        [SerializeField] private float idleToDanceTransitionTime = 0.2f;
        [SerializeField] private float danceToIdleTransitionTime = 0.5f;
        
        private void Start()
        {
            if (catAnimator == null)
            {
                catAnimator = GetComponent<Animator>();
            }
            
            // 纭繚鍔ㄧ敾鎺у埗鍣ㄥ瓨鍦?
            if (catAnimator == null)
            {
                Debug.LogWarning("AnimationManager: 鏈壘鍒癆nimator缁勪欢");
            }
            else
            {
                Debug.Log("AnimationManager: initialized.");
            }
        }
        
        /// <summary>
        /// 鎾斁鐐瑰嚮鍔ㄧ敾
        /// </summary>
        public void PlayTapAnimation()
        {
            if (catAnimator != null)
            {
                catAnimator.SetTrigger(tapTriggerName);
                Debug.Log("鎾斁鐐瑰嚮鍔ㄧ敾");
            }
        }
        
        /// <summary>
        /// 寮€濮嬭涪韪忚垶
        /// </summary>
        public void StartTapDance()
        {
            if (catAnimator != null)
            {
                catAnimator.SetBool(isTapDancingParam, true);
                Debug.Log("寮€濮嬭涪韪忚垶鍔ㄧ敾");
            }
        }
        
        /// <summary>
        /// 鍋滄韪㈣笍鑸?
        /// </summary>
        public void StopTapDance()
        {
            if (catAnimator != null)
            {
                catAnimator.SetBool(isTapDancingParam, false);
                Debug.Log("Stop tap dance animation.");
            }
        }
        
        /// <summary>
        /// 妫€鏌ユ槸鍚﹀湪鎾斁韪㈣笍鑸炲姩鐢?
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
        /// 鑾峰彇褰撳墠鍔ㄧ敾鐘舵€?
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
        /// 璁剧疆鍔ㄧ敾閫熷害
        /// </summary>
        public void SetAnimationSpeed(float speed)
        {
            if (catAnimator != null)
            {
                catAnimator.speed = speed;
                Debug.Log($"鍔ㄧ敾閫熷害璁剧疆涓? {speed}");
            }
        }
        
        /// <summary>
        /// 閲嶇疆鍔ㄧ敾閫熷害
        /// </summary>
        public void ResetAnimationSpeed()
        {
            SetAnimationSpeed(1.0f);
        }
    }
}
