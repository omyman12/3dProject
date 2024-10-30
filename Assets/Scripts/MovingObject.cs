using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public GameObject gameobject;
    private bool isMovingRight;
    public float moveSpeed = 1f;
    public float limitDistance = 2f;

    private void Update()
    {
        MoveObject();
    }

    void MoveObject()
    {
        // 이동 방향에 따라 위치 변경
        if (isMovingRight)
        {
            gameobject.transform.localPosition += Vector3.right * moveSpeed * Time.deltaTime;
            if (gameobject.transform.localPosition.x >= limitDistance)
            {
                isMovingRight = false; // 오른쪽 한계에 도달 시 방향 전환
            }
        }
        else
        {
            gameobject.transform.localPosition += Vector3.left * moveSpeed * Time.deltaTime;
            if (gameobject.transform.localPosition.x <= 0)
            {
                isMovingRight = true; // 왼쪽 한계에 도달 시 방향 전환
            }
        }
    }
}
