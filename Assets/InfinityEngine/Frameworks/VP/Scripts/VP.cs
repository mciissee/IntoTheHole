/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                              *
*************************************************************************************************************************************/

using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InfinityEngine.Extensions;

//// <summary>
//// This namespace provides access to serialization tools.
//// </summary>
using System.Linq;
namespace InfinityEngine.Serialization
{

    /// <summary>
    ///     The main class of <a href="http://u3d.as/GLW">Visual Prefs</a> plugin.<br/>
    ///     This class allow to save, load and visualize data in unity editor.<br/>
    ///     The data are placed at <a href="https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html"> Application.persistentDataPath</a> + visualPrefs.vp
    /// </summary>
    /// <remarks>
    /// You should not instantiate this class, it is managed by the class VPEditor. <br/>
    /// </remarks>
    [Serializable]
    public partial class VP
    {
        private enum ArrayType { Float, Int32, Bool, String, Vector2, Vector3, Quaternion, Color }

        #region Fields

        /// <summary>
        /// Setter functions
        /// </summary>
        private static Dictionary<Type, IMutator> Setter = new Dictionary<Type, IMutator>
         {
            { typeof(bool), new Mutator<string, bool>(SetBool)}, { typeof(int), new Mutator<string, int>(SetInt)},
            { typeof(float), new Mutator<string, float>(SetFloat)}, { typeof(long), new Mutator<string, long>(SetLong)},
            { typeof(double), new Mutator<string, double>(SetDouble)}, { typeof(string), new Mutator<string, string>(SetString)},
            { typeof(Vector2), new Mutator<string, Vector2>(SetVector2)},{ typeof(Vector3), new Mutator<string, Vector3>(SetVector3)},
            { typeof(Vector4), new Mutator<string, Vector4>(SetVector4)},{ typeof(Quaternion), new Mutator<string, Quaternion>(SetQuaternion)},
            { typeof(Color), new Mutator<string, Color>(SetColor)}
        };

        /// <summary>
        /// Getter functions
        /// </summary>
        private static Dictionary<Type, IAccessor> Getter = new Dictionary<Type, IAccessor>
        {
            { typeof(bool), new Accessor<string, bool>(GetBool)}, { typeof(int), new Accessor<string,int>(GetInt)},
            { typeof(float), new Accessor<string,float>(GetFloat)}, { typeof(long), new Accessor<string,long>(GetLong)},
            { typeof(double), new Accessor<string,double>(GetDouble)}, { typeof(string), new Accessor<string,string>(GetString)},
            { typeof(Vector2), new Accessor<string,Vector2>(GetVector2)},{ typeof(Vector3), new Accessor<string,Vector3>(GetVector3)},
            { typeof(Vector4), new Accessor<string,Vector4>(GetVector4)},{ typeof(Quaternion), new Accessor<string,Quaternion>(GetQuaternion)},
            { typeof(Color), new Accessor<string,Color>(GetColor)}
        };

        private const string ComplexeTypesValue = "NULL";
        private const string HiddenFormat = "___HIDDEN_{0}_HIDDEN___";

        /// <summary>
        /// The name of the file that contains the preferences. The file is placed at <a href="https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html"> Application.persistentDataPath</a> + visualPrefs.vp
        /// </summary>
        public const string FileName = "visualprefs.vp";

        /// <summary>
        /// Array of <see cref="PrefTypes"/> enum.
        /// </summary>
        public static PrefTypes[] AllTypes = Enum.GetNames(typeof(PrefTypes)).Select(elem => (PrefTypes)Enum.Parse(typeof(PrefTypes), elem)).ToArray();

        /// <summary>
        /// Array of all primitives types
        /// </summary>
        public static PrefTypes[] PrimitiveTypes = { PrefTypes.Integer, PrefTypes.Long, PrefTypes.Float, PrefTypes.Double, PrefTypes.String, PrefTypes.Bool };

