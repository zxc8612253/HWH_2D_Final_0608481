using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    // 陳列 類型後面加上中括號
    // 用於保存相同類型的複數資料
    public GameObject stone;
    [Header("要關閉的石頭們")]
    public GameObject[] stones;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "木箱")
        {
            stones[0].SetActive(false);
        }
    }
}
