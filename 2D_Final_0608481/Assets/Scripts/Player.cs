using UnityEngine;
using UnityEngine.UI; // 引用 介面 API

public class Player : MonoBehaviour
{
    // 修飾詞 類型 名稱 (指定 值) ;

    [Header("角色等級")]
    public int lv = 1;
    [Header("移動速度"), Range(0, 300)]
    public float speed = 10.5f;
    public bool isDead = false;
    [Tooltip("這是角色名稱")]
    public string cName = "原始人";
    [Header("虛擬搖桿")]
    public FixedJoystick joystick;
    [Header("變形元件")]
    public Transform tra;
    [Header("動畫元件")]
    public Animator ani;
    [Header("偵測範圍")]
    public float rangeAttack = 2.5f;
    [Header("音效來源")]
    public AudioSource aud;
    [Header("攻擊音效")]
    public AudioClip soundAttack;

    Vector3 currentEulerAngles;
    float x;
    float y;
    float z;

    // 事件 : 繪製圖示
    private void OnDrawGizmos()
    {
        // 指定圖示顏色 (紅, 綠, 藍, 透明)
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        // 繪製圖示 球體(中心點, 半徑)
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }

    private void Move()
    {
        float h = joystick.Horizontal;
        tra.Translate(h * speed * Time.deltaTime, 0, 0);
        ani.SetBool("等待", h != 0);
        
        if(h >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Attack()
    {
        // 音效來源.播放一次(音效片段,音量)
        aud.PlayOneShot(soundAttack, 1.2f);
        
        // 2D 物理 圓形碰撞(中心點, 半徑, 方向)
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, rangeAttack, -transform.up, 0, 1 << 8);
        // 如果 碰到的物件 標籤 為 道具 就取得道具腳本並呼叫掉落道具方法
        if (hit && hit.collider.tag == "道具") hit.collider.GetComponent<Item>().DropProp();
    }

    private void Hit()
    {

    }

    private void Dead()
    {

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Move();
    }

    [Header("吃金條音效")]
    public AudioClip soundEat;
    [Header("金幣數量")]
    public Text textCoin;

    private int coin;
    
    // 觸發事件 - 進入 : 兩個物件必須有一個勾選 Is Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "寶石")
        {
            coin++;
            aud.PlayOneShot(soundEat);
            Destroy(collision.gameObject);
            textCoin.text = "金幣 : " + coin;
        }
    }
}
