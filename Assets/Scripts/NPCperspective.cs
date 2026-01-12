using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NPCperspective : MonoBehaviour
{
    public Material 視覺材質;
    public float 視覺距離 = 2f;
    public float 視覺角度 = 120f;
    public LayerMask 視線遮蔽圖層;
    public int 視線圓弧點 = 120; //數字少則頂點少，像多邊型。數字大像圓弧形。

    //由程式建立
    Mesh 視線範圍模型;
    MeshFilter 變形視線範圍;
    Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = 視覺材質; //新增元件，並連結材質
        變形視線範圍 = transform.AddComponent<MeshFilter>();
        視線範圍模型 = new Mesh();
        視覺角度 *= Mathf.Deg2Rad; //將角度轉換為弧度
    }
    // Update is called once per frame
    void Update()
    {
        繪製視線範圍();
    }
    void 繪製視線範圍()
    {
        int[] 三角形 = new int[(視線圓弧點 - 1) * 3];
        Vector3[] 扇形頂點 = new Vector3[視線圓弧點 + 1];
        扇形頂點[0] = new Vector3(0f, 1f, 0f);
        float 目前角度 = -視覺角度 / 2;
        float 角度增強 = 視覺角度 / (視線圓弧點 - 1); //計算每個頂點之間的角度增量
        float Sine;
        float Cosine;
        Vector3 射線起點 = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        float 固定高度 = 1f;

        for (int i = 0; i < 視線圓弧點; i++)
        {
            Sine = Mathf.Sin(目前角度);
            Cosine = Mathf.Cos(目前角度);
            Vector3 射線方向 = (transform.forward * Cosine) + (Vector3.right * Sine);
            Vector3 增強方向 = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            //Debug.DrawRay(射線起點,射線方向 * 2, Color.red);
            if (Physics.Raycast(射線起點, 射線方向, out RaycastHit hit, 視覺距離, 視線遮蔽圖層))
            {
                // 修改點：強制 Y 為固定高度，只取 X 與 Z 的方向向量
                Vector3 平面方向 = new Vector3(增強方向.x, 0, 增強方向.z);
                扇形頂點[i + 1] = 平面方向 * hit.distance + new Vector3(0, 固定高度, 0);

                //print(hit.transform.tag);
                if (hit.transform.tag == "Player")
                {
                    //print("發現玩家");
                    playerPos = hit.transform;
                    StartCoroutine(發現後的暫停());
                }
            }
            else
            {
                // 修改點：同樣強制 Y 高度
                Vector3 平面方向 = new Vector3(增強方向.x, 0, 增強方向.z);
                扇形頂點[i + 1] = 平面方向 * 視覺距離 + new Vector3(0, 固定高度, 0);
            }
            目前角度 += 角度增強;
        }
        for (int i = 0, j = 0; i < 三角形.Length; i += 3, j++)
        {
            三角形[i] = 0;
            三角形[i + 1] = j + 1;
            三角形[i + 2] = j + 2;
        }
        視線範圍模型.Clear();
        視線範圍模型.vertices = 扇形頂點;
        視線範圍模型.triangles = 三角形;
        變形視線範圍.mesh = 視線範圍模型;
    }
    IEnumerator 發現後的暫停()
    {
        yield return new WaitForSeconds(1f);
        //GetComponent<敵人的巡邏>().準備攻擊玩家();
        GetComponent<敵人的巡邏1>().發現玩家 = true;
        GetComponent<敵人的巡邏1>().玩家位置 = playerPos;
    }
}