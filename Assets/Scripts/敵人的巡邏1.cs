using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class 敵人的巡邏1 : MonoBehaviour
{
    /*
     NavMesh Agent 導航用
     Transform 巡邏點座標 陣列
     動畫
     */
    NavMeshAgent 導航;
    public Transform[] 目標; //不預設容量
    public int 第幾個目標 = 0;

    Animator 動畫控制器;

    public bool 發現玩家 = false;
    public Transform 玩家位置;

    Vector3 方向;
    Quaternion 旋轉;
    void Start()
    {
        動畫控制器 = GetComponent<Animator>();
        導航 = GetComponent<NavMeshAgent>();
        導航.SetDestination(目標[第幾個目標].position);
    }
    void Update()
    {
        if (發現玩家)
        {
            導航.stoppingDistance = 2.5f;
            if (Vector3.Distance(this.transform.position, 玩家位置.position) < 導航.stoppingDistance)
            {
                動畫控制器.SetTrigger("isAttack");
                動畫控制器.SetBool("isWalk", false);
                //transform.LookAt(玩家位置.position);
                //transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
                方向 = this.transform.position - 玩家位置.position;
                旋轉 = Quaternion.LookRotation(方向 * -1, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, 旋轉, 10 * Time.deltaTime);
                this.transform.eulerAngles = new Vector3(0f, this.transform.eulerAngles.y, 0f);

            }
            else
            {
                if (Vector3.Distance(this.transform.position, 玩家位置.position) > 5)
                {
                    導航.stoppingDistance = 0.5f;
                    發現玩家 = false;
                    換目標();
                }
                else
                {
                    導航.SetDestination(玩家位置.position);
                    動畫控制器.SetBool("isWalk", true);
                }
            }
        }
        else
        {
            // --- 巡邏狀態 ---
            導航.stoppingDistance = 0.5f; // 確保巡邏時會很靠近目標點

            // 檢查是否抵達目標 (使用 remainingDistance 會比自測距離更精準)
            if (!導航.pathPending && 導航.remainingDistance < 導航.stoppingDistance)
            {
                換目標();
            }
            else
            {
                動畫控制器.SetBool("isWalk", true);
            }
        }


    }
    void 換目標()
    {
        第幾個目標 = Random.Range(0, 目標.Length);
        導航.SetDestination(目標[第幾個目標].position);
    }

    public void 準備攻擊玩家()
    {
        StartCoroutine(等一秒());
    }
    IEnumerator 等一秒()
    {
        yield return new WaitForSeconds(1f);
        動畫控制器.SetTrigger("isAtttack");
    }
}