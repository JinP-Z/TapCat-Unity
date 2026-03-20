using UnityEngine;

public class SceneAutoStarter : MonoBehaviour
{
    void Start()
    {
        // 确保自动设置脚本运行
        gameObject.AddComponent<AutoTapCatSetup>();
        
        Debug.Log("?? 场景自动启动完成");
        Debug.Log("?? 现在可以点击 Play 测试 TapCat 动画版了！");
    }
}
