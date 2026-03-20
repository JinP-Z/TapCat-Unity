// 简单猫咪动画控制器 - 临时版本，等待 Codex 完善
// 功能：播放10帧猫咪动画，每次点击播放完整序列

using UnityEngine;

public class SimpleCatAnimation : MonoBehaviour
{
    // 动画帧（在Unity编辑器中设置）
    public Sprite[] animationFrames;
    
    // 每帧持续时间（秒）
    public float frameTime = 0.1f;
    
    // 私有变量
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private bool isPlaying = false;
    private float timer = 0f;
    
    void Start()
    {
        // 获取或添加 SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // 如果有帧，设置第一帧
        if (animationFrames != null && animationFrames.Length > 0)
        {
            spriteRenderer.sprite = animationFrames[0];
        }
    }
    
    void Update()
    {
        if (isPlaying && animationFrames != null && animationFrames.Length > 0)
        {
            timer += Time.deltaTime;
            
            if (timer >= frameTime)
            {
                timer = 0f;
                currentFrame++;
                
                if (currentFrame >= animationFrames.Length)
                {
                    // 动画完成
                    isPlaying = false;
                    currentFrame = 0;
                    spriteRenderer.sprite = animationFrames[0];
                }
                else
                {
                    // 播放下一帧
                    spriteRenderer.sprite = animationFrames[currentFrame];
                }
            }
        }
    }
    
    // 播放动画
    public void Play()
    {
        if (animationFrames == null || animationFrames.Length == 0) return;
        
        isPlaying = true;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = animationFrames[0];
    }
    
    // 检查是否正在播放
    public bool IsPlaying()
    {
        return isPlaying;
    }
}