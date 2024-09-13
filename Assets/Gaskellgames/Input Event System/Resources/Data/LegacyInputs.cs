using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    [System.Serializable]
    public class LegacyInputs
    {
        [Header("Keybind (Action Buttons)")] [Space]

        public KeyCode button0 = KeyCode.Space;
        public KeyCode button1 = KeyCode.LeftControl;
        public KeyCode button2 = KeyCode.LeftAlt;
        public KeyCode button3 = KeyCode.C;
        public KeyCode button4 = KeyCode.LeftShift;
        public KeyCode button5 = KeyCode.F;
        public KeyCode button6 = KeyCode.P;
        public KeyCode button7 = KeyCode.Escape;
        public KeyCode button8 = KeyCode.Q;
        public KeyCode button9 = KeyCode.E;

        [Header("Keybind (Axis X & Y)")] [Space]

        public KeyCode axisXY_up = KeyCode.W;
        public KeyCode axisXY_left = KeyCode.A;
        public KeyCode axisXY_down = KeyCode.S;
        public KeyCode axisXY_right = KeyCode.D;

        [Header("Keybind (Axis 4 & 5)")] [Space]

        public KeyCode axis45_up = KeyCode.UpArrow;
        public KeyCode axis45_left = KeyCode.LeftArrow;
        public KeyCode axis45_down = KeyCode.DownArrow;
        public KeyCode axis45_right = KeyCode.RightArrow;

        [Header("Keybind (Axis 6 & 7)")] [Space]

        public KeyCode axis67_up = KeyCode.Alpha1;
        public KeyCode axis67_left = KeyCode.Alpha2;
        public KeyCode axis67_down = KeyCode.Alpha4;
        public KeyCode axis67_right = KeyCode.Alpha3;

        [Header("Keybind (Axis 9)")] [Space]

        public KeyCode axis9 = KeyCode.Mouse0;

        [Header("Keybind (Axis 10)")] [Space]

        public KeyCode axis10 = KeyCode.Mouse1;

    } // class end
}
