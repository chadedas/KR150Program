using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Gaskellgames.InputEventSystem;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public class CameraFreelookRig : GGMonoBehaviour
    {
        #region Variables

        [SerializeField]
        private bool lockCursorOnLoad;
        
        [SerializeField]
        private bool cameraCollisions;
        
        [SerializeField]
        private bool customInputAction;
        
        [SerializeField, LineSeparator]
        private InputActionProperty moveCamera;
        
        [SerializeField, LineSeparator, RequiredField]
        private GMKInputController gmkInputController;
        
        [SerializeField, LineSeparator]
        private Transform follow;
        
        [SerializeField]
        private CameraOrbits cameraOrbit;

        [SerializeField, Range(0, 100)]
        private int xSensitivity = 80;
        
        [SerializeField, Range(0, 100)]
        private int ySensitivity = 80;
        
        [SerializeField, Range(-180, 180)]
        private float rotationOffset = 0f;
        
        [SerializeField, Range(0, 1)]
        private float collisionOffset = 0.2f;
        
        [SerializeField]
        private LayerMask collisionLayers = default;
        
        
        private UserInputs userInputs;
        
        private Vector3 freelookCameraTargetPosition;
        private Vector3 freelookCameraPosition;
        private bool collisionDetected;
        private bool isRightClick = false;

        
        private float xRotation;
        private float yRotation;
        private float xClampTop;
        private float xClampBottom;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game Loop

        private void OnEnable()
        {
            moveCamera.action.Enable();
        }

        private void OnDisable()
        {
            moveCamera.action.Disable();
        }

 private void Start()
        {
            // lock cursor to screen
            if (lockCursorOnLoad)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
private void Update()
{
    // ตรวจสอบสถานะของปุ่มคลิกขวา
    bool isRightClick = Mouse.current.rightButton.isPressed;

    // แสดง/ซ่อนเคอร์เซอร์ตามการคลิกขวา
    Cursor.visible = !isRightClick;

    // ตั้งค่าการล็อกเคอร์เซอร์เมื่อล็อกเคอร์เซอร์เป็นจริง
    Cursor.lockState = isRightClick ? CursorLockMode.Locked : CursorLockMode.None;

    GetUserInputs();
    GetViewInput();
    UpdateFollowPosition();

    if (follow != null)
    {
        transform.position = follow.position + new Vector3(0, cameraOrbit.rigOffset, 0);
    }
}


        #endregion

        //----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

        #region Debug [Editor]

        private void Reset()
        {
            StartCoroutine(PauseForSeconds(0.1f));
        }

        private IEnumerator PauseForSeconds(float value)
        {
            yield return new WaitForSeconds(value);

            SetCameraOrbit(0, 3, -1, 2, 4, 3);
            CameraRig newCameraRig = GetComponentInChildren<CameraRig>();
            GameObject newFreelookRig;

            if (newCameraRig == null)
            {
                newFreelookRig = new GameObject("CameraRig (Freelook)");
                newFreelookRig.transform.parent = gameObject.transform;
                newFreelookRig.transform.localPosition = Vector3.zero;
                CameraRig cameraRig = newFreelookRig.AddComponent<CameraRig>();
                cameraRig.FreelookRig = this;
            }
            else
            {
                newCameraRig.FreelookRig = this;
            }
        }

        protected override void OnDrawGizmos()
        {
            UpdateFollowPosition();
            if (follow != null)
            {
                transform.position = follow.position + new Vector3(0, cameraOrbit.rigOffset, 0);
            }
            UpdateEditorValidation();
            
            base.OnDrawGizmos();
        }

        protected override void OnDrawGizmosConditional(bool selected)
        {
            DrawCameraGizmos();
        }

        private void UpdateEditorValidation()
        {
            if(cameraOrbit.height.up < 0)
            {
                cameraOrbit.height.up = 0;
            }

            if(0 < cameraOrbit.height.down)
            {
                cameraOrbit.height.down = 0;
            }

            if(cameraOrbit.radius.top < 0.1)
            {
                cameraOrbit.radius.top = 0.1f;
            }

            if(cameraOrbit.radius.middle < 0.1)
            {
                cameraOrbit.radius.middle = 0.1f;
            }

            if(cameraOrbit.radius.bottom < 0.1)
            {
                cameraOrbit.radius.bottom = 0.1f;
            }
        }

        private void DrawCameraGizmos()
        {
            // set gizmo matrix to local transform
            Matrix4x4 resetMatrix = Gizmos.matrix;
            Gizmos.matrix = gameObject.transform.localToWorldMatrix;

            // draw camera orbits
            UnityEditor.Handles.color = Color.cyan;
            UnityEditor.Handles.DrawWireDisc(new Vector3(transform.position.x, transform.position.y + cameraOrbit.height.up, transform.position.z), transform.up, cameraOrbit.radius.top);
            UnityEditor.Handles.DrawWireDisc(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.up, cameraOrbit.radius.middle);
            UnityEditor.Handles.DrawWireDisc(new Vector3(transform.position.x, transform.position.y + cameraOrbit.height.down, transform.position.z), transform.up, cameraOrbit.radius.bottom);

            // draw freelook current orbit
            if(collisionDetected) { UnityEditor.Handles.color = Color.red; }
            else { UnityEditor.Handles.color = Color.yellow; }
            float currentRadius = Vector3.Magnitude(new Vector2(freelookCameraTargetPosition.x, freelookCameraTargetPosition.z));
            UnityEditor.Handles.DrawWireDisc(new Vector3(transform.position.x, transform.position.y + freelookCameraTargetPosition.y, transform.position.z), transform.up, currentRadius);

            // draw line between orbits
            Gizmos.color = Color.cyan;
            Vector3 top = Quaternion.AngleAxis(-rotationOffset, Vector3.up) * new Vector3(cameraOrbit.radius.top, cameraOrbit.height.up, 0);
            Vector3 middle = Quaternion.AngleAxis(-rotationOffset, Vector3.up) * new Vector3(cameraOrbit.radius.middle, 0, 0);
            Vector3 bottom = Quaternion.AngleAxis(-rotationOffset, Vector3.up) * new Vector3(cameraOrbit.radius.bottom, cameraOrbit.height.down, 0);
            Gizmos.DrawLine(top, middle);
            Gizmos.DrawLine(middle, bottom);

            // draw line for rotation directions
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Vector3.zero, freelookCameraPosition);

            if (collisionDetected) { UnityEditor.Handles.color = Color.red; }
            else { UnityEditor.Handles.color = Color.yellow; }
            Vector3 lineOut = new Vector3(freelookCameraTargetPosition.x, 0, freelookCameraTargetPosition.z);
            Gizmos.DrawLine(Vector3.zero, lineOut);
            Gizmos.DrawLine(lineOut, freelookCameraTargetPosition);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(freelookCameraPosition, freelookCameraTargetPosition);

            // reset gizmo matrix to public position
            Gizmos.matrix = resetMatrix;
        }

        #endregion

#endif

        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private void GetUserInputs()
        {
            if (gmkInputController != null)
            {
                userInputs = gmkInputController.GetUserInputs();
            }
        }

        private void UpdateFollowPosition()
        {
            UpdateTargetPosition();
            UpdateCameraCollisions();
        }

        private void UpdateCameraCollisions()
        {
            if(cameraCollisions)
            {
                float maxDistance = freelookCameraTargetPosition.magnitude;
                Vector3 direction = freelookCameraTargetPosition.normalized;

                RaycastHit hit;
                Ray ray = new Ray(transform.position, direction);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionLayers))
                {
                    if(hit.distance < maxDistance)
                    {
                        freelookCameraPosition = direction * (hit.distance - collisionOffset);
                        collisionDetected = true;
                    }
                    else
                    {
                        freelookCameraPosition = freelookCameraTargetPosition;
                        collisionDetected = false;
                    }
                }
                else
                {
                    freelookCameraPosition = freelookCameraTargetPosition;
                    collisionDetected = false;
                }
            }
            else
            {
                freelookCameraPosition = freelookCameraTargetPosition;
                collisionDetected = false;
            }
        }
        private void UpdateTargetPosition()
        {
            float angleTop = GetAngleOnRightAngleTriangle(Mathf.Abs(cameraOrbit.radius.top), Mathf.Abs(cameraOrbit.height.up));
            float angleBottom = GetAngleOnRightAngleTriangle(Mathf.Abs(cameraOrbit.radius.bottom), Mathf.Abs(cameraOrbit.height.down));

            xClampTop = (90.0f - angleTop) * Mathf.Sign(cameraOrbit.height.up);
            xClampBottom = (90.0f - angleBottom) * Mathf.Sign(cameraOrbit.height.down);

            float magnitudeTop = Vector3.Magnitude(new Vector2(cameraOrbit.radius.top, cameraOrbit.height.up));
            float magnitudeBottom = Vector3.Magnitude(new Vector2(cameraOrbit.radius.bottom, cameraOrbit.height.down));

            float distance; ;
            if(0 < xRotation)
            {
                float ratio = xRotation / xClampTop;
                distance = (magnitudeTop * ratio) + (cameraOrbit.radius.middle * (1.0f - ratio));
            }
            else if(0 > xRotation)
            {
                float ratio = xRotation / xClampBottom;
                distance = (cameraOrbit.radius.middle * (1.0f - ratio)) + (magnitudeBottom * ratio);
            }
            else
            {
                distance = cameraOrbit.radius.middle;
            }

            float longitude = (rotationOffset - yRotation) * Mathf.Deg2Rad;
            float latitude = xRotation * Mathf.Deg2Rad;
            float equatorX = Mathf.Cos(longitude);
            float equatorZ = Mathf.Sin(longitude);
            float multiplier = Mathf.Cos(latitude);
            float x = multiplier * equatorX;
            float y = Mathf.Sin(latitude);
            float z = multiplier * equatorZ;

            freelookCameraTargetPosition = new Vector3(x, y, z) * distance;
        }

        private float GetAngleOnRightAngleTriangle(float Opposite, float Adjacent)
        {
            float angle = Mathf.Atan2(Opposite, Adjacent);
            angle = angle * Mathf.Rad2Deg;

            return angle;
        }

       private void GetViewInput()
{
    // ตรวจสอบสถานะของปุ่มคลิกขวา
    isRightClick = Mouse.current.rightButton.isPressed;

    if (isRightClick)
    {
        float mouseX;
        float mouseY;

        if (customInputAction)
        {
            mouseX = moveCamera.action.ReadValue<Vector2>().x * Time.deltaTime * xSensitivity * 2;
            mouseY = moveCamera.action.ReadValue<Vector2>().y * Time.deltaTime * ySensitivity * 2;
        }
        else
        {
            mouseX = userInputs.inputAxis4 * Time.deltaTime * xSensitivity * 2;
            mouseY = userInputs.inputAxis5 * Time.deltaTime * ySensitivity * 2;
        }

        // ตั้งค่าการหมุนและการหมุนสูงสุด/ต่ำสุด
        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, xClampBottom, xClampTop);
    }
}

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Getters / Setters
        
        public Vector3 GetFreelookCameraPosition() { return transform.position + freelookCameraPosition; }

        public Transform GetFreelookRigFollow() { return follow; }

        public void SetFreelookRigFollow(Transform newFollow) { follow = newFollow; }

        public CameraOrbits GetCameraOrbit() { return cameraOrbit; }

        public void SetCameraOrbit(float rigOffset, float heightUp, float heightDown, float radiusTop, float radiusMiddle, float radiusBottom)
        {
            cameraOrbit.rigOffset = rigOffset;
            cameraOrbit.height.up = heightUp;
            cameraOrbit.height.down = heightDown;
            cameraOrbit.radius.top = radiusTop;
            cameraOrbit.radius.middle = radiusMiddle;
            cameraOrbit.radius.bottom = radiusBottom;
        }

        public void SetSensitivity(Vector2 newSensitivity) { xSensitivity = (int)newSensitivity.x; ySensitivity = (int)newSensitivity.y; }

        #endregion

    } //class end
}
