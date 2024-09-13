using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames
{
    [System.Serializable]
    public class LayerDropdown
    {
        [SerializeField] private int m_LayerIndex = 0;
        
        public int value { get { return m_LayerIndex; } }
        public int mask { get { return 1 << m_LayerIndex; } }
        
        public void Set(int _layerIndex)
        {
            if (_layerIndex > 0 && _layerIndex < 32)
            {
                m_LayerIndex = _layerIndex;
            }
        }
        
    } // class end
}