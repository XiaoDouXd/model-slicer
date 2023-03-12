using UnityEngine;

public class APInitMgr : MonoBehaviour
{
    // ---------------------------------------------------------------------------
    // 这里用于挂在初始化需要的资源
    [Tooltip("用于混色模型的颜色-颜料映射表")]
    public Texture2D colorTable;
    
    [Space(20)]
    [Tooltip("默认纸张纹理")]
    public Texture2D defaultPaper;
    [Tooltip("默认纸张阻塞纹理")]
    public Texture2D defaultPaperSub;

    [Space(20)]
    [Tooltip("UI根节点")]
    public RectTransform surfaceRoot;

    [Space(20)]
    [Tooltip("笔刷贴图1")]
    public Texture2D brushTex1;
    
    // ---------------------------------------------------------------------------
    // 渲染初始化
    private RenderTexture _tex;
    private void Start()
    {

    }

    // ---------------------------------------------------------------------------
    #region 单例类
    public static APInitMgr I => _i;
    private static APInitMgr _i;
    
    private void Awake()
    {
        if (_i == null)
        {
            _i = this;
            // 锁帧
            Application.targetFrameRate = 120;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion
    #region 工具函数
    public Vector2 WindowCenter => new Vector2(Screen.width/2.0f, Screen.height/2.0f);
    public Vector2 WindowSize => new Vector2(Screen.width, Screen.height);
    public void RenderReset()
    {

    }
    public float WindowAspect => (float)Screen.width / Screen.height;
    #endregion
}
