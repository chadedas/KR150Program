using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    [System.Serializable]
    public class CameraLens
    {
        #region Variables

        public float verticalFOV = 50;
        public float nearClipPlane = 0.1f;
        public float farClipPlane = 1000f;
        public bool showFrustrum = true;
        [Range(-180, 180)] public float tilt = 0;
        public LayerMask cullingMask = ~0;

        #endregion
        
    } // class end
}