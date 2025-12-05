using UnityEngine;

public class MovingTargett : MonoBehaviour
{
    [Tooltip("來回移動的總距離。目標將在 -distance 和 +distance 之間移動。")]
    public float distance = 30f; // 您要求的左右各移動 30 (總範圍 60)

    [Tooltip("完成一次來回（從一端到另一端再回來）所需的時間（秒）。")]
    public float cycleTime = 3f; // 您要求的每 3 秒來回一次

    // --- 內部變數 ---

    private Vector3 initialPosition; // 儲存物體的初始位置

    // --- Unity 生命週期函式 ---
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float pingPongValue = Mathf.PingPong(Time.time * (distance * 2f / cycleTime), distance);

        float newXPosition = initialPosition.x + (pingPongValue - (distance / 2f)) * 2f;

        transform.position = new Vector3(
            newXPosition,
            initialPosition.y, // Y 軸保持不變
            initialPosition.z  // Z 軸保持不變
        );
    }
}