        /// <summary>
        /// Array of all complexe types
        /// </summary>
        public static PrefTypes[] ComplexeTypes = { PrefTypes.Vector2, PrefTypes.Vector3, PrefTypes.Vector4, PrefTypes.Quaternion, PrefTypes.Color };

        private static int endianDiff1;
        private static int endianDiff2;
        private static int idx;
        private static byte[] byteBlock;
        private static VP instance;

        private static List<float> mFloatValues = new List<float>();

        [SerializeField]
        private List<List<IPref>> preferences;

        private static VP Instance
        {
            get
            {
                if (instance == null)
                {
                    if (File.Exists(FilePath))
                    {
                        try
                        {
                            instance = Serializer.Deserialize<VP>(FileName, Location.PersistentDataPath, Format.BinaryFile);
                        }
                        catch
                        {
                            DeleteAll();
                        }
                    }
                    else
                    {
                        instance = new VP();
                    }
                }
                return instance;
            }
        }

        #endregion Fields

        private VP()
        {
            preferences = new List<List<IPref>>();
            AllTypes.ForEach(type => preferences.Add(new List<IPref>()));
        }

        #region Methods

        #region Primitive Types

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetBool(string key, bool value)
        {
            Set(PrefTypes.Bool, key, value);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static bool GetBool(string key)
        {
            return Get(PrefTypes.Bool, key, false);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">default value of this pref</param>
        /// <returns>value of the pref or default value</returns>
        public static bool GetBool(string key, bool defaultValue)
        {
            return Get(PrefTypes.Bool, key, defaultValue);
        }

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetInt(string key, int value)
        {
            Set(PrefTypes.Integer, key, value);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static int GetInt(string key)
        {
            return Get(PrefTypes.Integer, key, 0);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">default value of this pref</param>
        /// <returns>value of the pref or default value</returns>
        public static int GetInt(string key, int defaultValue)
        {
            return Get(PrefTypes.Integer, key, defaultValue);
        }

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetFloat(string key, float value)
        {
            Set(PrefTypes.Float, key, value);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static float GetFloat(string key)
        {
            return Get(PrefTypes.Float, key, 0.0f);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">default value of this pref</param>
        /// <returns>value of the pref or default value</returns>
        public static float GetFloat(string key, float defaultValue)
        {
            return Get(PrefTypes.Float, key, defaultValue);
        }

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetDouble(string key, double value)
        {
            Set(PrefTypes.Double, key, value);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static double GetDouble(string key)
        {
            return Get(PrefTypes.Double, key, 0d);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">default value of this pref</param>
        /// <returns>value of the pref or default value</returns>
        public static double GetDouble(string key, double defaultValue)
        {
            return Get(PrefTypes.Double, key, defaultValue);
        }

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetLong(string key, long value)
        {
            Set(PrefTypes.Long, key, value);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static long GetLong(string key)
        {
            return Get(PrefTypes.Long, key, 0L);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">default value of this pref</param>
        /// <returns>value of the pref or default value</returns>
        public static long GetLong(string key, long defaultValue)
        {
            return Get(PrefTypes.Long, key, defaultValue);
        }

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetString(string key, string value)
        {
            Set(PrefTypes.String, key, value);
        }
        /// <summary>
        /// Returns the value corresponding to key in the preferences file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">preference key</param>
        /// <returns>value of the preference or default value</returns>
        public static string GetString(string key)
        {
            return Get(PrefTypes.String, key, string.Empty);
        }
        /// <summary>
        /// Returns the value corresponding to key in the preferences file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">preference key</param>
        /// <param name="defaultValue">default value of this preference</param>
        /// <returns>value of the preference or default value</returns>
        public static string GetString(string key, string defaultValue)
        {
            return Get(PrefTypes.String, key, defaultValue);
        }

        #endregion Primitive Types

        #region Complexe Types

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetVector2(string key, Vector2 value)
        {
            Set(PrefTypes.Vector2, key, ComplexeTypesValue);
            SetFloatArray(string.Format(HiddenFormat, key), new float[] { value.x, value.y });
        }

        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static Vector2 GetVector2(string key)
        {
            var floatArray = GetFloatArray(string.Format(HiddenFormat, key));
            if (floatArray.Length < 2)
                return Vector2.zero;

            return new Vector2(floatArray[0], floatArray[1]);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">default value of this pref</param>
        /// <returns>value of the pref or default value</returns>
        public static Vector2 GetVector2(string key, Vector2 defaultValue)
        {
            if (HasKey(PrefTypes.Vector2, key))
                return GetVector2(key);

            SetVector2(key, defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetVector3(string key, Vector3 value)
        {
            Set(PrefTypes.Vector3, key, ComplexeTypesValue);
            SetFloatArray(string.Format(HiddenFormat, key), new float[] { value.x, value.y, value.z });
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static Vector3 GetVector3(string key)
        {
            var floatArray = GetFloatArray(string.Format(HiddenFormat, key));
            if (floatArray.Length < 3)
                return Vector3.zero;

            return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">default value of this pref</param>
        /// <returns>value of the pref or default value</returns>
        public static Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            if (HasKey(PrefTypes.Vector3, key))
                return GetVector3(key);

            SetVector3(key, defaultValue);
            return defaultValue;
        }


        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetVector4(string key, Vector4 value)
        {
            Set(PrefTypes.Vector4, key, ComplexeTypesValue);
            SetFloatArray(string.Format(HiddenFormat, key), new float[] { value.x, value.y, value.z, value.w });
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static Vector4 GetVector4(string key)
        {
            var floatArray = GetFloatArray(string.Format(HiddenFormat, key));
            if (floatArray.Length < 4)
                return Vector4.zero;

            return new Vector4(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">default value of this pref</param>
        /// <returns>value of the pref or default value</returns>
        public static Vector4 GetVector4(string key, Vector4 defaultValue)
        {
            if (HasKey(PrefTypes.Vector4, key))
                return GetVector4(key);

            SetVector3(key, defaultValue);
            return defaultValue;
        }


        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetQuaternion(string key, Quaternion value)
        {
            Set(PrefTypes.Quaternion, key, ComplexeTypesValue);
            SetFloatArray(string.Format(HiddenFormat, key), new float[] { value.x, value.y, value.z, value.w });
        }
        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetQuaternion(string key, Vector3 value)
        {
            SetQuaternion(key, Quaternion.Euler(value));
        }
        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetQuaternion(string key, Vector4 value)
        {
            SetQuaternion(key, value.ToQuaternion());
        }

        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static Quaternion GetQuaternion(string key)
        {
            var floatArray = GetFloatArray(string.Format(HiddenFormat, key));
            if (floatArray.Length < 4)
                return Quaternion.identity;

            return new Quaternion(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultValue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">value of the pref</param>
        /// <returns>value of the pref or default value</returns>
        public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
        {
            if (HasKey(PrefTypes.Quaternion, key))
                return GetQuaternion(key);

            SetQuaternion(key, defaultValue);
            return defaultValue;
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultValue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">value of the pref</param>
        /// <returns>value of the pref or default value</returns>
        public static Quaternion GetQuaternion(string key, Vector3 defaultValue)
        {
            if (HasKey(PrefTypes.Quaternion, key))
                return GetQuaternion(key);

            SetQuaternion(key, defaultValue);
            return Quaternion.Euler(defaultValue);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultValue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">value of the pref</param>
        /// <returns>value of the pref or default value</returns>
        public static Quaternion GetQuaternion(string key, Vector4 defaultValue)
        {
            if (HasKey(PrefTypes.Quaternion, key))
                return GetQuaternion(key);

            SetQuaternion(key, defaultValue);
            return defaultValue.ToQuaternion();
        }

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="value">new value of the pref</param>
        public static void SetColor(string key, Color value)
        {
            Set(PrefTypes.Color, key, ComplexeTypesValue);
            SetFloatArray(string.Format(HiddenFormat, key), new float[] { value.r, value.g, value.b, value.a });
        }

        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultvalue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <returns>value of the pref or default value</returns>
        public static Color GetColor(string key)
        {
            var floatArray = GetFloatArray(string.Format(HiddenFormat, key));
            if (floatArray.Length < 4)
                return Color.black;

            return new Color(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
        }
        /// <summary>
        /// Returns the value corresponding to key in the prefs file if it exists.
        /// If it doesn't exist, it will return defaultValue.
        /// </summary>
        /// <param name="key">pref key</param>
        /// <param name="defaultValue">value of the pref</param>
        /// <returns>value of the pref or default value</returns>
        public static Color GetColor(string key, Color defaultValue)
        {
            if (HasKey(PrefTypes.Color, key))
                return GetColor(key);

            SetColor(key, defaultValue);
            return defaultValue;
        }

        #endregion Complexe Types


        /// <summary>
        /// Returns the value corresponding to <paramref name="key"/> in the preferences file of 
        /// the given type if it exists.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Throwed when the given generic parameter is not valid.
        /// </exception>
        /// <param name="key">preference key</param>
        /// <returns>value of the preference or default value</returns>
        public static T Get<T>(string key)
        {
            var type = typeof(T);
            if (Getter.ContainsKey(type))
            {
                return (Getter[type] as Accessor<string, T>).Invoke(key);
            }

            throw new ArgumentException($"There is no preference of the type {type}");
        }

        public static T Get<T>(PrefTypes type, string key, T defaultValue)
        {
            var find = Instance.preferences[(int)type].Find(pref => pref.Key == key);
            if (find == null)
            {
                Instance.preferences[(int)type].Add(new Pref<T>(type, key, defaultValue));
                return defaultValue;
            }
            return (find as Pref<T>).Value;
        }

        /// <summary>
        /// Sets the value of the pref identified by key.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Throwed when the given generic parameter is not valid.
        /// </exception>
        /// <param name="key">preference key</param>
        /// <param name="value">new value of the preference</param>
        public static void Set<T>(string key, T value)
        {
            var type = typeof(T);
            if (Setter.ContainsKey(type))
            {
                (Setter[type] as Mutator<string, T>).Invoke(key, value);
                return;
            }
            throw new ArgumentException($"There is no preference of the type {type}");
        }

        public static void Set<T>(PrefTypes type, string key, T value)
        {
            var find = Instance.preferences[(int)type].Find(pref => pref.Key == key);
            if (find != null)
            {
                (find as Pref<T>).Value = value;
                return;
            }
            Instance.preferences[(int)type].Add(new Pref<T>(type, key, value));
        }

        /// <summary>
        /// Delete all prefs
        /// </summary>
        public static void DeleteAll()
        {
            string path = FilePath;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            instance = new VP();
            Save();
        }

        /// <summary>
        /// Returns a dictionary of all registered preferences.
        /// </summary>
        /// <returns>string list dictionary of all keys</returns>
        public static Dictionary<PrefTypes, string[]> Keys()
        {
            return AllTypes.ToDictionary(it => it, it => Instance.preferences[(int)it]
                                         .Where(elem => !elem.IsHiddenPref)
                                         .Select(elem => elem.Key).ToArray());
        }

        /// <summary>
        /// Deletes the preference of the given type with the given key if it exists
        /// </summary>
        /// <param name="type">the type of the preference</param>
        /// <param name="key">the key of the preference</param>
        /// <returns><c>true</c> if the name if deleted <c>false</c> otherwise</returns>
        public static bool DeleteKey(PrefTypes type, string key)
        {
            try
            {
                var tmp = Instance.preferences[(int)type];
                var find = tmp.Find(elem => elem.Key == key);
                tmp.Remove(find);

                switch (type)
                {
                    case PrefTypes.Vector2:
                    case PrefTypes.Vector3:
                    case PrefTypes.Vector4:
                    case PrefTypes.Color:
                    case PrefTypes.Quaternion:
                        DeleteKey(PrefTypes.String, string.Format(HiddenFormat, key));
                        break;
                }
                Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if there is a Pref of the given type with the given key
        /// </summary>
        /// <param name="type">the type of the pref</param>
        /// <param name="key">the key of the pref</param>
        /// <returns><c>true</c> if the given name exist <c>false</c> otherwise</returns>
        public static bool HasKey(PrefTypes type, string key)
        {
            return Instance.preferences[(int)type].Any(pref => pref.Key == key);
        }

        /// <summary>
        /// Save the preferences to the disk.
        /// </summary>
        public static void Save()
        {
            Serializer.Serialize(Instance, FileName, Location.PersistentDataPath, Format.BinaryFile);
        }

        /// <summary>
        /// The path where the preference file is stored  "<a href="https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html"> Application.persistentDataPath</a> + visualPrefs.vp"
        /// </summary>
        public static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        #endregion Methods

        #region Array

        private static void SetFloatArray(string key, float[] floatArray)
        {
            SetValue(key, floatArray, ArrayType.Float, 1, ConvertFromFloat);
        }

        private static float[] GetFloatArray(string key)
        {
            mFloatValues.Clear();
            GetValue(key, mFloatValues, ArrayType.Float, 1, ConvertToFloat);
            return mFloatValues.ToArray();
        }

        private static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
        {
            if (HasKey(PrefTypes.String, key))
            {
                return GetFloatArray(key);
            }
            var floatArray = new float[defaultSize];
            for (int i = 0; i < defaultSize; i++)
            {
                floatArray[i] = defaultValue;
            }
            return floatArray;
        }

        private static bool SetValue<T>(string key, T list, ArrayType arrayType, int vectorNumber, Action<T, byte[], int> convert) where T : IList
        {
            var bytes = new byte[(4 * list.Count) * vectorNumber + 1];
            bytes[0] = Convert.ToByte(arrayType);    // Identifier
            Initialize();

            for (var i = 0; i < list.Count; i++)
            {
                convert(list, bytes, i);
            }
            return SaveBytes(key, bytes);
        }

        private static void GetValue<T>(string key, T list, ArrayType arrayType, int vectorNumber, Action<T, byte[]> convert) where T : IList
        {
            if (HasKey(PrefTypes.String, key))
            {
                var bytes = Convert.FromBase64String(GetString(key));
                if ((bytes.Length - 1) % (vectorNumber * 4) != 0)
                {
                    Debug.LogError("Corrupt preference file for " + key);
                    return;
                }
                if ((ArrayType)bytes[0] != arrayType)
                {
                    Debug.LogError(key + " is not a " + arrayType.ToString() + " list");
                    return;
                }
                Initialize();

                var end = (bytes.Length - 1) / (vectorNumber * 4);
                for (var i = 0; i < end; i++)
                {
                    convert(list, bytes);
                }
            }
        }

        private static void ConvertToInt(List<int> list, byte[] bytes)
        {
            list.Add(ConvertBytesToInt32(bytes));
        }

        private static void ConvertToFloat(List<float> list, byte[] bytes)
        {
            list.Add(ConvertBytesToFloat(bytes));
        }

        private static void ConvertToVector2(List<Vector2> list, byte[] bytes)
        {
            list.Add(new Vector2(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
        }

        private static void ConvertToVector3(List<Vector3> list, byte[] bytes)
        {
            list.Add(new Vector3(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
        }

        private static void ConvertToQuaternion(List<Quaternion> list, byte[] bytes)
        {
            list.Add(new Quaternion(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
        }

        private static void ConvertToColor(List<Color> list, byte[] bytes)
        {
            list.Add(new Color(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
        }

        private static void ShowArrayType(string key)
        {
            var bytes = System.Convert.FromBase64String(PlayerPrefs.GetString(key));
            if (bytes.Length > 0)
            {
                ArrayType arrayType = (ArrayType)bytes[0];
                Debug.Log(key + " is a " + arrayType.ToString() + " list");
            }
        }

        private static void Initialize()
        {
            if (System.BitConverter.IsLittleEndian)
            {
                endianDiff1 = 0;
                endianDiff2 = 0;
            }
            else
            {
                endianDiff1 = 3;
                endianDiff2 = 1;
            }
            if (byteBlock == null)
            {
                byteBlock = new byte[4];
            }
            idx = 1;
        }

        private static bool SaveBytes(string key, byte[] bytes)
        {
            try
            {
                SetString(key, System.Convert.ToBase64String(bytes));
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static void ConvertFloatToBytes(float f, byte[] bytes)
        {
            byteBlock = System.BitConverter.GetBytes(f);
            ConvertTo4Bytes(bytes);
        }

        private static float ConvertBytesToFloat(byte[] bytes)
        {
            ConvertFrom4Bytes(bytes);
            return System.BitConverter.ToSingle(byteBlock, 0);
        }

        private static void ConvertInt32ToBytes(int i, byte[] bytes)
        {
            byteBlock = System.BitConverter.GetBytes(i);
            ConvertTo4Bytes(bytes);
        }

        private static int ConvertBytesToInt32(byte[] bytes)
        {
            ConvertFrom4Bytes(bytes);
            return System.BitConverter.ToInt32(byteBlock, 0);
        }

        private static void ConvertTo4Bytes(byte[] bytes)
        {
            bytes[idx] = byteBlock[endianDiff1];
            bytes[idx + 1] = byteBlock[1 + endianDiff2];
            bytes[idx + 2] = byteBlock[2 - endianDiff2];
            bytes[idx + 3] = byteBlock[3 - endianDiff1];
            idx += 4;
        }

        private static void ConvertFrom4Bytes(byte[] bytes)
        {
            byteBlock[endianDiff1] = bytes[idx];
            byteBlock[1 + endianDiff2] = bytes[idx + 1];
            byteBlock[2 - endianDiff2] = bytes[idx + 2];
            byteBlock[3 - endianDiff1] = bytes[idx + 3];
            idx += 4;
        }

        private static void ConvertFromInt(int[] list, byte[] bytes, int i)
        {
            ConvertInt32ToBytes(list[i], bytes);
        }

        private static void ConvertFromFloat(float[] list, byte[] bytes, int i)
        {
            ConvertFloatToBytes(list[i], bytes);
        }

        private static void ConvertFromVector2(Vector2[] list, byte[] bytes, int i)
        {
            ConvertFloatToBytes(list[i].x, bytes);
            ConvertFloatToBytes(list[i].y, bytes);
        }

        private static void ConvertFromVector3(Vector3[] list, byte[] bytes, int i)
        {
            ConvertFloatToBytes(list[i].x, bytes);
            ConvertFloatToBytes(list[i].y, bytes);
            ConvertFloatToBytes(list[i].z, bytes);
        }

        private static void ConvertFromQuaternion(Quaternion[] list, byte[] bytes, int i)
        {
            ConvertFloatToBytes(list[i].x, bytes);
            ConvertFloatToBytes(list[i].y, bytes);
            ConvertFloatToBytes(list[i].z, bytes);
            ConvertFloatToBytes(list[i].w, bytes);
        }

        private static void ConvertFromColor(Color[] list, byte[] bytes, int i)
        {
            ConvertFloatToBytes(list[i].r, bytes);
            ConvertFloatToBytes(list[i].g, bytes);
            ConvertFloatToBytes(list[i].b, bytes);
            ConvertFloatToBytes(list[i].a, bytes);
        }

        #endregion Array
    }

}