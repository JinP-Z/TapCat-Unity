using UnityEngine;

namespace TapCat
{
    /// <summary>
    /// 输入处理器
    /// 负责处理键盘和鼠标输入
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Header("组件引用")]
        [SerializeField] private TapCatController tapCatController;
        [SerializeField] private AnimationManager animationManager;
        
        [Header("输入设置")]
        [SerializeField] private KeyCode tapKey = KeyCode.Space;
        [SerializeField] private KeyCode resetKey = KeyCode.R;
        [SerializeField] private KeyCode danceToggleKey = KeyCode.D;
        
        [Header("鼠标设置")]
        [SerializeField] private LayerMask catLayerMask = -1; // 默认所有层
        [SerializeField] private float maxRayDistance = 100f;
        
        private Camera mainCamera;
        
        private void Start()
        {
            // 获取组件引用
            if (tapCatController == null)
            {
                tapCatController = GetComponent<TapCatController>();
            }
            
            if (animationManager == null)
            {
                animationManager = GetComponent<AnimationManager>();
            }
            
            // 获取主相机
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("InputHandler: 未找到主相机");
            }
            
            Debug.Log("InputHandler: 输入系统初始化完成");
        }
        
        private void Update()
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }
        
        /// <summary>
        /// 处理键盘输入
        /// </summary>
        private void HandleKeyboardInput()
        {
            // 空格键点击
            if (Input.GetKeyDown(tapKey))
            {
                HandleTap();
            }
            
            // R键重置
            if (Input.GetKeyDown(resetKey))
            {
                if (tapCatController != null)
                {
                    tapCatController.ResetTapCount();
                    Debug.Log("按R键重置点击计数");
                }
            }
            
            // D键切换舞蹈状态
            if (Input.GetKeyDown(danceToggleKey))
            {
                ToggleTapDance();
            }
            
            // 任意键点击（除了功能键）
            if (Input.anyKeyDown && !IsFunctionKey())
            {
                // 避免重复触发
                if (!Input.GetKeyDown(tapKey) && !Input.GetKeyDown(resetKey) && !Input.GetKeyDown(danceToggleKey))
                {
                    HandleTap();
                }
            }
        }
        
        /// <summary>
        /// 处理鼠标输入
        /// </summary>
        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0)) // 左键点击
            {
                // 检查是否点击到猫咪
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, maxRayDistance, catLayerMask))
                {
                    // 检查是否点击到TapCat对象
                    if (hit.collider.gameObject == gameObject || 
                        hit.collider.transform.IsChildOf(transform))
                    {
                        HandleTap();
                        Debug.Log("鼠标点击到猫咪！");
                    }
                }
            }
        }
        
        /// <summary>
        /// 处理点击事件
        /// </summary>
        private void HandleTap()
        {
            if (tapCatController != null)
            {
                tapCatController.OnCatTapped();
            }
            
            if (animationManager != null)
            {
                animationManager.PlayTapAnimation();
            }
        }
        
        /// <summary>
        /// 切换踢踏舞状态
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
                    tapCatController.OnCatTapped(); // 开始舞蹈
                }
            }
            
            Debug.Log("按D键切换舞蹈状态");
        }
        
        /// <summary>
        /// 检查是否是功能键
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
        /// 设置点击键
        /// </summary>
        public void SetTapKey(KeyCode newKey)
        {
            tapKey = newKey;
            Debug.Log($"点击键已设置为: {newKey}");
        }
        
        /// <summary>
        /// 获取当前点击键
        /// </summary>
        public KeyCode GetTapKey()
        {
            return tapKey;
        }
    }
}