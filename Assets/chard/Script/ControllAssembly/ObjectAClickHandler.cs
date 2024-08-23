using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectAClickHandler : BaseObjectClickHandler
{
    public Transform nextObject; // Change from GameObject to Transform
    public ControlledFreeLookCamera cameraController;
    public GameObject nowObject;
    private Outline highlight;
    

    protected override void OnAppear()
    {
        // Disable the current highlight if it exists
        if (highlight != null)
        {
            highlight.enabled = false;
            highlight = null;
        }

        if (nextObject != null)
        {
            var nextHandler = nextObject.GetComponent<BaseObjectClickHandler>();
            if (nextHandler != null)
            {
                nextHandler.Activate();
                if (cameraController != null)
                {
                    cameraController.SetTarget(nextObject);

                    // Enable outline for the next object and set it as the new highlight
                    Outline nextOutline = nextObject.gameObject.GetComponent<Outline>();
                    highlight = nowObject.GetComponent<Outline>();
                    if (nextOutline == null)
                    {
                        nextOutline = nextObject.gameObject.AddComponent<Outline>();
                        nextOutline.OutlineColor = Color.magenta;
                        nextOutline.OutlineWidth = 7.0f;
                    }
                    nextOutline.enabled = true;
                    highlight.enabled = false; // Update the highlight reference
                }
            }
        }
    }
}
