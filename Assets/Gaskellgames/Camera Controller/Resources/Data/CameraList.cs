using System.Collections.Generic;
using UnityEngine;

using Gaskellgames;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public static class CameraList
    {
        #region Variables

        static List<CameraRig> cameraRigs = new List<CameraRig>();
        static List<CameraRig> shakableRigs = new List<CameraRig>();

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Public Functions

        public static List<CameraRig> GetCameraRigList()
        {
            return cameraRigs;
        }

        public static void Register(CameraRig cameraRig)
        {
            if (!cameraRigs.Contains(cameraRig))
            {
                cameraRigs.Add(cameraRig);
                Debug.Log(cameraRig.name + " registered. " + cameraRigs.Count + " total registered camera rigs");
            }
        }

        public static void Unregister(CameraRig cameraRig)
        {
            if(cameraRigs.Contains(cameraRig))
            {
                cameraRigs.Remove(cameraRig);
            }
        }
        
        public static List<CameraRig> GetShakableRigList()
        {
            return shakableRigs;
        }
        
        public static void SetShakable(CameraRig cameraRig)
        {
            if (!shakableRigs.Contains(cameraRig))
            {
                shakableRigs.Add(cameraRig);
            }
        }

        public static void UnsetShakable(CameraRig cameraRig)
        {
            if(shakableRigs.Contains(cameraRig))
            {
                shakableRigs.Remove(cameraRig);
            }
        }

        #endregion

    } //class end
}
