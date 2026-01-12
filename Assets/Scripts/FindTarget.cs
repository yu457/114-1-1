using UnityEngine;

public class FindTarget : MonoBehaviour
{
    [Header("視覺設定")]
    public Material 視覺材質;
    public float 視覺距離 = 10f;
    public float 視覺角度 = 90f;
    public LayerMask 視線遮蔽圖層; // 包含障礙物與敵人
    public int 視線圓弧點 = 60;

    [Header("目標資訊")]
    public Transform 最近的敵人;
    public GameObject 最終目標;
    public Vector3 原始相對座標 = new Vector3(0, 1.6f, 1.72f);

    // 內部建立模型用
    Mesh 視線範圍模型;
    MeshFilter 變形視線範圍;

    void Start()
    {
        最終目標 = GameObject.Find("Target");

        // 初始化視覺模型繪製組件
        GameObject visualObj = new GameObject("PlayerFOV");
        visualObj.transform.SetParent(this.transform);
        visualObj.transform.localPosition = Vector3.zero;
        visualObj.transform.localRotation = Quaternion.identity;

        visualObj.AddComponent<MeshRenderer>().material = 視覺材質;
        變形視線範圍 = visualObj.AddComponent<MeshFilter>();
        視線範圍模型 = new Mesh();

        視覺角度 *= Mathf.Deg2Rad;
    }

    void Update()
    {
        繪製並搜尋目標();
    }

    void 繪製並搜尋目標()
    {
        int[] 三角形 = new int[(視線圓弧點 - 1) * 3];
        Vector3[] 扇形頂點 = new Vector3[視線圓弧點 + 1];

        float 固定高度 = 0.1f; // 讓模型稍微浮起，避免與地面閃爍
        扇形頂點[0] = new Vector3(0, 固定高度, 0);

        float 目前角度 = -視覺角度 / 2;
        float 角度增強 = 視覺角度 / (視線圓弧點 - 1);

        Vector3 射線起點 = transform.position + Vector3.up * 1.0f; // 從腰部高度發射

        GameObject closestEnemy = null;
        float shortestDistance = 視覺距離;

        for (int i = 0; i < 視線圓弧點; i++)
        {
            float sine = Mathf.Sin(目前角度);
            float cosine = Mathf.Cos(目前角度);

            // 計算方向
            Vector3 射線方向 = (transform.forward * cosine) + (transform.right * sine);
            Vector3 頂點方向 = (Vector3.forward * cosine) + (Vector3.right * sine);

            if (Physics.Raycast(射線起點, 射線方向, out RaycastHit hit, 視覺距離, 視線遮蔽圖層))
            {
                扇形頂點[i + 1] = 頂點方向 * hit.distance + new Vector3(0, 固定高度, 0);

                // 邏輯篩選：如果是敵人，且比目前的目標近
                if (hit.transform.CompareTag("Enemy"))
                {
                    if (hit.distance < shortestDistance)
                    {
                        shortestDistance = hit.distance;
                        closestEnemy = hit.transform.gameObject;
                    }
                }
            }
            else
            {
                扇形頂點[i + 1] = 頂點方向 * 視覺距離 + new Vector3(0, 固定高度, 0);
            }
            目前角度 += 角度增強;
        }

        // 更新模型
        更新Mesh(扇形頂點, 三角形);

        // 設定最終結果
        處理目標結果(closestEnemy, shortestDistance);
    }

    void 更新Mesh(Vector3[] vertices, int[] triangles)
    {
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        視線範圍模型.Clear();
        視線範圍模型.vertices = vertices;
        視線範圍模型.triangles = triangles;
        變形視線範圍.mesh = 視線範圍模型;
    }

    void 處理目標結果(GameObject closestEnemy, float distance)
    {
        if (closestEnemy != null)
        {
            最近的敵人 = closestEnemy.transform;
            Vector3 pos = closestEnemy.transform.position;
            pos.y = 1.4f;
            最終目標.transform.position = pos;
        }
        else
        {
            最近的敵人 = null;
            最終目標.transform.localPosition = 原始相對座標;
        }
    }
}

//using TMPro;
//using UnityEngine;

//public class FindTarget : MonoBehaviour
//{
//    public Transform 最近的敵人;
//    public Vector3 敵人座標;
//    public float 最近距離;
//    public GameObject 最終目標;

//    private void Start()
//    {
//        最終目標 = GameObject.Find("Target");
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        // 1. 搜尋所有具有指定 Tag 的遊戲物件
//        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

//        // 如果場景中沒有任何敵人
//        if (enemies.Length == 0)
//        {
//            最近的敵人 = null;
//            敵人座標 = Vector3.zero;
//            最近距離 = 0f;
//            return;
//        }

//        // 初始設定最短距離為無限大，以便第一次比較時一定會被取代
//        float shortestDistance = float.MaxValue;
//        GameObject closestEnemy = null;

//        // 2. 遍歷所有敵人
//        foreach (GameObject enemy in enemies)
//        {
//            // 3. 計算當前物體與敵人的距離
//            float distance = Vector3.Distance(transform.position, enemy.transform.position);

//            // 4. 判斷是否為目前找到的最近目標
//            if (distance < shortestDistance)
//            {
//                shortestDistance = distance;
//                closestEnemy = enemy; // 儲存這個敵人物件
//            }
//            if(distance > 10f)
//            {
//                closestEnemy = null;
//            }
//        }

//        // 5. 設定最終結果
//        if (closestEnemy != null)
//        {
//            最近的敵人 = closestEnemy.transform;
//            // 設定目標座標
//            敵人座標 = closestEnemy.transform.position;
//            最近距離 = shortestDistance;
//            敵人座標.y = 1.4f;
//            最終目標.transform.position = 敵人座標;
//        }
//        else
//        {
//            Vector3 原始座標 = new Vector3(0,1.6f,1.72f);
//            最終目標.transform.localPosition = 原始座標;
//        }
//    }
//}