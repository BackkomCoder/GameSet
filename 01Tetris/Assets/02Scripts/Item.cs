using UnityEngine;

/// <summary>
/// 每一个方块的控制
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// 是否暂停
    /// </summary>
    private bool isPause = false;
    /// <summary>
    /// 下落计时器
    /// </summary>
    private float timer = 0;
    /// <summary>
    /// 下落间隔
    /// </summary>
    private float intervalTime = 0.5f;
    /// <summary>
    /// 旋转点
    /// </summary>
    private Transform pivot;

    private void Start()
    {
        pivot = transform.Find("Pivot");
    }
    private void Update()
    {
        if (isPause)
            return;
        timer += Time.deltaTime;
        if (timer > intervalTime)
        {
            timer = 0;
            //下落
            Fall();
        }
        //方块控制
        InputCtrl();
    }

    /// <summary>
    /// 下落
    /// </summary>
    private void Fall()
    {
        Vector3 pos = transform.position;
        pos.y -= 1;
        transform.position = pos;
        //位置不合法，回复到上一个位置，并暂停下落，将方块保存到地图中
        if (MapData._instance.IsValidPosition(transform) == false)
        {
            pos.y += 1;
            transform.position = pos;
            isPause = true;
            //将方块保存到地图中
            MapData._instance.PlaceItem(transform);
            //当前方块完成下落
            GameMgr._instance.FallDownComplete();
        }
    }
    /// <summary>
    /// 输入控制
    /// </summary>
    void InputCtrl()
    {
        float h = 0;
        //左
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            h = -1;
        }
        //右
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            h = 1;
        }
        //旋转
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(pivot.position, Vector3.forward, -90);
            //位置是否合法检测
            if (MapData._instance.IsValidPosition(transform) == false)
            {
                transform.RotateAround(pivot.position, Vector3.forward, 90);
            }
        }
        //下落
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            intervalTime = 0.1f;
        }
        else
        {
            intervalTime = 0.5f;
        }
        if (h != 0)
        {

            Vector3 pos = transform.position;
            pos.x += h;
            transform.position = pos;
            //位置是否合法检测
            if (MapData._instance.IsValidPosition(transform) == false)
            {
                pos.x -= h;
                transform.position = pos;
            }
        }
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        isPause = true;
    }
    /// <summary>
    /// 继续
    /// </summary>
    public void Resume()
    {
        isPause = false;
    }

}
