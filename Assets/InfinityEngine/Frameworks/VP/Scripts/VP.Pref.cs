/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                              *
*************************************************************************************************************************************/

using System;
using UnityEngine;
using InfinityEngine.Utils;

//// <summary>
//// This namespace provides access to serialization tools.
//// </summary>
namespace InfinityEngine.Serialization
{
    public partial class VP
    {
        /// <summary>
        ///  Represents a simple preference data.
        /// </summary>
        /// <typeparam name="T">This pref value type</typeparam>
        [Serializable]
        public class Pref<T> : IPref
        {
            #region Fields

            [SerializeField] private PrefTypes type;
            [SerializeField] private string key;
            [SerializeField] private T value;
            [SerializeField] private string error;

            #endregion Fields

            #region Properties
            /// <summary>
            /// This preference type
            /// </summary>
            public PrefTypes Type => type;
            /// <summary>
            /// This preference key
            /// </summary>
            public string Key { get { return key; } set { key = value; } }

            /// <summary>
            /// This preference value (set to <see cref="VP.ComplexeTypesValue"/> if this is a <see cref="VP.ComplexeTypes"/>) 
            /// </summary>
            public T Value { get { return value; } set { this.value = value; } }

            /// <summary>
            /// An error message if there is an error in this key name
            /// </summary>
            public string ErrorMessage { get { return error; } set { this.error = value; } }

            /// <summary>
            /// Check if this preference key contains an error
            /// </summary>
            /// <returns></returns>
            public bool IsValid
            {
                get
                {
                    if (HasKey(type, key))
                    {
                        error = "PREF_DUPLICATION_ERROR";
                        return false;
                    }
                    error = CodeGenerationUtils.CheckIdentifier(key);
                    return ErrorMessage == null;
                }
            }

            /// <summary>
            /// Return <c>true</c> if this key name starts and ends with '___' .
            /// </summary>
            public bool IsHiddenPref => Key.StartsWith("___", StringComparison.Ordinal) && Key.EndsWith("___", StringComparison.Ordinal);

            #endregion Properties

            /// <summary>
            /// Creates new preference
            /// </summary>
            /// <param name="type">Preference type</param>
            /// <param name="key">Preference key</param>
            /// <param name="value">Preference value</param>
            public Pref(PrefTypes type, string key, T value)
            {
                this.type = type;
                this.key = key;
                this.value = value;
            }

            public override string ToString()
            {
                return $"Type {type} Key : {key}  Value : {value}";
            }
        }


    }

}