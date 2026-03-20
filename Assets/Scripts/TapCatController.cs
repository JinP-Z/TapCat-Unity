using UnityEngine;

namespace TapCat
{
    /// <summary>
    /// TapCat涓绘帶鍒跺櫒
    /// 璐熻矗澶勭悊鐚挭鐨勭偣鍑诲搷搴斿拰鐘舵€佺鐞?
    /// </summary>
    public class TapCatController : MonoBehaviour
    {
        [Header("鍔ㄧ敾寮曠敤")]
        [SerializeField] private Animator catAnimator;
        
        [Header("鐐瑰嚮璁剧疆")]
        [SerializeField] private int tapCount = 0;
        [SerializeField] private float tapCooldown = 0.2f;
        
        private float lastTapTime = 0f;
        private bool isTapDancing = false;
        
        /// <summary>
        /// 褰撶尗鍜鐐瑰嚮鏃惰皟鐢?
        /// </summary>
        public void OnCatTapped()
        {
            // 妫€鏌ュ喎鍗存椂闂?
            if (Time.time - lastTapTime < tapCooldown)
                return;
            
            lastTapTime = Time.time;
            tapCount++;
            
            // 鍒囨崲鑸炶箞鐘舵€?
            if (!isTapDancing)
            {
                StartTapDance();
            }
            
            // 瑙﹀彂鐐瑰嚮鍔ㄧ敾
            if (catAnimator != null)
            {
                catAnimator.SetTrigger("Tap");
            }
            
            Debug.Log($"鐚挭琚偣鍑伙紒鎬绘鏁? {tapCount}");
            
            // 閫氱煡UI鏇存柊
            CounterUI.Instance?.UpdateTapCount(tapCount);
        }
        
        /// <summary>
        /// 寮€濮嬭涪韪忚垶
        /// </summary>
        private void StartTapDance()
        {
            isTapDancing = true;
            
            if (catAnimator != null)
            {
                catAnimator.SetBool("IsTapDancing", true);
            }
            
            Debug.Log("Tap dance started.");
            CounterUI.Instance?.UpdateStatus("鐘舵€? 韪㈣笍鑸炰腑...");
        }
        
        /// <summary>
        /// 鍋滄韪㈣笍鑸?
        /// </summary>
        public void StopTapDance()
        {
            isTapDancing = false;
            
            if (catAnimator != null)
            {
                catAnimator.SetBool("IsTapDancing", false);
            }
            
            Debug.Log("Tap dance stopped.");
            CounterUI.Instance?.UpdateStatus("鐘舵€? 绛夊緟杈撳叆...");
        }
        
        /// <summary>
        /// 鑾峰彇褰撳墠鐐瑰嚮娆℃暟
        /// </summary>
        public int GetTapCount()
        {
            return tapCount;
        }
        
        /// <summary>
        /// 閲嶇疆鐐瑰嚮璁℃暟
        /// </summary>
        public void ResetTapCount()
        {
            tapCount = 0;
            Debug.Log("Tap count reset.");
            CounterUI.Instance?.UpdateTapCount(0);
        }
        
        /// <summary>
        /// 妫€鏌ユ槸鍚﹀湪璺宠垶
        /// </summary>
        public bool IsTapDancing()
        {
            return isTapDancing;
        }
    }
}
