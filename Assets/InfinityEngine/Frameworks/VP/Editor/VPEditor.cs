/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    * 
*************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using InfinityEngine.Extensions;
using InfinityEngine.Serialization;
using InfinityEngine.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace InfinityEditor
{
    /// <summary>
    /// Editor class for <see cref="VP"/> class
    /// </summary>
    public partial class VPEditor : EditorWindow
    {
        #region Fields

        private static VPEditor Instance;

        private const int Width = 920;
        private const int Height = 500;
        private static string[] Toolbar = Enum.GetNames(typeof(PrefTypes));

        /// <summary>
        /// Temporary created prefs
        /// </summary>
        private static Dictionary<PrefTypes, List<string>> preferences;

        /// <summary>
        /// Dictionary of all prefs
        /// </summary>
        private static Dictionary<PrefTypes, string[]> keys;

        /// <summary>
        /// GetSet Property formats used during R2 class generation.
        /// </summary>
        private static Dictionary<PrefTypes, string> Signatures = new Dictionary<PrefTypes, string>()
        {
            {PrefTypes.Bool,     "\t\t\tpublic static PrefGetSet<bool> {0} = new PrefGetSet<bool>(\"{0}\");" },
            {PrefTypes.Integer,   "\t\t\tpublic static PrefGetSet<int> {0} = new PrefGetSet<int>(\"{0}\");" },
            {PrefTypes.Float,     "\t\t\tpublic static PrefGetSet<float> {0} = new PrefGetSet<float>(\"{0}\");" },
            {PrefTypes.Double,    "\t\t\tpublic static PrefGetSet<double> {0} = new PrefGetSet<double>(\"{0}\");" },
            {PrefTypes.Long,      "\t\t\tpublic static PrefGetSet<long> {0} = new PrefGetSet<long>(\"{0}\");"},
            {PrefTypes.String,    "\t\t\tpublic static PrefGetSet<string> {0} = new PrefGetSet<string>(\"{0}\");" },
            {PrefTypes.Vector2,   "\t\t\tpublic static PrefGetSet<Vector2> {0} = new PrefGetSet<Vector2>(\"{0}\");" },
            {PrefTypes.Vector3,   "\t\t\tpublic static PrefGetSet<Vector3> {0} = new PrefGetSet<Vector3>(\"{0}\");" },
            {PrefTypes.Vector4,   "\t\t\tpublic static PrefGetSet<Vector4> {0} = new PrefGetSet<Vector4>(\"{0}\");" },
            {PrefTypes.Color,     "\t\t\tpublic static PrefGetSet<Color> {0} = new PrefGetSet<Color>(\"{0}\");" },
            {PrefTypes.Quaternion, "\t\t\tpublic static PrefGetSet<Quaternion> {0} = new PrefGetSet<Quaternion>(\"{0}\");" },
        };

        /// <summary>
        /// Reoardable list used to display prefs
        /// </summary>
        private ReorderableList reordableList;

        /// <summary>
        /// Current pref list that is visible
        /// </summary>
        private string[] selectedPreferences;

        private PrefTypes selectedType;
        private Vector2 leftAreaScroll;
        private Vector2 rightAreaScroll;

        private GUIContent labelTitle;
        private bool changed;
        private static Color BackgroundColor = new Color(1, 1, 1, .2f);

        #endregion Fields

        #region Unity

        void OnEnable()
        {
            Instance = this;
            labelTitle = new GUIContent();

            if (preferences == null)
            {
                preferences = new Dictionary<PrefTypes, List<string>>();
                foreach (var type in VP.AllTypes)
                {
                    preferences.Add(type, new List<string>());
                }
            }

            if (reordableList == null)
            {
                reordableList = new ReorderableList(null, typeof(string))
                {
                    drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                    {
                        DrawPrefeference(rect, selectedPreferences[index]);
                    },

                    headerHeight = 2,

                    onAddCallback = list =>
                    {
                        preferences[selectedType].Add("preference name...");
                    },

                    onRemoveCallback = reorderableList =>
                    {
                        if (reorderableList.index < selectedPreferences.Length)
                        {
                            VP.DeleteKey(selectedType, selectedPreferences[reorderableList.index]);
                            selectedPreferences.RemoveAt(reorderableList.index);
                            changed = true;
                        }
                    }
                };
            }
        }

        void OnGUI()
        {
            if (changed || keys == null)
            {
                keys = VP.Keys();
                changed = false;
                Repaint();
            }

            GUILayout.BeginArea(new Rect(0, 0, 200, Height));
            DrawLeftArea();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(200, 0, Width - 200, Height));
            DrawRightArea();
            GUILayout.EndArea();

        }

        #endregion Unity

        private void DrawLeftArea()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Preferences");
            GUILayout.EndHorizontal();

            leftAreaScroll = GUILayout.BeginScrollView(leftAreaScroll);

            for (var i = 0; i < Toolbar.Length; i++)
            {
                if (GUILayout.Button(Toolbar[i], EditorStyles.toolbarButton))
                    selectedType = (PrefTypes)Enum.Parse(typeof(PrefTypes), Toolbar[i]);
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            var lastColor = GUI.color;
            GUI.color = BackgroundColor;
            GUI.Box(GUILayoutUtility.GetLastRect(), GUIContent.none);
            GUI.color = lastColor;
        }

        private void DrawRightArea()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Visualizer");
            GUILayout.Space(10);
            if (GUILayout.Button("Regenerate R2", EditorStyles.toolbarButton, GUILayout.Width(200)))
                GeneratePreferences();
            GUILayout.EndHorizontal();

            rightAreaScroll = GUILayout.BeginScrollView(rightAreaScroll);

            DrawPreferences();

            GUILayout.Space(10);

            DrawTemporaryPreferences();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void DrawPreferences()
        {
            selectedPreferences = keys[selectedType];
            reordableList.list = selectedPreferences;
            reordableList.DoLayoutList();
        }

        private void DrawTemporaryPreferences()
        {
            var prefName = string.Empty;
            for (var i = 0; i < preferences[selectedType].Count; i++)
            {
                prefName = preferences[selectedType][i];

                GUILayout.BeginHorizontal();
                preferences[selectedType][i] = EditorGUILayout.TextField("Name", prefName, GUILayout.MaxWidth(Width));
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("✓"))
                    AddPreference(selectedType, prefName);

                if (GUILayout.Button("✗"))
                    preferences[selectedType].Remove(prefName);

                GUILayout.EndHorizontal();

                GUILayout.Space(5);
            }
        }

        private void AddPreference(PrefTypes type, string key)
        {
            if (CodeGenerationUtils.IsIdentifier(key))
            {
                switch (type)
                {
                    case PrefTypes.Integer:
                        VP.SetInt(key, 0);
                        break;
                    case PrefTypes.Long:
                        VP.SetLong(key, 0L);
                        break;
                    case PrefTypes.Float:
                        VP.SetFloat(key, 0.0f);
                        break;
                    case PrefTypes.Double:
                        VP.SetDouble(key, 0D);
                        break;
                    case PrefTypes.String:
                        VP.SetString(key, string.Empty);
                        break;
                    case PrefTypes.Bool:
                        VP.SetBool(key, false);
                        break;
                    case PrefTypes.Vector2:
                        VP.SetVector2(key, Vector2.zero);
                        break;
                    case PrefTypes.Vector3:
                        VP.SetVector3(key, Vector3.zero);
                        break;
                    case PrefTypes.Vector4:
                        VP.SetVector4(key, Vector4.zero);
                        break;
                    case PrefTypes.Color:
                        VP.SetColor(key, Color.black);
                        break;
                    case PrefTypes.Quaternion:
                        VP.SetQuaternion(key, Quaternion.identity);
                        break;
                }

                VP.Save();
                preferences[type].Remove(key);
                changed = true;
            }
            else
            {
                ShowNotification(new GUIContent($"'{key}' cannot be used as a preference name."));
            }
        }

        private void DrawPrefeference(Rect rect, string key)
        {
            labelTitle.text = key;
            rect.x = 20;
            rect.width -= 300;
            EditorGUI.LabelField(rect, labelTitle);
            rect.x = 320;

            switch (selectedType)
            {
                case PrefTypes.Integer:
                    VP.SetInt(key, EditorGUI.IntField(rect, VP.GetInt(key)));
                    break;
                case PrefTypes.Long:
                    VP.SetLong(key, EditorGUI.LongField(rect, VP.GetLong(key)));
                    break;
                case PrefTypes.Float:
                    VP.SetFloat(key, EditorGUI.FloatField(rect, VP.GetFloat(key)));
                    break;
                case PrefTypes.Double:
                    VP.SetDouble(key, EditorGUI.DoubleField(rect, VP.GetDouble(key)));
                    break;
                case PrefTypes.String:
                    VP.SetString(key, EditorGUI.TextField(rect, VP.GetString(key)));
                    break;
                case PrefTypes.Bool:
                    VP.SetBool(key, EditorGUI.Toggle(rect, VP.GetBool(key)));
                    break;
                case PrefTypes.Vector2:
                    VP.SetVector2(key, EditorGUI.Vector2Field(rect, string.Empty, VP.GetVector2(key)));
                    break;
                case PrefTypes.Vector3:
                    VP.SetVector3(key, EditorGUI.Vector3Field(rect, string.Empty, VP.GetVector3(key)));
                    break;
                case PrefTypes.Vector4:
                    VP.SetVector4(key, EditorGUI.Vector4Field(rect, string.Empty, VP.GetVector4(key)));
                    break;
                case PrefTypes.Quaternion:
                    VP.SetQuaternion(key, EditorGUI.Vector4Field(rect, string.Empty, VP.GetQuaternion(key).ToVector4()));
                    break;
                case PrefTypes.Color:
                    VP.SetColor(key, EditorGUI.ColorField(rect, labelTitle, VP.GetColor(key)));
                    break;
            }
        }


        /// <summary>
        /// Read R2 class. 
        /// (Allows to not lose preference keys when the R2 class is shared between different projects)
        /// </summary>
        private static void LoadPreferences()
        {
            var separator = new char[] { ',' };

            foreach (var name in R2.integers.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Integer, name))
                    VP.SetInt(name, 0);
            }
            foreach (var name in R2.floats.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Float, name))
                    VP.SetFloat(name, 0);
            }

            foreach (var name in R2.longs.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Long, name))
                    VP.SetLong(name, 0L);
            }

            foreach (var name in R2.doubles.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Double, name))
                    VP.SetDouble(name, 0d);
            }

            foreach (var name in R2.bools.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Bool, name))
                    VP.SetBool(name, false);
            }

            foreach (var name in R2.strings.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.String, name))
                    VP.SetString(name, string.Empty);
            }

            foreach (var name in R2.vector2s.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Vector2, name))
                    VP.SetVector2(name, Vector2.zero);
            }
            foreach (var name in R2.vector3s.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Vector3, name))
                    VP.SetVector3(name, Vector3.zero);
            }

            foreach (var name in R2.vector4s.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Vector4, name))
                    VP.SetVector4(name, Vector4.zero);
            }
            foreach (var name in R2.quaternions.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Quaternion, name))
                    VP.SetQuaternion(name, Quaternion.identity);
            }
            foreach (var name in R2.colors.Names.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!VP.HasKey(PrefTypes.Color, name))
                    VP.SetColor(name, Color.black);
            }
            VP.Save();
        }

        /// <summary>
        /// Generates <see cref="R2"/> class
        /// </summary>
        public static void GeneratePreferences()
        {
            EditorUtility.DisplayProgressBar(string.Empty, string.Empty, .5f);
            keys = VP.Keys();
            EditorUtils.CheckGenFolder();

            using (var writer = new StreamWriter($"{EditorUtils.GenScriptFolder}R2.cs"))
            {
                writer.WriteLine("#pragma warning disable IDE1006 // Naming Styles\n");

                writer.WriteLine("using UnityEngine;");
                writer.WriteLine("using InfinityEngine;\n");

                writer.WriteLine("namespace InfinityEngine.Serialization {");

                var comment = "This class is generated automaticaly by InfinityEngine, it contains constants used by many scripts.  DO NOT EDIT IT !";

                writer.WriteLine($"\t///<summary>\n\t/// {comment} \n\t///</summary>");
                writer.WriteLine("\tpublic static class R2\n\t{");

                var signature = string.Concat(
                    "\t\tpublic class PrefGetSet<T> \n\t\t{\n",
                    "\t\t\t///<summary>\n\t\t\t/// The key of the encapsulated preference \n\t\t\t///</summary>\n",
                    "\t\t\tpublic string Key { get; private set; }\n\n",
                    "\t\t\t///<summary>\n\t\t\t/// The value of the encapsulated preference, Call VP.Save() after you modify this value. \n\t\t\t///</summary>\n",
                    "\t\t\tpublic T Value { get => VP.Get<T>(Key); set => VP.Set(Key, value); }\n\n",
                    "\t\t\tpublic PrefGetSet(string key) => Key = key;\n\n",
                    "\t\t\tpublic static implicit operator T(PrefGetSet<T> self) => self.Value;\n",
                    "\t\t}\n"
                );

                writer.WriteLine(signature);

                var className = string.Empty;
                foreach (var pair in keys)
                {
                    className = pair.Key.ToString().ToLower();
                    writer.WriteLine($"\t\tpublic static class {className}s \n\t\t{{");
                    writer.WriteLine($"\t\t\tpublic const string Names = \"{string.Join(",", keys[pair.Key])}\";");
                    pair.Value.ForEach(it =>
                    {
                        writer.WriteLine(string.Format(Signatures[pair.Key], it));
                    });
                    writer.WriteLine("\t\t}");
                }
                writer.WriteLine("\t}");
                writer.WriteLine("}");
            }

            if (Instance != null)
            {
                Instance.ShowNotification(new GUIContent("Build Terminated"));
                preferences.ForEach((key, value) => value.Clear());
            }

            VP.Save();

            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();
        }


        [MenuItem("Tools/Infinity Engine/Preferences/Editor &p", false, -6)]
        private static void MenuEditor()
        {
            LoadPreferences();

            var editor = GetWindow<VPEditor>();
            editor.titleContent = new GUIContent(nameof(VP));
            editor.minSize = new Vector2(Width, Height);
            editor.Show();
        }

        [MenuItem("Tools/Infinity Engine/Preferences/Clear All (VP)")]
        private static void MenuClearAll()
        {
            VP.DeleteAll();
        }

        [MenuItem("Tools/Infinity Engine/Preferences/Delete All (VP)")]
        private static void MenuDeletePrefs()
        {
            VP.DeleteAll();
            GeneratePreferences();
        }

        [MenuItem("Tools/Infinity Engine/Preferences/Delete PlayerPrefs")]
        private static void MenuDeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Tools/Infinity Engine/Preferences/Delete EditorPrefs")]
        private static void MenuDeleteEditorPrefs()
        {
            EditorPrefs.DeleteAll();
        }

    }
}