using UnityEngine;
using UnityEngine.SceneManagement;//引用 場景管理 API
using UnityEngine.UI; // 引用 介面 API

public class Player : MonoBehaviour
{
    // 修飾詞 類型 名稱 (指定 值) ;

    [Header("等級")]
    public int lv = 1;
    [Header("移動速度"), Range(0, 300)]
    public float speed = 10.5f;
    [Header("角色是否死亡")]
    public bool isDead = false;
    [Tooltip("這是角色的名字")]
    public string cName = "貓咪";
    [Header("虛擬搖桿")]
    public FixedJoystick joystick;
    [Header("變形元件")]
    public Transform tra;
    [Header("動畫元素")]
    public Animator ani;
    [Header("偵測範圍")]
    public float rangeAttack = 2.5f;
    [Header("音效來源")]
    public AudioSource aud;
    [Header("攻擊音效")]
    public AudioClip soundAttack;
    [Header("血量")]
    public float hp = 200;
    [Header("血條系統")]
    public HpManager hpManager;
    [Header("攻擊力"), Range(0, 1000)]
    public float attack = 20;


    private float hpMax;


    //Vector3 currentEulerAngles;
    //float x;
    //float y;
    //float z;

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
        if(isDead) return;     // 如果 死亡 就跳出

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
        if (isDead) return;     // 如果 死亡 就跳出

        // 音效來源.播放一次(音效片段,音量)
        aud.PlayOneShot(soundAttack, 1.2f);
        
        // 2D 物理 圓形碰撞(中心點, 半徑, 方向)
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, rangeAttack, -transform.up, 0, 1 << 8);
        // 如果 碰到的物件 標籤 為 道具 就取得道具腳本並呼叫掉落道具方法
        if (hit && hit.collider.tag == "道具") hit.collider.GetComponent<Item>().DropProp();
        // 如果 打到的標籤是 敵人 就對他 造成傷害
        if (hit && hit.collider.tag == "敵人") hit.collider.GetComponent<Enemy>().Hit(attack);
    }

    public void Hit(float damage)
    {
        hp -= damage;                            // 扣除傷害值               
        hpManager.UpdateHpBar(hp, hpMax);        // 更新血條
        StartCoroutine(hpManager.ShowDamage(damage));  // 啟動協同程序 (顯示傷害值())

        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        hp = 0;
        isDead = true;
        Invoke("Replay", 2);      // 延遲呼叫("方法名稱",延遲時間)
    }
    private void Replay()
    {
        SceneManager.LoadScene("遊戲場景");
    }

    private void Start()
    {
        hpMax = hp; // 取得血量最大值
    }

    private void Update()
    {
        Move();
    }

    
    // 觸發事件 - 進入 : 兩個物件必須有一個勾選 Is Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "寶石")
        {
            Destroy(collision.gameObject);
        }
    }
}
