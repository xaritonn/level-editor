using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    [SerializeField]
    float mouseSensivity = 1.8f;

    [SerializeField]
    float movementSpeed = 10f;

    [SerializeField]
    KeyCode moveUp = KeyCode.E;

    [SerializeField]
    KeyCode moveDown = KeyCode.Q;

    [SerializeField]
    float speedAccelerationFactor = 1.5f;

    float currentIncrease = 1;
    float currentIncreaseMem = 0;

    void CalculateCurrentIncrease(bool moving)
    {
        currentIncrease = Time.deltaTime;

        if (!moving)
        {
            currentIncreaseMem = 0;
            return;
        }

        currentIncreaseMem += Time.deltaTime * (speedAccelerationFactor - 1);
        currentIncrease = Time.deltaTime + Mathf.Pow(currentIncreaseMem, 3) * Time.deltaTime;
    }

    void Update()
    {
        // Movement
        Vector3 deltaPosition = Vector3.zero;
        float currentSpeed = movementSpeed;

        if (Input.GetKey(KeyCode.W))
            deltaPosition += transform.forward;

        if (Input.GetKey(KeyCode.S))
            deltaPosition -= transform.forward;

        if (Input.GetKey(KeyCode.A))
            deltaPosition -= transform.right;

        if (Input.GetKey(KeyCode.D))
            deltaPosition += transform.right;

        if (Input.GetKey(moveUp))
            deltaPosition += transform.up;

        if (Input.GetKey(moveDown))
            deltaPosition -= transform.up;

        // Calc acceleration
        CalculateCurrentIncrease(deltaPosition != Vector3.zero);

        transform.position += deltaPosition * currentSpeed * currentIncrease;

        // Rotation
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * mouseSensivity, Vector3.right);

            transform.rotation = Quaternion.Euler(
                transform.eulerAngles.x,
                transform.eulerAngles.y + Input.GetAxis("Mouse X") * mouseSensivity,
                transform.eulerAngles.z
            );
        }
    }
}
