using UnityEngine;
using UnityEngine.UIElements;

public class moveCamera1 : MonoBehaviour
{
    [SerializeField] private float 速度 = 3.5f;
    public float rotationSpeed = 5f; // 旋轉速度倍率
    public bool lockY = true;        // 是否鎖定 Y 軸
    //public GameObject myTaxi;
    //private Vector3 myPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * ME/* Time.dE/taTime):
        //myPosition = transform.position;
        //myPosition.x = myPosition.x + 5;
        //myPosition.y = myPosition.y + 2;
        //my Taxi. transform.position = this.transform position;
        //transform.Translate(Vector3.forward * 速度 * Time.deltaTime);

        //滑鼠移動角度
        float mouseX = Input.GetAxis("Mouse X"); // 滑鼠左右移動
        float mouseY = Input.GetAxis("Mouse Y"); // 滑鼠上下移動

        if (lockY)
        {
            // 只用 X 軸來旋轉 Y 軸方向（左右轉動）
            transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);
        }
        else
        {
            // 使用 X, Y 同時旋轉
            transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, mouseY * rotationSpeed, Space.World);
        }


        //位移 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) //往前 forward z
        {
            transform.Translate(Vector3.forward * 速度 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //往後
        {
            transform.Translate(Vector3.back * 速度 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //往左
        {
            transform.Translate(Vector3.left * 速度 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //往右
        {
            transform.Translate(Vector3.right * 速度 * Time.deltaTime);
        }
    }
}

