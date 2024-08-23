using UnityEngine;
using Cinemachine;

public class ControlledFreeLookCamera : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    private Transform target;

    private bool isRightMouseButtonHeld = false;
    private Vector3 previousMousePosition;

    void Start()
    {
        if (freeLookCamera == null)
        {
            Debug.LogError("FreeLookCamera not assigned.");
            return;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRightMouseButtonHeld = true;
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseButtonHeld = false;
        }

        if (!isRightMouseButtonHeld)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 0f;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0f;
        }
        else
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 800f;
            freeLookCamera.m_YAxis.m_MaxSpeed = 6f;

            if (target != null)
            {
                Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;
                float xRotation = deltaMousePosition.x * 0.1f;
                float yRotation = -deltaMousePosition.y * 0.1f;

                Quaternion rotation = Quaternion.Euler(yRotation, xRotation, 0f);
                transform.position = target.position + rotation * (transform.position - target.position);
                transform.LookAt(target);

                previousMousePosition = Input.mousePosition;
            }
            else
            {
                Debug.LogWarning("Target is not assigned.");
            }
        }
    }

    public void SetTarget(Transform target)
    {
        if (freeLookCamera == null)
        {
            Debug.LogError("FreeLookCamera is not assigned.");
            return;
        }

        if (target == null)
        {
            Debug.LogError("Target is not assigned.");
            return;
        }

        this.target = target; // Set the internal target variable
        freeLookCamera.Follow = target;
        freeLookCamera.LookAt = target;

        // Adjust the camera's position and rotation to look down from above
        Vector3 offset = new Vector3(0f, 15f, -10f); // Adjust as needed
        freeLookCamera.transform.position = target.position + offset;
        
        // Calculate rotation to look at the target from the offset position
        Vector3 direction = (target.position - freeLookCamera.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        freeLookCamera.transform.rotation = lookRotation;

        // Optional: Ensure that the free look camera is using the right angles for its rigs
        freeLookCamera.m_YAxis.Value = 0.5f; // 0 is bottom rig, 1 is top rig, 0.5 is middle
    }
}
