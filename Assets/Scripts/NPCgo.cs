using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Unity.VisualScripting.FullSerializer;

public class NPCgo : MonoBehaviour
{
    private NavMeshAgent 導航;
    private Animator 動畫器;
    public Transform 目標;
    public float 距離 = 0;

    public TextMeshPro 血量文字;
    public int 血量 = 100;
    public Transform 血條;
    

    void Start()
    {
        導航 = GetComponent<NavMeshAgent>();
        動畫器 = GetComponent<Animator>();
    }
    void Update()
    {
        if (目標 != null)
        {
            導航.SetDestination(目標.position);
            距離 = Vector3.Distance(目標.position, this.transform.position);
            if (距離 <= 3.1f) { 動畫器.SetBool("isWalk", false); }
            else { 動畫器.SetBool("isWalk", true); }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet") 
        {
            Destroy(other.gameObject);
            血量--;
            血量文字.text = 血量.ToString();
            float 血量比例 = (float)血量 / 100f;
            血條.localScale = new Vector3(血量比例, 1, 1);
            if(血量 <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

//using UnityEngine;
//using UnityEngine.AI;

//public class NPCgo : MonoBehaviour
//{
//    Animator anim;
//    NavMeshAgent agent;
//    public Transform player;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        anim = GetComponent<Animator>();
//        agent = GetComponent<NavMeshAgent>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(player!= null)
//        {
//            agent.SetDestination(player.position);
//        }
//        if (Vector3.Distance(player.position, transform.position)>1.5f)
//        {
//            anim.SetBool("isWalk", true);
//        }
//        else
//        {
//            anim.SetBool("isWalk", false);
//        }
//    }
//}



