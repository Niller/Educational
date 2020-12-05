using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TrafficSystem_V2.Editor
{
    public abstract class ListPropertyEditor : PropertyDrawer
    {
        private float _listItemHeight;
        
        private SerializedProperty _serializedProperty;
        private ReorderableList _reorderableList;
        private ReorderableList ReorderableList
        {
            get
            {
                if (_reorderableList != null)
                {
                    return _reorderableList;
                }

                _reorderableList = new ReorderableList(_serializedProperty.serializedObject, _serializedProperty)
                {
                    drawHeaderCallback = rect => { EditorGUI.LabelField(rect, ObjectNames.NicifyVariableName(_serializedProperty.name)); }
                };
                
                _reorderableList.drawElementCallback = (rect, index, active, focused) =>
                {
                    var itemProperty = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    _listItemHeight = rect.height;
                    DrawItem(new Rect(rect.x, rect.y, rect.width - 4, rect.height - 4), itemProperty);
                };

                return _reorderableList;
            }
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _serializedProperty = property;
            ReorderableList.DoList(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var itemsCount = property.arraySize;
            return itemsCount * _listItemHeight + ReorderableList.elementHeight;
        }

        protected abstract void DrawItem(Rect rect, SerializedProperty itemProperty);
    }
}