/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *                                                                                                          *
*************************************************************************************************************************************/

using InfinityEngine.Extensions;
using InfinityEngine.Interpolations;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InfinityEditor
{
    /// <summary>
    ///  Custom property drawer of <see cref="EaseTypes"/> enum.
    /// </summary>
    [CustomPropertyDrawer(typeof(EaseTypes))]
    public class EaseTypeDrawer : PropertyDrawer
    {
        private string[] options = Enum.GetNames(typeof(EaseTypes))
                                       .Select(elem => Format(elem))
                                       .ToArray();

        /// <summary>
        /// Called when unity draws the attribute.
        /// </summary>
        /// <param name="position">The position where to draw the attribute</param>
        /// <param name="property">The SerializedProperty of the attribute</param>
        /// <param name="label">The label of the attribute</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            property.enumValueIndex = EditorGUI.Popup(position, options.IndexOf(options[property.enumValueIndex]), options);
            EditorGUI.EndProperty();
        }

        private static string Format(string str)
        {
            var len = str.Length;
            for (var i = 1; i < len; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    return str.Substring(0, i) + "/" + str.Substring(i, str.Length - i);
                }
            }
            return str;
        }
    }

}