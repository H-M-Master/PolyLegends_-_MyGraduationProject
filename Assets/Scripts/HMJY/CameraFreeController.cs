using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraFreeController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float scrollSpeed = 100f;
    public float minY = 10f;
    public float maxY = 30f;

    public float boundaryBuffer = 5f; // 缓冲区宽度
    public float followSmoothTime = 0.3f;

    private Vector2 xLimit;
    private Vector2 zLimit;

    private bool isFollowingPlayer = false;
    private GameObject player;
    private Vector3 followVelocity = Vector3.zero;

    void Start()
    {
        CalculateNavMeshBounds();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        HandleCameraControl();
        HandleCameraFollowToggle();
    }

    void HandleCameraControl()
    {
        // 如果正在跟随玩家，不允许自由控制
        if (isFollowingPlayer && player != null)
        {
            Vector3 target = new Vector3(
                Mathf.Clamp(player.transform.position.x, xLimit.x, xLimit.y),
                Mathf.Clamp(transform.position.y, minY, maxY),
                Mathf.Clamp(player.transform.position.z, zLimit.x, zLimit.y)
            );

            transform.position = Vector3.SmoothDamp(transform.position, target, ref followVelocity, followSmoothTime);
            return;
        }

        Vector3 pos = transform.position;
        Vector3 direction = Vector3.zero;

        // 控制方向（方向键 + 鼠标贴边）
        if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - 5)
            direction += Vector3.right;
        if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= 5)
            direction += Vector3.left;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= 5)
            direction += Vector3.forward;
        if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - 5)
            direction += Vector3.back;

        // 方向归一化
        if (direction != Vector3.zero)
            direction.Normalize();

        float xDamp = GetEdgeDamping(pos.x, xLimit);
        float zDamp = GetEdgeDamping(pos.z, zLimit);

        Vector3 finalMove = new Vector3(direction.x * xDamp, 0, direction.z * zDamp) * moveSpeed * Time.deltaTime;
        pos += finalMove;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        pos.x = Mathf.Clamp(pos.x, xLimit.x, xLimit.y);
        pos.z = Mathf.Clamp(pos.z, zLimit.x, zLimit.y);

        transform.position = pos;
    }

    void HandleCameraFollowToggle()
    {
        // 按下空格：瞬时启动平滑移动到玩家位置
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player != null)
            {
                StopAllCoroutines();
                StartCoroutine(MoveCameraToPlayer(player.transform.position));
            }
        }

        // 持续按住空格时：保持跟随
        if (Input.GetKey(KeyCode.Space))
        {
            isFollowingPlayer = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isFollowingPlayer = false;
        }
    }

    IEnumerator MoveCameraToPlayer(Vector3 playerPosition)
    {
        Vector3 start = transform.position;
        Vector3 target = new Vector3(
            Mathf.Clamp(playerPosition.x, xLimit.x, xLimit.y),
            Mathf.Clamp(start.y, minY, maxY),
            Mathf.Clamp(playerPosition.z, zLimit.x, zLimit.y)
        );

        float duration = 0.5f;
        float time = 0;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }

    float GetEdgeDamping(float current, Vector2 limit)
    {
        float distanceToEdge = Mathf.Min(current - limit.x, limit.y - current);

        if (distanceToEdge >= boundaryBuffer)
            return 1f;

        if (distanceToEdge <= 0)
            return 0f;

        return distanceToEdge / boundaryBuffer;
    }

    void CalculateNavMeshBounds()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        if (navMeshData.vertices == null || navMeshData.vertices.Length == 0)
        {
            Debug.LogWarning("⚠ 未检测到 NavMesh 数据！");
            return;
        }

        Vector3 min = navMeshData.vertices[0];
        Vector3 max = navMeshData.vertices[0];

        foreach (Vector3 v in navMeshData.vertices)
        {
            if (v.x < min.x) min.x = v.x;
            if (v.z < min.z) min.z = v.z;
            if (v.x > max.x) max.x = v.x;
            if (v.z > max.z) max.z = v.z;
        }

        xLimit = new Vector2(min.x, max.x);
        zLimit = new Vector2(min.z, max.z);

        Debug.Log($"📘 NavMesh 边界设置完成 X: {xLimit}, Z: {zLimit}");
    }
}
