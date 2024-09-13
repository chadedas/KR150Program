using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    public class CameraTrack : GGMonoBehaviour
    {
        #region Variables

        private enum TrackTypes
        {
            Linear,
            BasisSpline,
            CatmullRomSpline
        }
        
        [SerializeField, Range(0.1f, 2)]
        private float nodeScale = 1f;
        
        [SerializeField, Range(0.1f, 2)]
        private float waypointScale = 1f;
        
        [SerializeField]
        private Color32 nodeColor = new Color32(000, 079, 223, 255);
        
        [SerializeField]
        private Color32 trackColor = new Color32(000, 179, 223, 255);
        
        [SerializeField]
        private bool showAllHandles = false;
        
        [SerializeField]
        private bool autoUpdateTrack = true;
        
        [SerializeField]
        private bool showNodes = true;
        
        [SerializeField]
        private bool element0IsOrigin = true;
        
        [SerializeField]
        private bool showTrack = true;
        
        [SerializeField]
        private bool showWaypoints = false;
        
        [SerializeField]
        private bool closedLoop = false;
        
        [LineSeparator, ReadOnly, SerializeField]
        private float trackLength;
        
        [LineSeparator, SerializeField]
        private TrackTypes trackType = TrackTypes.CatmullRomSpline;
        
        [SerializeField, Range(0, 20)]
        private int subdivisions = 10;
        
        [SerializeField, Range(0, 1)]
        private float tension = 0.85f;
        
        [SerializeField]
        [Tooltip("If element0IsOrigin is true, Element 0 is set by the object transform")]
        private List<Vector3> nodes;
        
        [SerializeField]
        private bool updateTrack;
        
        [ReadOnly, SerializeField]
        private TrackData waypoints = new TrackData();
        
        private TrackTypes trackTypeCheck = TrackTypes.Linear;
        private Vector3 positionCheck;
        private float tensionCheck;
        private bool closedLoopCheck;
        private bool initialised;
        private int nodesCountCheck;
        private int subdivisionsCheck;
        private int selectedIndex;
        
        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Game Loop

        private void Reset()
        {
            nodes = new List<Vector3>(new Vector3[]{Vector3.zero, new Vector3(-2, 0, -2), new Vector3(0, 0, -4), new Vector3(-2, 0, -6)});
            selectedIndex = 3;
            waypoints = new TrackData();
            waypoints.position = new List<Vector3>();
            waypoints.tangent = new List<Vector3>();
            waypoints.normal = new List<Vector3>();
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

    #if UNITY_EDITOR
        
        #region Gizmos [Editor]

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            if (initialised == false)
            {
                positionCheck = transform.position;
                initialised = true;
            }
        }

        protected override void OnDrawGizmosConditional(bool selected)
        {
            if (autoUpdateTrack) { updateTrack = true; }
            UpdateNodes();
            CalculateWaypoints();
            DrawNodes();
            if (waypoints.position != null && 0 < waypoints.position.Count) { DrawWaypoints(); }
        }

        private void OnValidate()
        {
            selectedIndex = Mathf.Min(selectedIndex, Nodes.Count - 1);
            selectedIndex = Mathf.Max(selectedIndex, 0);
        }

        private void DrawNodes()
        {
            if (showNodes)
            {
                int waypointsCount = nodes.Count;
            
                // draw waypoints
                if (2 <= waypointsCount)
                {
                    Gizmos.color = nodeColor;
                    for (int i = 0; i < waypointsCount; i++)
                    {
                        Gizmos.DrawSphere(nodes[i], nodeScale * 0.25f);
                    }
                }
            }
        }

        private void DrawWaypoints()
        {
            // draw subdivision waypoints and curve
            if (2 <= waypoints.position.Count)
            {
                Gizmos.color = trackColor;
                for (int i = 0; i < waypoints.position.Count - 1; i++)
                {
                    if (showWaypoints)
                    {
                        Gizmos.DrawSphere(waypoints.position[i], waypointScale * 0.075f);
                    }
                    if (showTrack)
                    {
                        Gizmos.DrawLine(waypoints.position[i], waypoints.position[i + 1]);
                    }
                }
                if (closedLoop)
                {
                    if (showWaypoints)
                    {
                        Gizmos.DrawSphere(waypoints.position[waypoints.position.Count - 1], waypointScale * 0.075f);
                    }
                    if (showTrack)
                    {
                        Gizmos.DrawLine(waypoints.position[waypoints.position.Count - 1], waypoints.position[0]);
                    }
                }
            }
        }

        #endregion
        
    #endif
        
        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private void UpdateNodes()
        {
            Vector3 position = transform.position;
            
            if (element0IsOrigin && 0 < nodes.Count)
            {
                nodes[0] = position;
            }
            
            float changeInPositionX = position.x - positionCheck.x;
            float changeInPositionY = position.y - positionCheck.y;
            float changeInPositionZ = position.z - positionCheck.z;
            
            if (changeInPositionX != 0 || changeInPositionY != 0 || changeInPositionZ != 0)
            {
                int iStart = 0;
                if (element0IsOrigin) { iStart = 1; }
                
                for (int i = iStart; i < nodes.Count; i++)
                {
                    nodes[i] += new Vector3(changeInPositionX, changeInPositionY, changeInPositionZ);
                }

                positionCheck = position;
                updateTrack = true;
            }
        }

        private void CalculateWaypoints()
        {
            int nodesCount = nodes.Count;
            
            if (updateTrack || subdivisionsCheck != subdivisions || nodesCountCheck != nodesCount || trackTypeCheck != trackType || tensionCheck != tension || closedLoopCheck != closedLoop)
            {
                if (trackType == TrackTypes.Linear)
                {
                    waypoints = new TrackData();
                    CalculateLinearPath();
                }
                else // (pathType == PathTypes.BasisSpline || pathType == PathTypes.CatmullRomSpline)
                {
                    waypoints = new TrackData();
                    CalculateSplinePath();
                }

                closedLoopCheck = closedLoop;
                tensionCheck = tension;
                trackTypeCheck = trackType;
                nodesCountCheck = nodesCount;
                subdivisionsCheck = subdivisions;
                updateTrack = false;
            }
        }
        
        private void CalculateLinearPath()
        {
            if (2 <= nodes.Count)
            {
                waypoints.position = new List<Vector3>();

                int maxIndex = nodes.Count - 1;
                int pointsPerNode = subdivisions + 1;
                
                // calculate waypoints
                if (1 <= subdivisions)
                {
                    for (int i = 0; i < maxIndex; i++)
                    {
                        for (int j = 0; j < pointsPerNode; j++)
                        {
                            float time = (float)j / pointsPerNode;
                            waypoints.position.Add(LinearInterpolateBetweenPoints(time, nodes[i], nodes[i+1]));
                        }
                    }
                    if (closedLoop)
                    {
                        for (int j = 0; j < pointsPerNode; j++)
                        {
                            float time = (float)j / pointsPerNode;
                            waypoints.position.Add(LinearInterpolateBetweenPoints(time, nodes[maxIndex], nodes[0]));
                        }
                    }
                    else
                    {
                        waypoints.position.Add(nodes[maxIndex]);
                    }
                }
                else
                {
                    waypoints.position = nodes;
                }
                
                CalculateTangents();
                CalculateNormals();
                CalculateTrackLength();
            }
        }
        
        private void CalculateSplinePath()
        {
            int maxIndex = nodes.Count - 1;
            if (closedLoop)
            {
                maxIndex += 1;
            }
            int pointsPerNode = subdivisions + 1;
            
            if (4 <= nodes.Count)
            {
                waypoints.position = new List<Vector3>();
                waypoints.tangent = new List<Vector3>();
            
                for (int i = 0; i < maxIndex; i++)
                {
                    for (int j = 0; j < pointsPerNode; j++)
                    {
                        float time = (float)j / pointsPerNode;
                        waypoints.position.Add(GetSplinePoint(time, i));
                        waypoints.tangent.Add(GetSplineTangent(time, i).normalized);
                    }
                }
                if (!closedLoop)
                {
                    waypoints.position.Add(GetSplinePoint(1, maxIndex-1));
                    waypoints.tangent.Add(GetSplineTangent(1, maxIndex-1).normalized);
                }
                
                CalculateNormals();
                CalculateTrackLength();
            }
        }
        
        private void CalculateTangents()
        {
            waypoints.tangent = new List<Vector3>();
            int maxIndex = waypoints.position.Count - 1;
            for (int i = 0; i < maxIndex; i++)
            {
                waypoints.tangent.Add((waypoints.position[i + 1] - waypoints.position[i]).normalized);
            }
            if (closedLoop)
            {
                waypoints.tangent.Add((waypoints.position[0] - waypoints.position[maxIndex]).normalized);
            }
        }

        private void CalculateNormals()
        {
            waypoints.normal = new List<Vector3>();
            int maxIndex = Mathf.Min(waypoints.position.Count, waypoints.tangent.Count);
            for (int i = 0; i < maxIndex; i++)
            {
                waypoints.normal.Add(Vector3.Cross(waypoints.position[i] + waypoints.tangent[i], waypoints.position[i]).normalized);
            }
        }

        private void CalculateTrackLength()
        {
            float distance = 0;
            
            int maxCount = waypoints.position.Count - 1;
            for (int i = 0; i < maxCount; i++)
            {
                distance += Vector3.Distance(waypoints.position[i], waypoints.position[i + 1]);
            }
            if (closedLoop)
            {
                distance += Vector3.Distance(waypoints.position[maxCount], waypoints.position[0]);
            }

            trackLength = MathUtility.RoundFloat(distance, 3);
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Linear

        private Vector3 LinearInterpolateBetweenPoints(float t, Vector3 a, Vector3 b)
        {
            return ((1-t)*a) + (t*b);
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Spline Functions

        public Vector3 GetSplinePoint(float value, int index)
        {
            Vector3[] controlPoints = CalculateControlPoints(index);
            
            // work out point from basis functions
            Vector3 r = controlPoints[0]*B0(value) + controlPoints[1]*B1(value) + controlPoints[2]*B2(value) + controlPoints[3]*B3(value);
            return r;
        }
        
        public Vector3 GetSplineTangent(float value, int index)
        {
            Vector3[] controlPoints = CalculateControlPoints(index);
            
            // work out tangent from differential basis functions
            Vector3 t = controlPoints[0]*B0Diff(value) + controlPoints[1]*B1Diff(value) + controlPoints[2]*B2Diff(value) + controlPoints[3]*B3Diff(value);
            return t;
        }

        private Vector3[] CalculateControlPoints(int index)
        {
            int waypointsCount = nodes.Count;
            Vector3[] controlPoints = new Vector3[4];

            if (closedLoop)
            {
                controlPoints[0] = nodes[(index - 1 + waypointsCount) % waypointsCount];
                controlPoints[1] = nodes[index % waypointsCount];
                controlPoints[2] = nodes[(index + 1) % waypointsCount];
                controlPoints[3] = nodes[(index + 2) % waypointsCount];
            }
            else
            {
                // controlPoints 1 and 2 should be fine
                controlPoints[1] = nodes[index];
                controlPoints[2] = nodes[index + 1];
                // controlPoints 0 and 3 need special treatment at the ends.
                if (index == 0)
                {
                    controlPoints[0] = nodes[0] + (nodes[0] - nodes[1]);
                }
                else
                {
                    controlPoints[0] = nodes[index - 1];
                }
                if (index == waypointsCount - 2)
                {
                    controlPoints[3] = nodes[waypointsCount - 1] + (nodes[waypointsCount - 1] - nodes[waypointsCount - 2]);
                }
                else
                {
                    controlPoints[3] = nodes[index + 2];
                }
            }
            
            return controlPoints;
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Spline Basis Functions

        private float B0(float value)
        {
            if (trackType == TrackTypes.BasisSpline)
            {
                return (1-value)*(1-value)*(1-value)*(1.0f/6.0f);
            }
            else // CatmullRomSpline
            {
                return -tension*value + 2*tension*value*value - tension*value*value*value;
            }
        }
        
        private float B1(float value)
        {
            if (trackType == TrackTypes.BasisSpline)
            {
                return (3*value*value*value - 6*value*value + 4) * (1.0f / 6.0f);
            }
            else // CatmullRomSpline
            {
                return 1 + (tension - 3)*value*value + (2 - tension)*value*value*value;
            }
        }
        
        private float B2(float value)
        {
            if (trackType == TrackTypes.BasisSpline)
            {
                return (-3*value*value*value + 3*value*value + 3*value + 1) * (1.0f / 6.0f);
            }
            else // CatmullRomSpline
            {
                return tension*value + (3-2*tension)*value*value + (tension - 2)*value*value*value;
            }
        }
        
        private float B3(float value)
        {
            if (trackType == TrackTypes.BasisSpline)
            {
                return value*value*value * (1.0f/6.0f);
            }
            else // CatmullRomSpline
            {
                return -tension*value*value + tension*value*value*value;
            }
        }
        
        private float B0Diff(float value)
        {
            if (trackType == TrackTypes.BasisSpline)
            {
                return -0.5f * (1-value) * (1-value);
            }
            else // CatmullRomSpline
            {
                return -tension + 4*tension*value - 3*tension*value*value;
            }
        }
        
        private float B1Diff(float value)
        {
            if (trackType == TrackTypes.BasisSpline)
            {
                return 1.5f*value*value - 2*value;
            }
            else // CatmullRomSpline
            {
                return 2*(tension - 3)*value + 3*(2 - tension)*value*value;
            }
        }
        
        private float B2Diff(float value)
        {
            if (trackType == TrackTypes.BasisSpline)
            {
                return -1.5f*value*value + value + 0.5f;
            }
            else // CatmullRomSpline
            {
                return tension + 2*(3 - 2*tension)*value + 3*(tension - 2)*value*value;
            }
        }
        
        private float B3Diff(float value)
        {
            if (trackType == TrackTypes.BasisSpline)
            {
                return 0.5f*value*value;
            }
            else // CatmullRomSpline
            {
                return -2*tension*value + 3*tension*value*value;
            }
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Getter / Setter

        // track info
        public string TrackType
        {
            get { return trackType.ToString(); }
        }
        
        public bool ShowAllHandles
        {
            get { return showAllHandles; }
            set { showAllHandles = value; }
        }
        
        public bool ClosedLoop
        {
            get { return closedLoop; }
            set { closedLoop = value; }
        }

        public List<Vector3> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        public Vector3 Node(int index)
        {
            return nodes[index];
        }

        public void Node(int index, Vector3 value)
        {
            nodes[index] = value;
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }

        public int Subdivisions
        {
            get { return subdivisions; }
            set { subdivisions = value; }
        }

        public TrackData Waypoints
        {
            get { return waypoints; }
        }
        
        // waypoint position
        public List<Vector3> WaypointPositions
        {
            get { return waypoints.position; }
        }

        public Vector3 WaypointPosition(int index)
        {
            return waypoints.position[index];
        }

        public int WaypointPositionCount()
        {
            return waypoints.position.Count;
        }

        // waypoint tangent
        public List<Vector3> WaypointTangents()
        {
            return waypoints.tangent;
        }

        public Vector3 WaypointTangent(int index)
        {
            return waypoints.tangent[index];
        }

        public int WaypointTangentCount()
        {
            return waypoints.tangent.Count;
        }
        
        // waypoint normal
        public List<Vector3> WaypointNormals()
        {
            return waypoints.normal;
        }

        public Vector3 WaypointNormal(int index)
        {
            return waypoints.normal[index];
        }

        public int WaypointNormalCount()
        {
            return waypoints.normal.Count;
        }

        #endregion
        
    } // class end
}