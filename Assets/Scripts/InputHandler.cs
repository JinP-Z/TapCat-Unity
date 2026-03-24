using UnityEngine;

namespace TapCat
{
    /// <summary>
    /// 杈撳叆澶勭悊鍣?
    /// 璐熻矗澶勭悊閿洏鍜岄紶鏍囪緭鍏?
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Header("缁勪欢寮曠敤")]
        [SerializeField] private TapCatController tapCatController;
        [SerializeField] private AnimationManager animationManager;
        [SerializeField] private TapCat2D tapCat2D;
        [SerializeField] private FinalTapCat_Animated_Fix spriteSequenceController;
        
        [Header("杈撳叆璁剧疆")]
        [SerializeField] private KeyCode tapKey = KeyCode.Space;
        [SerializeField] private KeyCode resetKey = KeyCode.R;
        [SerializeField] private KeyCode danceToggleKey = KeyCode.D;
        
        [Header("榧犳爣璁剧疆")]
        [SerializeField] private LayerMask catLayerMask = -1; // 榛樿鎵€鏈夊眰
        [SerializeField] private float maxRayDistance = 100f;
        
        private Camera mainCamera;
        
        private void Start()
        {
            // 鑾峰彇缁勪欢寮曠敤
            if (tapCatController == null)
            {
                tapCatController = GetComponent<TapCatController>();
            }
            
            if (animationManager == null)
            {
                animationManager = GetComponent<AnimationManager>();
            }

            if (tapCat2D == null)
            {
                tapCat2D = GetComponent<TapCat2D>();
            }

            if (spriteSequenceController == null)
            {
                spriteSequenceController = GetComponent<FinalTapCat_Animated_Fix>();
            }

            if (spriteSequenceController == null)
            {
                spriteSequenceController = FindObjectOfType<FinalTapCat_Animated_Fix>();
            }

            if (spriteSequenceController == null && tapCat2D == null)
            {
                spriteSequenceController = gameObject.AddComponent<FinalTapCat_Animated_Fix>();
                Debug.Log("InputHandler: Added FinalTapCat_Animated_Fix to TapCat.");
            }

            if (spriteSequenceController != null)
            {
                spriteSequenceController.SetExternalInputMode(true);
                Animator animator = GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = false;
                }
            }
            
            // 鑾峰彇涓荤浉鏈?
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("InputHandler: 鏈壘鍒颁富鐩告満");
            }
            
            Debug.Log("InputHandler: initialized.");
        }
        
        private void Update()
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }
        
        /// <summary>
        /// 澶勭悊閿洏杈撳叆
        /// </summary>
        private void HandleKeyboardInput()
        {
            // 绌烘牸閿偣鍑?
            if (Input.GetKeyDown(tapKey))
            {
                HandleTap();
            }
            
            // R閿噸缃?
            if (Input.GetKeyDown(resetKey))
            {
                if (tapCatController != null)
                {
                    tapCatController.ResetTapCount();
                    Debug.Log("Press R: tap count reset.");
                }

                if (tapCat2D != null)
                {
                    tapCat2D.ResetGame();
                }

                if (spriteSequenceController != null)
                {
                    spriteSequenceController.ResetGame();
                }
            }
            
            // D閿垏鎹㈣垶韫堢姸鎬?
            if (Input.GetKeyDown(danceToggleKey))
            {
                ToggleTapDance();
            }
            
            // 浠绘剰閿偣鍑伙紙闄や簡鍔熻兘閿級
            if (Input.anyKeyDown && !IsFunctionKey())
            {
                // 閬垮厤閲嶅瑙﹀彂
                if (!Input.GetKeyDown(tapKey) && !Input.GetKeyDown(resetKey) && !Input.GetKeyDown(danceToggleKey))
                {
                    HandleTap();
                }
            }
        }
        
        /// <summary>
        /// 澶勭悊榧犳爣杈撳叆
        /// </summary>
        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0)) // 宸﹂敭鐐瑰嚮
            {
                if (mainCamera == null)
                {
                    mainCamera = Camera.main;
                    if (mainCamera == null)
                    {
                        Debug.LogWarning("InputHandler: Cannot handle mouse input without a main camera.");
                        return;
                    }
                }

                // 妫€鏌ユ槸鍚︾偣鍑诲埌鐚挭
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                bool handled = false;

                if (Physics.Raycast(ray, out hit, maxRayDistance, catLayerMask))
                {
                    // 妫€鏌ユ槸鍚︾偣鍑诲埌TapCat瀵硅薄
                    if (hit.collider.gameObject == gameObject || 
                        hit.collider.transform.IsChildOf(transform))
                    {
                        HandleTap();
                        Debug.Log("榧犳爣鐐瑰嚮鍒扮尗鍜紒");
                        handled = true;
                    }
                }

                if (!handled)
                {
                    RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction, maxRayDistance, catLayerMask);
                    if (hit2D.collider != null)
                    {
                        if (hit2D.collider.gameObject == gameObject ||
                            hit2D.collider.transform.IsChildOf(transform))
                        {
                            HandleTap();
                            Debug.Log("榧犳爣鐐瑰嚮鍒扮尗鍜紒");
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 澶勭悊鐐瑰嚮浜嬩欢
        /// </summary>
        private void HandleTap()
        {
            bool playedSprite = false;

            if (tapCat2D != null)
            {
                tapCat2D.StartCatAnimation();
                playedSprite = true;
            }
            else if (spriteSequenceController != null)
            {
                spriteSequenceController.PlayTapAnimation();
                playedSprite = true;
            }

            if (tapCatController != null)
            {
                tapCatController.OnCatTapped();
            }
            
            if (!playedSprite && animationManager != null)
            {
                animationManager.PlayTapAnimation();
            }
        }
        
        /// <summary>
        /// 鍒囨崲韪㈣笍鑸炵姸鎬?
        /// </summary>
        private void ToggleTapDance()
        {
            if (tapCatController != null)
            {
                if (tapCatController.IsTapDancing())
                {
                    tapCatController.StopTapDance();
                }
                else
                {
                    tapCatController.OnCatTapped(); // 寮€濮嬭垶韫?
                }
            }
            
            Debug.Log("Toggle dance mode.");
        }
        
        /// <summary>
        /// 妫€鏌ユ槸鍚︽槸鍔熻兘閿?
        /// </summary>
        private bool IsFunctionKey()
        {
            return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ||
                   Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                   Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) ||
                   Input.GetKey(KeyCode.CapsLock) || Input.GetKey(KeyCode.Tab) ||
                   Input.GetKey(KeyCode.Escape);
        }
        
        /// <summary>
        /// 璁剧疆鐐瑰嚮閿?
        /// </summary>
        public void SetTapKey(KeyCode newKey)
        {
            tapKey = newKey;
            Debug.Log($"鐐瑰嚮閿凡璁剧疆涓? {newKey}");
        }
        
        /// <summary>
        /// 鑾峰彇褰撳墠鐐瑰嚮閿?
        /// </summary>
        public KeyCode GetTapKey()
        {
            return tapKey;
        }
    }
}
