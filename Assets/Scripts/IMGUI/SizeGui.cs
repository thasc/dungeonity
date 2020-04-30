using UnityEngine;

namespace IMGUI
{
    public class SizeGui : MonoBehaviour
    {
        public Vector2 TopLeft;

        private Rect m_ContainerBounds;
        private Rect m_WidthBounds;
        private Rect m_HeightBounds;
        private Rect m_ApplyBounds;

        private string m_WidthString = 10.ToString();
        private string m_HeightString = 10.ToString();

        public int RequestedWidth => int.TryParse(m_WidthString, out var result) ? result : -1;
        public int RequestedHeight => int.TryParse(m_HeightString, out var result) ? result : -1;

        private void Awake()
        {
            const float width = 100;
            const float height = 150;

            const float textFieldHeight = 30;
            const float margin = 10;
            
            m_ContainerBounds = new Rect(TopLeft.x, TopLeft.y, width, height);
            m_WidthBounds = new Rect(TopLeft.x + margin, TopLeft.y + margin*3, width - margin*2, textFieldHeight);
            m_HeightBounds = new Rect(TopLeft.x + margin, TopLeft.y + margin*4 + textFieldHeight, width - margin*2, textFieldHeight);
            m_ApplyBounds = new Rect(TopLeft.x + margin, height - textFieldHeight, width - margin*2, textFieldHeight);
        }

        private void OnGUI()
        {
            GUI.Box(m_ContainerBounds, "Map size");
            
            m_WidthString = GUI.TextField(m_WidthBounds, m_WidthString);
            m_HeightString = GUI.TextField(m_HeightBounds, m_HeightString);

            if (GUI.Button(m_ApplyBounds, "Apply"))
            {
                Debug.Log($"({RequestedWidth},{RequestedHeight})");                
            }
        }
    }
}
