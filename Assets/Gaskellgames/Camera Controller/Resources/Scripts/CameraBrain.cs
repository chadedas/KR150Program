using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public class CameraBrain : GGMonoBehaviour
    {
        #region Variables

        [HideInInspector]
        private enum cameraBlendStyle
        {
            Cut,
            FadeToColor,
            MoveToPosition
        }

        [SerializeField, RequiredField] private CameraRig activeCamera;
        [ReadOnly, SerializeField] private CameraRig previousCamera;

        [LineSeparator]
        
        [SerializeField] private cameraBlendStyle blendingStyle = cameraBlendStyle.Cut;
        [SerializeField] private cameraBlendStyle blendingStyleCheck;
        [SerializeField, CustomCurve(000, 179, 223, 255)] private AnimationCurve fadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));
        [SerializeField] private Color fadeColor = Color.black;
        [SerializeField, Range(0.01f, 3.0f)] private float fadeSpeed = 1.5f;
        [SerializeField] private bool fadeFullScreen = true;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField, CustomCurve(000, 179, 223, 255)] private AnimationCurve blendCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 0.5f, 1.5f, 1.5f), new Keyframe(1, 1));
        [SerializeField, Range(0.01f, 3.0f)] private float blendSpeed = 1.5f;
        private Transform blendTransform;
        private CameraLens blendLens;
        
        private bool isBlending = false;
        private bool triggerFade = false;
        private Texture2D texture;
        private float alpha = 0f;
        private float timer = 0f;
        private int fadeDirection = 0;

        [ReadOnly, SerializeField] private Transform follow;
        [ReadOnly, SerializeField] private Transform lookAt;
        [ReadOnly, SerializeField] private CameraLens lens;
        [ReadOnly, SerializeField] private CameraOrbits CameraOrbit;
        private CameraRig activeCameraCheck;
        private Camera cam;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game Loop

        private void Reset()
        {
            if (gameObject.GetComponent<Camera>() != null)
            {
                cam = GetComponent<Camera>();
                activeCameraCheck = null;
            }
        }

        private void Start()
        {
            InitializeVariables();
        }

        private void Update()
        {
            if (activeCamera != null)
            {
                if (activeCamera.IsFreeFlyCamera)
                {
                    UpdateCameraSettings();
                    transform.SetPositionAndRotation(activeCamera.transform.position, activeCamera.transform.rotation);
                }
            }
        }

        private void LateUpdate()
        {
            if(!isBlending)
            {
                UpdateCamera();    
            }
            else
            {
                BlendCamera();
            }
            
            if (triggerFade)
            {
                triggerFade = false;
                UpdateFade();
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region On Events

        public void OnGUI()
        {
            if(blendingStyle == cameraBlendStyle.FadeToColor)
            {
                // draw texture to screen
                if (alpha > 0f && fadeFullScreen)
                {
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
                }

                if (fadeDirection != 0)
                {
                    // set alpha based on timer value
                    timer += fadeDirection * Time.deltaTime * fadeSpeed;
                    alpha = fadeCurve.Evaluate(timer);

                    // apply alpha to screen texture
                    if (!fadeFullScreen && canvasGroup != null)
                    {
                        canvasGroup.alpha = 1 - alpha;
                    }
                    texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
                    texture.Apply();

                    // clamp alpha
                    if (alpha <= 0f || alpha >= 1f)
                    {
                        fadeDirection = 0;
                    }
                }
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        
        #region Editor / Debug

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            if (!Application.isPlaying && activeCamera != null)
            {
                transform.position = activeCamera.transform.position;
                transform.rotation = activeCamera.transform.rotation;

                if (cam == null)
                {
                    cam = GetComponent<Camera>();
                }
                else
                {
                    UpdateCameraSettings();
                }
                
                activeCameraCheck = activeCamera;
            }
        }

        #endregion
        
#endif

        //----------------------------------------------------------------------------------------------------

        #region Functions

        private void InitializeVariables()
        {
            cam = GetComponent<Camera>();
            alpha = 0f;
            texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();

            if (activeCamera != null)
            {
                blendTransform = activeCamera.transform;
                blendLens = activeCamera.Lens;
                previousCamera = activeCamera;
            }
        }

        private void UpdateCamera()
        {
            if (activeCamera != null)
            {
                if (activeCameraCheck != activeCamera)
                {
                    if (cam == null)
                    {
                        cam = GetComponent<Camera>();
                    }
                    else
                    {
                        if (blendingStyle == cameraBlendStyle.MoveToPosition)
                        {
                            timer = 0;
                            blendTransform.position = transform.position;
                            blendTransform.rotation = transform.rotation;
                            blendLens.verticalFOV = (int)cam.fieldOfView;
                            blendLens.nearClipPlane = cam.nearClipPlane;
                            blendLens.farClipPlane = cam.farClipPlane;
                            cam.cullingMask = activeCamera.Lens.cullingMask;
                            isBlending = true;
                        }
                        else if (blendingStyle == cameraBlendStyle.FadeToColor)
                        {
                            // fade to 'black'
                            if (CheckScreenClear() && previousCamera != null)
                            {
                                triggerFade = true;
                            }

                            if (CheckScreenFaded())
                            {
                                UpdateCameraSettings();

                                // unfade from 'black'
                                triggerFade = true;
                            }
                        }
                        else // blendingStyle == cameraBlendStyle.Cut
                        {
                            UpdateCameraSettings();
                        }
                    }
                }
                else
                {
                    UpdateCameraSettings();
                    transform.SetPositionAndRotation(activeCamera.transform.position, activeCamera.transform.rotation);
                }
            }
        }

        private void BlendCamera()
        {
            // evaluate blend completion
            float completion = blendCurve.Evaluate(timer);
            timer += (blendSpeed * Time.deltaTime);
            
            // update position
            transform.position = Vector3.Lerp(blendTransform.position, activeCamera.transform.position, completion);

            // update rotation
            transform.rotation = Quaternion.Lerp(blendTransform.rotation, activeCamera.transform.rotation, completion);
            
            // update CameraLens
            CameraLens activeCameraLens = activeCamera.Lens;
            cam.fieldOfView =  Mathf.Lerp(blendLens.verticalFOV, activeCameraLens.verticalFOV, completion);
            cam.nearClipPlane =  Mathf.Lerp(blendLens.nearClipPlane, activeCameraLens.nearClipPlane, completion);
            cam.farClipPlane =  Mathf.Lerp(blendLens.farClipPlane, activeCameraLens.farClipPlane, completion);
            
            if (completion == 1.0f)
            {
                isBlending = false;
                blendTransform = activeCamera.transform;
                blendLens = activeCamera.Lens;
                activeCameraCheck = activeCamera;
            }
        }

        private void UpdateCameraSettings()
        {
            CameraLens tempLens = activeCamera.Lens;
            cam.fieldOfView = tempLens.verticalFOV;
            cam.nearClipPlane = tempLens.nearClipPlane;
            cam.farClipPlane = tempLens.farClipPlane;
            cam.cullingMask = tempLens.cullingMask;

            activeCameraCheck = activeCamera;
        }

        private void UpdateFade()
        {
            if (fadeDirection == 0)
            {
                if (alpha >= 1f) // Fully faded out
                {
                    alpha = 1f;
                    timer = 0f;
                    fadeDirection = 1;
                }
                else // Fully faded in
                {
                    alpha = 0f;
                    timer = 1f;
                    fadeDirection = -1;
                }
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Getters / Setters

        public string GetBlendingStyle() { return blendingStyle.ToString(); }
        
        public CameraRig GetActiveCamera() { return activeCamera; }

        public void SetActiveCamera(CameraRig newActiveCamera)
        {
            if (activeCamera != newActiveCamera)
            {
                previousCamera = activeCamera;
            }
            activeCamera = newActiveCamera;
        }

        public CameraRig GetPreviousCamera() { return previousCamera; }

        public void SetPreviousCamera() { activeCamera = previousCamera; }

        public CanvasGroup GetCanvasGroup() { return canvasGroup; }

        public void SetCanvasGroup(CanvasGroup value) { canvasGroup = value; }

        public bool CheckScreenFaded() { return alpha >= 1; }

        public bool CheckScreenClear() { return alpha <= 0; }

        public void ActivateFade() { triggerFade = true; }

        #endregion

    } // class end
}
