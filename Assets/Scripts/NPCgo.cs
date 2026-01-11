using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;

public class NPCgo : MonoBehaviour
{
    private NavMeshAgent 導航;
    private Animator 動畫器;
    public Transform 目標;
    public float 距離 = 0;

    public TextMeshPro 血量文字;
    public int 血量 = 100;
    int 原始血量;
    public Transform 血條;
    bool 開始攻擊 = false;
    float 攻擊間距 = 2f;
    float 下次可攻擊時間;
    public float 攻擊距離 = 1.2f;
    void Start()
    {
        導航 = GetComponent<NavMeshAgent>();
        動畫器 = GetComponent<Animator>();
        原始血量 = 血量;
        血量文字.text = 血量.ToString();
        導航.stoppingDistance = 攻擊距離;
    }
    void Update()
    {
        if (目標 != null)
        {
            導航.SetDestination(目標.position);
            距離 = Vector3.Distance(目標.position, this.transform.position);
            if (距離 <= 攻擊距離)
            {
                動畫器.SetBool("isWalk", false);
                開始攻擊 = true;
            }
            else
            {
                動畫器.SetBool("isWalk", true);
                開始攻擊 = false;
            }
            if (開始攻擊)
            {
                if (血量 <= 0) return;
                if (Time.time >= 下次可攻擊時間)
                {
                    動畫器.SetTrigger("isAttack");
                    下次可攻擊時間 = Time.time + 攻擊間距;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            血量--;
            if (血量 < 0) return;

            血量文字.text = 血量.ToString();
            float 血量比例 = (float)血量 / (float)原始血量;
            血條.localScale = new Vector3(血量比例, 1, 1);
            if (血量 <= 0)
            {
                動畫器.SetTrigger("isDead");
                Destroy(this.gameObject, 3f);
            }
            else
            {
                動畫器.SetTrigger("isHit");
            }
        }
    }
}

//using UnityEngine;
//using UnityEngine.AI;
//using TMPro;
//using Unity.VisualScripting.FullSerializer;
//using UnityEditor;

//public class NPCgo : MonoBehaviour
//{
//    private NavMeshAgent 導航;
//    private Animator 動畫器;
//    public Transform 目標;
//    public float 距離 = 0;

//    public TextMeshPro 血量文字;
//    public int 血量 = 100;
//    int 原始血量;
//    public Transform 血條;
//    bool 開始攻擊 = false;
//    float 攻擊間距 = 2f;
//    float 下次可攻擊時間; 

//    void Start()
//    {
//        導航 = GetComponent<NavMeshAgent>();
//        動畫器 = GetComponent<Animator>();
//        原始血量 = 血量;
//        血量文字.text = 血量.ToString();
//    }
//    void Update()
//    {
//        if (目標 != null)
//        {
//            導航.SetDestination(目標.position);
//            距離 = Vector3.Distance(目標.position, this.transform.position);
//            if (距離 <= 3.1f) 
//            { 
//                動畫器.SetBool("isWalk", false);
//                動畫器.SetBool("isAttack", true);
//            }
//            else
//            { 
//                動畫器.SetBool("isWalk", true); 
//                開始攻擊 =
//            }
//            if (Time.time >= 下次可攻擊時間)
//            {
//                攻擊時間 = Time.time;
//                if (攻擊時間 - 攻擊距離 < 0)
//                {

//                }

//            }
//        }
//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "Bullet") 
//        {
//            Destroy(other.gameObject);
//            血量--;
//            血量文字.text = 血量.ToString();
//            float 血量比例 = (float)血量 /(float)原始血量 ;
//            血條.localScale = new Vector3(血量比例, 1, 1);
//            if (血量 <= 0)
//            {
//                動畫器.SetTrigger("isDead");
//                Destroy(this.gameObject);
//            }
//            else
//            {
//                動畫器.SetTrigger("isHit");
//            }
//        }
//    }
//}
////-----------------------------------------------
////using UnityEngine;
////using UnityEngine.AI;

////public class NPCgo : MonoBehaviour
////{
////    Animator anim;
////    NavMeshAgent agent;
////    public Transform player;

////    // Start is called once before the first execution of Update after the MonoBehaviour is created
////    void Start()
////    {
////        anim = GetComponent<Animator>();
////        agent = GetComponent<NavMeshAgent>();
////    }

////    // Update is called once per frame
////    void Update()
////    {
////        if(player!= null)
////        {
////            agent.SetDestination(player.position);
////        }
////        if (Vector3.Distance(player.position, transform.position)>1.5f)
////        {
////            anim.SetBool("isWalk", true);
////        }
////        else
////        {
////            anim.SetBool("isWalk", false);
////        }
////    }
////}



