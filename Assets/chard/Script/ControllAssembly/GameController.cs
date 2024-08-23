using UnityEngine;

public class GameController : MonoBehaviour
{
    public ControlledFreeLookCamera cameraController;
    public Transform firstObject; // Reference to the first object in the sequence

    void Start()
    {
        if (cameraController == null)
        {
            Debug.LogError("CameraController is not assigned.");
            return;
        }

        if (firstObject == null)
        {
            Debug.LogError("First object is not assigned.");
            return;
        }

        var firstHandler = firstObject.GetComponent<BaseObjectClickHandler>();
        if (firstHandler != null)
        {
            firstHandler.Activate();
            cameraController.SetTarget(firstObject); // Pass Transform instead of GameObject
        }
        else
        {
            Debug.LogError("First object does not have a BaseObjectClickHandler component.");
        }
    }
}
