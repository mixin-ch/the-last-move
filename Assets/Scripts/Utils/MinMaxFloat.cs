using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mixin.Utils
{
    [System.Serializable]
    public struct MinMaxFloat
    {
        public float Min;
        public float Max;

        public MinMaxFloat(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public int GetRandomIntBetween()
        {
            return Random.Range((int)Min, (int)Max);
        }

        public float GetRandomFloatBetween()
        {
            return Random.Range((float)Min, (float)Max);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MinMaxFloat))]
    public class MinMaxFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Define label widths
            float labelWidth = EditorGUIUtility.labelWidth;
            float minMaxLabelWidth = 30;
            float fieldWidth = 70;

            // Draw label
            position = EditorGUI.PrefixLabel(position, label);

            // Draw Min field
            Rect minRect = new Rect(position.x, position.y, fieldWidth, position.height);
            EditorGUI.LabelField(minRect, new GUIContent("Min"), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft });

            minRect.x += minMaxLabelWidth;
            minRect.width = fieldWidth;

            EditorGUI.PropertyField(minRect, property.FindPropertyRelative("Min"), GUIContent.none);

            // Draw Max field
            Rect maxRect = new Rect(position.x + minMaxLabelWidth + fieldWidth, position.y, fieldWidth, position.height);
            EditorGUI.LabelField(maxRect, new GUIContent("Max"), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft });

            maxRect.x += minMaxLabelWidth;
            maxRect.width = fieldWidth;

            EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("Max"), GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}