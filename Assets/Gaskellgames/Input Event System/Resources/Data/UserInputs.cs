using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    [System.Serializable]
    public class UserInputs
    {
        [Tooltip("Default: Space / Button South")]
        public ButtonInputs inputButton0;
        [Tooltip("Default: L Ctrl / Button East")]
        public ButtonInputs inputButton1;
        [Tooltip("Default: L Alt / Button West")]
        public ButtonInputs inputButton2;
        [Tooltip("Default: C / Button North")]
        public ButtonInputs inputButton3;
        [Tooltip("Default: L Shift / Left Shoulder")]
        public ButtonInputs inputButton4;
        [Tooltip("Default: F / Right Shoulder")]
        public ButtonInputs inputButton5;
        [Tooltip("Default: P / Select")]
        public ButtonInputs inputButton6;
        [Tooltip("Default: Esc / Start")]
        public ButtonInputs inputButton7;
        [Tooltip("Default: Q / Left Stick Press")]
        public ButtonInputs inputButton8;
        [Tooltip("Default: E / Right Stick Press")]
        public ButtonInputs inputButton9;

        [Tooltip("Default: A, D / Left Stick Horizontal")]
        [Range(-1, 1)] public float inputAxisX;
        [Tooltip("Default: W, S / Left Stick Vertical")]
        [Range(-1, 1)] public float inputAxisY;
        [Tooltip("Default: Arrows Horizontal / Right Stick Horizontal")]
        [Range(-1, 1)] public float inputAxis4;
        [Tooltip("Default: Arrows Vertical / Right Stick Vertical")]
        [Range(-1, 1)] public float inputAxis5;
        [Tooltip("Default: 1, 4 / D-Pad Vertical")]
        [Range(-1, 1)] public float inputAxis6;
        [Tooltip("Default: 2, 3 / D-Pad Horizontal")]
        [Range(-1, 1)] public float inputAxis7;
        [Tooltip("Default: L Mouse / L Trigger")]
        [Range(0, 1)] public float inputAxis9;
        [Tooltip("Default: R Mouse / R Trigger")]
        [Range(0, 1)] public float inputAxis10;

    } // class end
}

    
