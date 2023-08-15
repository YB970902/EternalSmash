using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits>
        {
        }

        public InspectorView()
        {

        }
    }
}