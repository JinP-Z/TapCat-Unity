using System;
using UnityEngine;

namespace TapCat.Input
{
    /// <summary>
    /// 输入管理器，负责处理所有键盘和鼠标输入事件
    /// 符合技术宪法第六章：输入系统规范
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// 输入事件委托
        /// </summary>
        public event Action OnInputTriggered;

        /// <summary>
        /// 输入计数变化事件（包含冷却期内的输入统计）
        /// </summary>
        public event Action<int> OnInputCountChanged;

        [Header("冷却设置")]
        [SerializeField, Range(0.01f, 1f)] private float cooldownTime = 0.2f;
        [SerializeField] private bool enableCooldown = true;

        private float lastInputTime;
        private int totalInputCount;
        private bool isInCooldown;

        /// <summary>
        /// 总输入次数
        /// </summary>
        public int TotalInputCount => totalInputCount;

        /// <summary>
        /// 是否在冷却中
        /// </summary>
        public bool IsInCooldown => isInCooldown;

        /// <summary>
        /// 冷却时间（秒）
        /// </summary>
        public float CooldownTime => cooldownTime;

        /// <summary>
        /// 冷却剩余时间（0-1）
        /// </summary>
        public float CooldownRemainingRatio
        {
            get
            {
                if (!isInCooldown) return 0f;
                float elapsed = Time.time - lastInputTime;
                return Mathf.Clamp01(1f - Mathf.Clamp01(elapsed / cooldownTime));
            }
        }

        private void Update()
        {
            UpdateCooldownState();
            ProcessInput();
        }

        /// <summary>
        /// 处理所有输入
        /// </summary>
        private void ProcessInput()
        {
            // 检查冷却状态
            if (enableCooldown && isInCooldown)
            {
                // 冷却期间统计输入次数但不触发动画
                if (CheckAnyInput())
                {
                    totalInputCount++;
                    OnInputCountChanged?.Invoke(totalInputCount);
                }
                return;
            }

            // 检查是否有输入
            if (CheckAnyInput())
            {
                TriggerInput();
            }
        }

        /// <summary>
        /// 检查是否有任何输入
        /// </summary>
        private bool CheckAnyInput()
        {
            // 检查所有键盘按键（技术宪法第二十二条：所有键盘按键都触发）
            for (int i = 0; i < (int)KeyCode.JoystickButton19; i++)
            {
                if (UnityEngine.Input.GetKeyDown((KeyCode)i))
                {
                    return true;
                }
            }

            // 检查所有鼠标按键（技术宪法第二十二条：所有鼠标按键都触发）
            for (int i = 0; i < 6; i++) // 支持最多6个鼠标按键
            {
                if (UnityEngine.Input.GetMouseButtonDown(i))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 触发输入事件
        /// </summary>
        private void TriggerInput()
        {
            totalInputCount++;
            lastInputTime = Time.time;
            isInCooldown = true;

            // 触发事件
            OnInputCountChanged?.Invoke(totalInputCount);
            OnInputTriggered?.Invoke();
        }

        /// <summary>
        /// 更新冷却状态
        /// </summary>
        private void UpdateCooldownState()
        {
            if (!enableCooldown || !isInCooldown) return;

            float elapsed = Time.time - lastInputTime;
            if (elapsed >= cooldownTime)
            {
                isInCooldown = false;
            }
        }

        /// <summary>
        /// 重置输入统计
        /// </summary>
        public void ResetInputCount()
        {
            totalInputCount = 0;
            OnInputCountChanged?.Invoke(totalInputCount);
        }

        /// <summary>
        /// 设置冷却时间
        /// </summary>
        public void SetCooldownTime(float time)
        {
            cooldownTime = Mathf.Clamp(time, 0.01f, 1f);
        }

        /// <summary>
        /// 启用/禁用冷却
        /// </summary>
        public void SetCooldownEnabled(bool enabled)
        {
            enableCooldown = enabled;
            if (!enabled)
            {
                isInCooldown = false;
            }
        }

        /// <summary>
        /// 获取输入统计信息
        /// </summary>
        public string GetInputStats()
        {
            return $"总输入: {totalInputCount}, 冷却: {(isInCooldown ? $"{(cooldownTime - (Time.time - lastInputTime)):F2}s" : "无")}";
        }
    }
}
