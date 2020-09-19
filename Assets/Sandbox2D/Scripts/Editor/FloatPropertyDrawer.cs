using Sandbox2D.Scripts.Water;
using UnityEditor;
using UnityEngine;

namespace Sandbox2D.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(FloatProperty))]
    public class FloatPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            const int spacing = 2;
            const int typeWidth = 100;

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            var typeRect = new Rect(position.xMax - typeWidth, position.y, typeWidth, position.height);
            var typeProperty = property.FindPropertyRelative("_type");
            EditorGUI.PropertyField(typeRect, typeProperty, GUIContent.none);
            var type = (PropertyType) typeProperty.enumValueIndex;
            
            switch (type)
            {
                case PropertyType.Constant:
                    var constantRect = new Rect(position.x, position.y, position.width - typeWidth - spacing * 2, position.height);
                    EditorGUI.PropertyField(constantRect, property.FindPropertyRelative("_constant"), GUIContent.none);
                    break;
                case PropertyType.Random:
                    var propertyWidth = (position.width - typeWidth) / 2 - spacing;
                    var minRect = new Rect(position.x, position.y, propertyWidth, position.height);
                    EditorGUI.PropertyField(minRect, property.FindPropertyRelative("_min"), GUIContent.none);
                    var maxRect = new Rect(minRect.xMax + spacing, position.y, propertyWidth, position.height);
                    EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("_max"), GUIContent.none);
                    break;
            }

        }
    }
}
