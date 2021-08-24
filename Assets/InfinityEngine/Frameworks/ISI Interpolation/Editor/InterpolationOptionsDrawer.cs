/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *                                                                                                          *
*************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Interpolations;
using UnityEditor;
using System;

namespace InfinityEditor
{

    /// <summary>
    ///  Custom property drawer of <see cref="InterpolationOptions"/> class.
    /// </summary>
    [CustomPropertyDrawer(typeof(InterpolationOptions))]
    public class InterpolationOptionsDrawer : PropertyDrawer
    {
        private Rect rect;
        private bool enableAnimationCurve;

        /// <summary>
        /// Called when unity draws the property.
        /// </summary>
        /// <param name="position">The position where to draw the property</param>
        /// <param name="property">The SerializedProperty of the property</param>
        /// <param name="label">The label of the attribute</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
           
            EditorGUI.BeginProperty(position, label, property);

            rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var enumerator = property.GetEnumerator();
            var name = String.Empty;
            SerializedProperty serializedProperty;
            while (enumerator.MoveNext())
            {
                serializedProperty = (enumerator.Current as SerializedProperty);
                name = serializedProperty.displayName;
                if (name == "Curve" && !enableAnimationCurve)
                {
                    continue;
                }
                EditorGUI.PropertyField(rect, serializedProperty, new GUIContent(name));
                rect.y += EditorGUIUtility.singleLineHeight;

                if (serializedProperty.name == "duration" || serializedProperty.name == "delay" || serializedProperty.name == "repeatInterval")
                {
                    serializedProperty.floatValue = Mathf.Clamp(serializedProperty.floatValue, 0.0f, serializedProperty.floatValue);
                }

                else if (serializedProperty.name == "repeat")
                {
                    serializedProperty.intValue = Mathf.Clamp(serializedProperty.intValue, -1, serializedProperty.intValue);
                }

            }
        }

      
        /// <summary>
        /// Called when unity calcuate the height necessaty to display the property.
        /// </summary>
        /// <param name="property">The SerializedProperty of the property</param>
        /// <param name="label">The label of the property</param>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            enableAnimationCurve = ((EaseTypes)property.FindPropertyRelative("ease").enumValueIndex) == EaseTypes.Custom;

            return base.GetPropertyHeight(property, label) * (enableAnimationCurve ? 8 : 7);
        }

    }
}