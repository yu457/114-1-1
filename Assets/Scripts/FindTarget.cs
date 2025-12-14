using TMPro;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    public Transform 最近的敵人;
    public Vector3 敵人座標;
    public float 最近距離;
    public GameObject 最終目標;

    private void Start()
    {
        最終目標 = GameObject.Find("Target");
    }
    // Update is called once per frame
    void Update()
    {
        // 1. 搜尋所有具有指定 Tag 的遊戲物件
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 如果場景中沒有任何敵人
        if (enemies.Length == 0)
        {
            最近的敵人 = null;
            敵人座標 = Vector3.zero;
            最近距離 = 0f;
            return;
        }

        // 初始設定最短距離為無限大，以便第一次比較時一定會被取代
        float shortestDistance = float.MaxValue;
        GameObject closestEnemy = null;

        // 2. 遍歷所有敵人
        foreach (GameObject enemy in enemies)
        {
            // 3. 計算當前物體與敵人的距離
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // 4. 判斷是否為目前找到的最近目標
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemy; // 儲存這個敵人物件
            }
            if(distance > 10f)
            {
                closestEnemy = null;
            }
        }

        // 5. 設定最終結果
        if (closestEnemy != null)
        {
            最近的敵人 = closestEnemy.transform;
            // 設定目標座標
            敵人座標 = closestEnemy.transform.position;
            最近距離 = shortestDistance;
            敵人座標.y = 1.4f;
            最終目標.transform.position = 敵人座標;
        }
        else
        {
            Vector3 原始座標 = new Vector3(0,1.6f,1.72f);
            最終目標.transform.localPosition = 原始座標;
        }
    }
}