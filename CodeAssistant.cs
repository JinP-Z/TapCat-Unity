using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Unity缂栫爜鍔╂墜鑴氭湰
/// 鍔熻兘锛?/// 1. 鍦ㄦ帶鍒跺彴杈撳嚭"缂栫爜鍔╂墜杩炴帴鎴愬姛锛?
/// 2. 鏄剧ず褰撳墠鏃ユ湡鍜屾椂闂?/// 3. 姣?绉掕緭鍑轰竴娆″績璺?/// 4. 鎸塃SC閿€€鍑?/// </summary>
public class CodeAssistant : MonoBehaviour
{
    // 蹇冭烦闂撮殧鏃堕棿锛堢锛?    [SerializeField] private float heartbeatInterval = 5f;
    
    // 蹇冭烦鍗忕▼寮曠敤
    private Coroutine heartbeatCoroutine;
    
    // 鏄惁姝ｅ湪杩愯
    private bool isRunning = false;
    
    /// <summary>
    /// 鑴氭湰鍚姩鏃惰皟鐢?    /// </summary>
    private void Start()
    {
        // 鍒濆鍖?        Initialize();
    }
    
    /// <summary>
    /// 鍒濆鍖栫紪鐮佸姪鎵?    /// </summary>
    private void Initialize()
    {
        // 杈撳嚭杩炴帴鎴愬姛淇℃伅
        Debug.Log("缂栫爜鍔╂墜杩炴帴鎴愬姛锛?);
        
        // 鏄剧ず褰撳墠鏃ユ湡鍜屾椂闂?        DisplayCurrentDateTime();
        
        // 寮€濮嬪績璺?        StartHeartbeat();
        
        isRunning = true;
        Debug.Log("缂栫爜鍔╂墜宸插惎鍔紝鎸塃SC閿€€鍑恒€?);
    }
    
    /// <summary>
    /// 鏄剧ず褰撳墠鏃ユ湡鍜屾椂闂?    /// </summary>
    private void DisplayCurrentDateTime()
    {
        DateTime now = DateTime.Now;
        string dateTimeString = now.ToString("yyyy-MM-dd HH:mm:ss");
        string dayOfWeek = GetChineseDayOfWeek(now.DayOfWeek);
        
        Debug.Log($"褰撳墠鏃堕棿锛歿dateTimeString}  {dayOfWeek}");
    }
    
    /// <summary>
    /// 灏嗚嫳鏂囨槦鏈熻浆鎹负涓枃
    /// </summary>
    private string GetChineseDayOfWeek(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Sunday: return "鏄熸湡鏃?;
            case DayOfWeek.Monday: return "鏄熸湡涓€";
            case DayOfWeek.Tuesday: return "鏄熸湡浜?;
            case DayOfWeek.Wednesday: return "鏄熸湡涓?;
            case DayOfWeek.Thursday: return "鏄熸湡鍥?;
            case DayOfWeek.Friday: return "鏄熸湡浜?;
            case DayOfWeek.Saturday: return "鏄熸湡鍏?;
            default: return "鏈煡";
        }
    }
    
    /// <summary>
    /// 寮€濮嬪績璺?    /// </summary>
    private void StartHeartbeat()
    {
        // 濡傛灉宸茬粡鏈夊績璺冲湪杩愯锛屽厛鍋滄
        if (heartbeatCoroutine != null)
        {
            StopCoroutine(heartbeatCoroutine);
        }
        
        // 鍚姩鏂扮殑蹇冭烦鍗忕▼
        heartbeatCoroutine = StartCoroutine(HeartbeatRoutine());
    }
    
    /// <summary>
    /// 蹇冭烦鍗忕▼
    /// </summary>
    private IEnumerator HeartbeatRoutine()
    {
        int heartbeatCount = 0;
        
        while (isRunning)
        {
            yield return new WaitForSeconds(heartbeatInterval);
            
            heartbeatCount++;
            DateTime now = DateTime.Now;
            string timeString = now.ToString("HH:mm:ss");
            
            Debug.Log($"蹇冭烦 #{heartbeatCount} - 鏃堕棿锛歿timeString}");
        }
    }
    
    /// <summary>
    /// 姣忓抚鏇存柊
    /// </summary>
    private void Update()
    {
        // 妫€鏌SC閿槸鍚﹁鎸変笅
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }
    
    /// <summary>
    /// 閫€鍑虹紪鐮佸姪鎵?    /// </summary>
    private void Exit()
    {
        if (!isRunning) return;
        
        isRunning = false;
        
        // 鍋滄蹇冭烦鍗忕▼
        if (heartbeatCoroutine != null)
        {
            StopCoroutine(heartbeatCoroutine);
            heartbeatCoroutine = null;
        }
        
        // 杈撳嚭閫€鍑轰俊鎭?        Debug.Log("缂栫爜鍔╂墜宸查€€鍑恒€?);
        
        // 濡傛灉鏄紪杈戝櫒妯″紡锛屽仠姝㈡挱鏀?        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 濡傛灉鏄瀯寤虹増鏈紝閫€鍑哄簲鐢?        Application.Quit();
        #endif
    }
    
    /// <summary>
    /// 鑴氭湰琚鐢ㄦ椂璋冪敤
    /// </summary>
    private void OnDisable()
    {
        // 纭繚蹇冭烦鍗忕▼琚仠姝?        if (heartbeatCoroutine != null)
        {
            StopCoroutine(heartbeatCoroutine);
            heartbeatCoroutine = null;
        }
    }
    
    /// <summary>
    /// 鑴氭湰琚攢姣佹椂璋冪敤
    /// </summary>
    private void OnDestroy()
    {
        // 娓呯悊璧勬簮
        if (isRunning)
        {
            Debug.Log("缂栫爜鍔╂墜鑴氭湰琚攢姣併€?);
        }
    }
    
    /// <summary>
    /// 鍏叡鏂规硶锛氭墜鍔ㄨЕ鍙戜竴娆″績璺?    /// </summary>
    public void TriggerHeartbeat()
    {
        DateTime now = DateTime.Now;
        string timeString = now.ToString("HH:mm:ss");
        Debug.Log($"鎵嬪姩蹇冭烦 - 鏃堕棿锛歿timeString}");
    }
    
    /// <summary>
    /// 鍏叡鏂规硶锛氶噸鏂板惎鍔ㄧ紪鐮佸姪鎵?    /// </summary>
    public void Restart()
    {
        if (heartbeatCoroutine != null)
        {
            StopCoroutine(heartbeatCoroutine);
            heartbeatCoroutine = null;
        }
        
        Initialize();
    }
    
    /// <summary>
    /// 鍏叡鏂规硶锛氬仠姝㈢紪鐮佸姪鎵?    /// </summary>
    public void Stop()
    {
        Exit();
    }
}
