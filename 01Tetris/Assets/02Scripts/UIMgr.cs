using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI管理
/// </summary>
public class UIMgr : MonoBehaviour
{
    public static UIMgr _instance;
    /// <summary>
    /// 开始按钮
    /// </summary>
    private Button btnStart;
    /// <summary>
    /// 暂停按钮
    /// </summary>
    private Button btnPause;
    /// <summary>
    /// 重新开始按钮
    /// </summary>
    private Button btnRestart;
    /// <summary>
    /// 分数
    /// </summary>
    private Text textScore;
    /// <summary>
    /// 游戏结束
    /// </summary>
    private Text textGameOver;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        textGameOver = transform.Find("GameOverText").GetComponent<Text>();
        textScore = transform.Find("ScoreText").GetComponent<Text>();
        btnStart = transform.Find("StartBtn").GetComponent<Button>();
        btnPause = transform.Find("PauseBtn").GetComponent<Button>();
        btnRestart = transform.Find("RestartBtn").GetComponent<Button>();
        //开始游戏
        btnStart.onClick.AddListener(() =>
        {
            if (MapData._instance.IsGameOver())
            {
                return;
            }
            GameMgr._instance.StartGame();
            btnStart.gameObject.SetActive(false);
            btnPause.gameObject.SetActive(true);
        });
        //暂停游戏
        btnPause.onClick.AddListener(() =>
        {
            if (MapData._instance.IsGameOver())
            {
                return;
            }	
            GameMgr._instance.PauseGame();
            btnStart.gameObject.SetActive(true);
            btnPause.gameObject.SetActive(false);
        });
        //重新游戏
        btnRestart.onClick.AddListener(() =>
        {
            btnStart.gameObject.SetActive(false);
            btnPause.gameObject.SetActive(true);
            textGameOver.gameObject.SetActive(false);
            //删除当前方块
            GameMgr._instance.DeleteCurrentItem();
            //清空地图
            MapData._instance.ClearMap();
            //重置分数
            GameMgr._instance.SetScore(0);
            //重新游戏
            GameMgr._instance.StartGame();
        });
    }
    /// <summary>
    /// 展示分数
    /// </summary>
    /// <param name="score">分数</param>
    public void ShowScore(int score)
    {
        textScore.text = score.ToString();
    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    public void ShowGameOver()
    {
        textGameOver.gameObject.SetActive(true);
    }
}
