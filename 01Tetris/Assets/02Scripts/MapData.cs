using UnityEngine;

/// <summary>
/// 地图数据
/// </summary>
public class MapData : MonoBehaviour
{
    public static MapData _instance;

    /// <summary>
    /// 方块正常下落的行数，大于20行，则表示游戏结束
    /// </summary>
    public const int NORMAL_ROWS = 20;
    /// <summary>
    /// 最大行数
    /// </summary>
    public const int MAX_ROWS = 24;
    /// <summary>
    /// 最大列数
    /// </summary>
    public const int MAX_COLUMNS = 10;
    /// <summary>
    /// 地图数据
    /// </summary>
    private Transform[,] map = new Transform[MAX_COLUMNS, MAX_ROWS];

    private void Awake()
    {
        _instance = this;
    }

    #region 公共方法
    /// <summary>
    /// 方块当前位置是否合法
    /// </summary>
    /// <param name="t">方块的父物体</param>
    /// <returns>位置是否合法</returns>
    public bool IsValidPosition(Transform t)
    {
        foreach (Transform child in t)
        {
            if (!child.CompareTag("Item"))
                continue;
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            //是否在地图内
            if (IsInsideMap(new Vector2(x, y)) == false)
                return false;
            //当前地图位置是否为空
            if (map[x, y] != null)
                return false;
        }
        return true;
    }
    /// <summary>
    /// 方块停止下落后，将方块保存地图中
    /// </summary>
    /// <param name="t">方块的父物</param>
    public void PlaceItem(Transform t)
    {
        foreach (Transform child in t)
        {
            if (!child.CompareTag("Item"))
                continue;
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            map[x, y] = child;
        }
        //检测是否有满行的
        CheckMapRowFull();
    }
    /// <summary>
    /// 游戏是否结束
    /// </summary>
    /// <returns>是否结束</returns>
    public bool IsGameOver()
    {
        for (int i = NORMAL_ROWS; i < MAX_ROWS; i++)
        {
            for (int j = 0; j < MAX_COLUMNS; j++)
            {
                if (map[j, i] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 清空地图
    /// </summary>
    public void ClearMap()
    {
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            for (int j = 0; j < MAX_ROWS; j++)
            {
                if (map[i, j] != null)
                {
                    Destroy(map[i, j].gameObject);
                    map[i, j] = null;
                }
            }
        }
    }
    #endregion

    #region 私有方法
    /// <summary>
    /// 当前位置是否在地图内
    /// </summary>
    /// <param name="pos">位置</param>
    /// <returns>是否在地图内</returns>
    private bool IsInsideMap(Vector2 pos)
    {
        return pos.x >= 0 && pos.x < MAX_COLUMNS && pos.y >= 0;
    }
    /// <summary>
    /// 检查地图是否要消除行
    /// </summary>
    private void CheckMapRowFull()
    {
        for (int i = 0; i < MAX_ROWS; i++)
        {
            //检查每一行是否满
            bool isFull = CheckIsRowFull(i);
            if (isFull)
            {
                //增加分数
                GameMgr._instance.AddScore(100);
                //删除满行
                DeleteRow(i);
                //满行以上的方块下落
                MoveDownRowAbove(i + 1);
                i--;
            }
        }
    }
    /// <summary>
    /// 检查行是否满
    /// </summary>
    /// <param name="row">行</param>
    /// <returns>是否行满</returns>
    private bool CheckIsRowFull(int row)
    {
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            if (map[i, row] == null)
                return false;
        }
        return true;
    }
    /// <summary>
    /// 删除行
    /// </summary>
    /// <param name="row">行</param>
    private void DeleteRow(int row)
    {
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            Destroy(map[i, row].gameObject);
            map[i, row] = null;
        }
    }
    /// <summary>
    /// 满行以上的方块下落
    /// </summary>
    /// <param name="row">行</param>
    private void MoveDownRowAbove(int row)
    {
        for (int i = row; i < MAX_ROWS; i++)
        {
            MoveDownRow(i);
        }
    }
    /// <summary>
    /// 满行,以上的某一行方块下落
    /// </summary>
    /// <param name="row">行</param>
    private void MoveDownRow(int row)
    {
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            if (map[i, row] != null)
            {
                map[i, row - 1] = map[i, row];
                map[i, row] = null;
                map[i, row - 1].position += new Vector3(0, -1, 0);
            }
        }
    }
    #endregion
}
