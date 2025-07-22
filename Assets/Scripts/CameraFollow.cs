using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public Transform target;         // Игрок
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Смещение камеры от игрока
    public float smoothSpeed = 0.125f; // Скорость сглаживания

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
