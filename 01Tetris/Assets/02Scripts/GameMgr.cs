using UnityEngine;

/// <summary>
/// 游戏管理
/// </summary>
public class GameMgr : MonoBehaviour
{
    public static GameMgr _instance;

    /// <summary>
    /// 方块预设
    /// </summary>
    public Item[] items;

    /// <summary>
    /// 游戏是否暂停
    /// </summary>
    private bool isPause = false;
    /// <summary>
    /// 当前的方块
    /// </summary>
    private Item currentItem;
    /// <summary>
    /// 分数
    /// </summary>
    private int score = 0;

    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        if (isPause)
            return;
        if (currentItem == null)
            //生成方块
            SpawnItem();
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        isPause = false;
        if (currentItem != null)
        {
            currentItem.Resume();
        }
    }
    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        isPause = true;
        if (currentItem != null)
        {
            currentItem.Pause();
        }
    }
    /// <summary>
    /// 当前方块落下完成
    /// </summary>
    public void FallDownComplete()
    {
        currentItem = null;
        //删除无用的方块父物体
        foreach (Transform t in transform)
        {
            if (t.childCount <= 1)
            {
                Destroy(t.gameObject);
            }
        }
        //方块下落完成后，检查游戏是否结束
        if (MapData._instance.IsGameOver())
        {
            PauseGame();
            UIMgr._instance.ShowGameOver();
        }
    }
    /// <summary>
    /// 删除当前方块
    /// </summary>
    public void DeleteCurrentItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem.gameObject);
            currentItem = null;
        }
    }
    /// <summary>
    /// 增加分数
    /// </summary>
    /// <param name="score">分数</param>
    public void AddScore(int score)
    {
        this.score += score;
        UIMgr._instance.ShowScore(this.score);
    }
    /// <summary>
    /// 设置分数
    /// </summary>
    /// <param name="score">分数</param>
    public void SetScore(int score)
    {
        this.score = score;
        UIMgr._instance.ShowScore(this.score);
    }

    /// <summary>
    /// 生成方块
    /// </summary>
    private void SpawnItem()
    {
        int index = Random.Range(0, items.Length);
        Item item = Instantiate(items[index]);
        item.transform.SetParent(transform);
        currentItem = item;
    }

}
