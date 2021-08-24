#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using UnityEditor;
using UnityEngine;
using InfinityEngine;
using InfinityEngine.Extensions;
using InfinityEngine.ResourceManagement;
using InfinityEngine.Utils;

namespace InfinityEditor
{
    /// <summary>
    ///    Editor script of <see cref="ISIResource"/> class.      
    ///    This is also the class which generates the class <see cref="R"/>
    /// </summary>
    public partial class ISIResourceEditor : EditorWindow
    {
        #region Fields

        /// <summary>
        /// The path where the <c>ScriptableObject</c> <see cref="ISIResource"/> is generated
        /// </summary>
        public const string ScriptableObjectPath = "Assets/InfinityEngine/Gen/Resources/ISIResource.asset";

        /// <summary>
        /// The path where the <c>GameObject</c> <see cref="ReleaseDatabase"/> is generated
        /// </summary>
        public const string PrefabPath = "Assets/InfinityEngine/Gen/Resources/ISIResourcePrefab.prefab";

        /// <summary>
        /// The symbol which is at the starts and the ends of the name
        /// of the resources which will not be included in the database.
        /// </summary>
        private const string HiddenFileSymb = "___";

        /// <summary>
        /// The array of all folders that you cannot use in during the research for the resources
        /// </summary>
        public static readonly string[] ProhibedPaths = {
            "Assets/InfinityEngine", "Assets/Plugins", "Assets/Standard Assets"
        };

        private static readonly Color BackgroundColor = new Color(1, 1, 1, .2f);

        /// <summary>
        /// toolbar items.
        /// </summary>
        private readonly string[] Toolbar = { Strings.Settings, Strings.Database };

        /// <summary>
        /// editor window witdh
        /// </summary>
        private const int Width = 920;

        /// <summary>
        /// editor window height
        /// </summary>
        private const int Height = 500;

        /// <summary>
        /// editor toolbar witdh
        /// </summary>
        private const int ToolbarWidth = 280;

        /// <summary>
        /// editor toolbar height
        /// </summary>
        private const int ToolbarHeight = 20;

        /// <summary>
        /// drag and drop areas height
        /// </summary>
        private const int DragAndDropHeight = 20;

        /// <summary>
        /// a value indicating whether the plugin is running
        /// </summary>
        private static bool isBuilding;

        /// <summary>
        /// Update progresion
        /// </summary>
        private static float progress;

        /// <summary>
        /// Current step of the update
        /// </summary>
        private static int step;

        /// <summary>
        /// build steps (+1 is the step of the generation of resources like ISIResource prefab...)
        /// </summary>
        private static readonly int TotalSteps = ISIResource.UnityResTypes.Length + 3;

        /// <summary>
        /// resources found in assets folder.
        /// </summary>
        private static Dictionary<ResTypes, List<IKeyValue>> resources;

        /// <summary>
        /// current instance of this class
        /// </summary>
        private static EditorWindow instance;


        /// <summary>
        /// editor window left area scrollbar position
        /// </summary>
        private Vector2 leftScroll;

        /// <summary>
        /// editor window right area scrollbar position
        /// </summary>
        private Vector2 rightScroll;

        /// <summary>
        /// selected tab index in the toolbar
        /// </summary>
        private int selectedTabIndex;

        /// <summary>
        /// the type of the resources to display.
        /// </summary>
        private ResTypes selectedRes = ResTypes.AnimationClip;

        private bool showHelp;

        #endregion Fields

        #region Methods

        private void OnGUI()
        {

            resources = resources ?? ISIResource.Resources;

            foreach (var path in ProhibedPaths)
            {
                if (ISIResource.PathsToInclude.Contains(path))
                    ISIResource.PathsToInclude.Remove(path);

                if (!ISIResource.PathsToExclude.Contains(path))
                    ISIResource.PathsToExclude.Add(path);
            }

            GUI.Box(new Rect(ToolbarWidth, 40, position.width, 2), GUIContent.none, GUI.skin.box);
            GUI.Box(new Rect(ToolbarWidth - 2, 0, 2, position.height), GUIContent.none, GUI.skin.box);

            GUILayout.BeginHorizontal();

            var toolbarRect = GUILayoutUtility.GetRect(0, ToolbarHeight, GUILayout.Width(ToolbarWidth));
            selectedTabIndex = GUI.Toolbar(toolbarRect, selectedTabIndex, Toolbar, EditorStyles.toolbarButton);

            GUILayout.Space(20);

            showHelp = EditorUtils.DrawHeader(showHelp);

            GUILayout.EndHorizontal();

            switch (selectedTabIndex)
            {
                case 0:
                    DrawSettingTab();
                    break;
                case 1:
                    DrawDatabaseTab();
                    break;
            }
        }

        private void DrawSettingTab()
        {
            void leftArea()
            {
                leftScroll = GUILayout.BeginScrollView(leftScroll);
                GUILayout.Space(10);

                foreach (var type in ISIResource.AllResTypes)
                {
                    var toggleEnabled = ISIResource.ResToInclude.Contains(type);
                    GUILayout.BeginHorizontal();
                    if (toggleEnabled)
                    {
                        toggleEnabled = GUILayout.Toggle(toggleEnabled, type.ToString(), EditorStyles.radioButton);
                    }
                    else
                    {
                        GUILayout.Space(ToolbarWidth / 2);
                        toggleEnabled = GUILayout.Toggle(toggleEnabled, type.ToString(), EditorStyles.radioButton);
                    }
                    GUILayout.EndHorizontal();

                    if (toggleEnabled)
                    {
                        if (!type.IsToInclude())
                            ISIResource.ResToInclude.Add(type);

                        if (type.IsToExclude())
                            ISIResource.ResToExclude.Remove(type);
                    }
                    else
                    {
                        if (!type.IsToExclude())
                            ISIResource.ResToExclude.Add(type);

                        if (type.IsToInclude())
                            ISIResource.ResToInclude.Remove(type);
                    }
                }
                GUILayout.EndScrollView();
            }

            GUILayout.BeginArea(new Rect(0, ToolbarHeight, ToolbarWidth, position.height - ToolbarHeight));
            leftArea();
            GUILayout.EndArea();

            void rightArea()
            {
                rightScroll = GUILayout.BeginScrollView(rightScroll);

                GUILayout.Label(Strings.InclusionArea, EditorStyles.boldLabel);
                EditorUtils.ShowMessage(Strings.HelpInclusion, showHelp);
                DrawDropZone(true);

                GUILayout.Space(50);

                GUILayout.Label(Strings.ExclusionArea, EditorStyles.boldLabel);
                EditorUtils.ShowMessage(Strings.HelpExclusion, showHelp);
                DrawDropZone(false);

                GUILayout.EndScrollView();
            }

            GUILayout.BeginArea(new Rect(ToolbarWidth + 10, 50, position.width - ToolbarWidth - 10, position.height - 50));
            rightArea();
            GUILayout.EndArea();
        }

        private void DrawDatabaseTab()
        {
            void leftArea()
            {
                leftScroll = GUILayout.BeginScrollView(leftScroll);
                if (GUILayout.Button(Strings.Update))
                    Build();

                EditorUtils.ShowMessage(Strings.HelpUpdate, MessageType.Info, showHelp);

                GUILayout.Label($"{Strings.Resources} : {ISIResource.TotalResource}", EditorStyles.boldLabel);
                GUILayout.Label($"{Strings.Missings} : {ISIResource.MissingResource}", EditorStyles.boldLabel);

                GUILayout.Space(10);
                GUIStyle style;
                foreach (var type in ISIResource.UnityResTypes)
                {
                    style = (type == selectedRes) ? EditorStyles.helpBox : EditorStyles.toolbarButton;
                    if (GUILayout.Button(type.ToString(), style))
                        selectedRes = type;
                }
                GUILayout.EndScrollView();
            }
            GUILayout.BeginArea(new Rect(0, 40, ToolbarWidth - 2, position.height - 40));
            leftArea();
            GUILayout.EndArea();


            void rightArea()
            {
                rightScroll = GUILayout.BeginScrollView(rightScroll);
                if (resources[selectedRes].Any())
                {
                    GUILayout.BeginVertical();
                    resources[selectedRes].ForEach(resource =>
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(resource.Key, GUILayout.MaxWidth(300));
                        EditorGUILayout.ObjectField((UnityEngine.Object)resource.Obj, selectedRes.ToSystemType(), false);
                        GUILayout.EndHorizontal();
                    });
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.BeginArea(new Rect(ToolbarWidth + 10, 60, position.width - ToolbarWidth - 10, position.height - 60));
            rightArea();
            GUILayout.EndArea();
        }

        private void DrawDropZone(bool isInclusionZone)
        {
            try
            {
                var listA = ISIResource.PathsToInclude;
                var listB = ISIResource.PathsToExclude;
                if (!isInclusionZone)
                {
                    listA = ISIResource.PathsToExclude;
                    listB = ISIResource.PathsToInclude;
                }

                void process(string folder)
                {
                    if (string.IsNullOrEmpty(folder))
                        return;

                    folder = folder.Replace(Application.dataPath, "Assets");

                    if (!folder.StartsWith("Assets/", StringComparison.Ordinal))
                    {
                        ShowNotification(new GUIContent(Strings.FolderMustBeRelativeToAssets));
                        return;
                    }
                    var parent = string.Empty;
                    if (IsDirectory(folder))
                    {
                        if (ProhibedPaths.Contains(folder) || ProhibedPaths.Any(folder.Contains))
                        {
                            ShowNotification(new GUIContent(string.Format(Strings.DefaultPath, folder)));
                        }
                        else if (listA.Contains(folder))
                        {
                            listA.Remove(folder);
                        }
                        else
                        {
                            // add the new folder only if it's not a sub-foler of any added folder.
                            parent = listA.Find(folder.Contains);
                            if (parent == null)
                            {
                                listA = listA.Where(p => !p.Contains(folder)).ToList();
                                listA.Add(folder);
                                if (listB.Contains(folder))
                                {
                                    listB.Remove(folder);
                                }
                            }
                            else
                            {
                                ShowNotification(new GUIContent(Strings.FolderDuplication));
                            }
                        }
                    }
                }


                GUILayout.BeginHorizontal();

                if (EditorUtils.Drop(position.width - ToolbarWidth, DragAndDropHeight, Strings.DropFolderHere))
                {
                    DragAndDrop.paths?.ForEach(process);
                }
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(Strings.ChooseFolder))
                {
                    var path = EditorUtility.OpenFolderPanel(Strings.ChooseFolder, Application.dataPath, string.Empty);
                    process(path);
                }

                if (GUILayout.Button(Strings.ClearAll))
                    listA.Clear();

                GUILayout.EndHorizontal();


                for (int i = 0; i < listA.Count; i++)
                {
                    if (Event.current.type == EventType.DragPerform)
                        continue;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(listA[i], EditorStyles.boldLabel);
                    if (!ProhibedPaths.Contains(listA[i]) && GUILayout.Button("-", GUILayout.Width(20)))
                        listA.RemoveAt(i);
                    GUILayout.EndHorizontal();
                }


                ISIResource.PathsToInclude = listA;
                ISIResource.PathsToExclude = listB;
                if (!isInclusionZone)
                {
                    ISIResource.PathsToInclude = listB;
                    ISIResource.PathsToExclude = listA;
                }
            }
            catch { }
        }

        private static bool IsPathToExclude(string path)
        {
            return ISIResource.PathsToExclude.Any(path.Contains);
        }

        private static bool IsPathToInclude(string path)
        {
            return ISIResource.PathsToInclude.Any(path.Contains);
        }

        private static bool IsHiddenResourceName(string name)
        {
            return name.StartsWith(HiddenFileSymb, StringComparison.Ordinal)
                       && name.EndsWith(HiddenFileSymb, StringComparison.Ordinal);
        }

        private static bool IsDirectory(string path)
        {
            var attribute = (File.GetAttributes(path) & FileAttributes.Directory);
            return (attribute == FileAttributes.Directory);
        }

        private static void GenerateResources()
        {
            (string, string) joinFields(List<(string, string)> fields)
            {
                var names = string.Join(",", fields.Select(it => it.Item1));
                return ("Names", $"\t\t\tpublic const string Names = \"{names}\";");
            }

            (ResTypes, string)[] findXmls()
            {
                var assets = EditorUtils.FindAssetPaths(TypeOF.TextAsset);
                return assets.Select(it =>
                {
                    if (IsPathToInclude(it) && !IsPathToExclude(it))
                    {
                        foreach (var type in ISIResource.XmlResTypes)
                        {
                            if (it.EndsWith($"{type}_res.xml", StringComparison.Ordinal) && type.IsToInclude())
                                return (type, it);
                        }
                        if (it.EndsWith(".xml", StringComparison.Ordinal) && ResTypes.XmlDocument.IsToInclude())
                            return (ResTypes.XmlDocument, it);
                    }
                    return (ResTypes.None, string.Empty);
                }).Where(it => it.Item1 != ResTypes.None).ToArray();
            }

            string addSpecialXmlResource(ResTypes resType, string key, string value)
            {
                switch (resType)
                {
                    case ResTypes.Color:
                        ISIResource.Add(ResTypes.Color, key, Infinity.HexToColor(value));
                        return string.Format("\t\t\tpublic static Color {0} = ISIResource.Find<Color>(ResTypes.Color, \"{0}\");", key);

                    case ResTypes.Boolean:
                        ISIResource.Add(ResTypes.Boolean, key, bool.Parse(value));
                        return string.Format("\t\t\tpublic static bool {0} = ISIResource.Find<bool>(ResTypes.Boolean, \"{0}\");", key);

                    case ResTypes.Int32:
                        ISIResource.Add(ResTypes.Int32, key, int.Parse(value));
                        return string.Format("\t\t\tpublic static int {0} = ISIResource.Find<int>(ResTypes.Int32, \"{0}\");", key);

                    case ResTypes.String:
                        ISIResource.Add(ResTypes.String, key, value);
                        return string.Format("\t\t\tpublic static string {0} = ISIResource.Find<string>(ResTypes.String, \"{0}\");", key);
                    default:
                        return string.Empty;
                }
            }

            ResTypes res;
            var xmls = findXmls();
            resources = ISIResource.Resources;

            EditorUtils.CheckGenFolder();

            using (var writer = new StreamWriter($"{EditorUtils.GenScriptFolder}R.cs"))
            {
                void generateTags()
                {
                    var fields = new List<(string, string)>();

                    var tags = UnityEditorInternal.InternalEditorUtility.tags;
                    var fieldName = string.Empty;
                    var fieldSignature = string.Empty;
                    foreach (var tag in tags)
                    {
                        fieldName = CodeGenerationUtils.MakeIdentifier(tag);
                        fieldSignature = $"\t\t\tpublic const string {fieldName} = \"{tag}\";";
                        fields.Add((fieldName, fieldSignature));
                    }
                    fields.Insert(0, joinFields(fields));
                    writer.WriteLine("\t\tpublic static class tags {");
                    fields.ForEach(it => writer.WriteLine(it.Item2));
                    writer.WriteLine("\t\t}");
                }

                void generateLayerMasks()
                {
                    var fields = new List<(string, string)>();
                    var fieldName = string.Empty;
                    var fieldSignature = string.Empty;
                    for (int i = 0; i <= 31; i++) //user defined layers start with layer 8 and unity supports 31 layers
                    {
                        var layerName = LayerMask.LayerToName(i); //get the name of the layer
                        if (layerName.Length > 0)
                        {
                            fieldName = CodeGenerationUtils.MakeIdentifier(layerName);
                            fieldSignature = $"\t\t\tpublic const int {fieldName} = {LayerMask.GetMask(layerName)};";
                            fields.Add((fieldName, fieldSignature));
                        }
                    }
                    fields.Insert(0, joinFields(fields));
                    writer.WriteLine("\t\tpublic static class layers {");
                    fields.ForEach(it => writer.WriteLine(it.Item2));
                    writer.WriteLine("\t\t}");
                }

                void generateXmls()
                {
                    var paths = xmls.Where(it => it.Item1 == ResTypes.XmlDocument);
                    if (paths.Any())
                    {
                        var fields = new List<(string, string)>();
                        TextAsset asset;
                        foreach (var it in paths)
                        {
                            asset = AssetDatabase.LoadAssetAtPath<TextAsset>(it.Item2);
                            var fieldName = CodeGenerationUtils.MakeIdentifier(asset.name);
                            var fieldSignature = $"\t\t\tpublic static XmlDocument {fieldName} = ISIResource.Find<XmlDocument>(ResTypes.XmlDocument, \"{asset.name}\");";
                            if (fields.Any(field => field.Item1 == fieldName))
                            {
                                Debug.LogWarning(string.Format(Strings.ResourceDuplication, ResTypes.XmlDocument, asset.name));
                            }
                            else
                            {
                                ISIResource.Add(ResTypes.XmlDocument, asset.name, asset.text);
                                fields.Add((fieldName, fieldSignature));
                            }
                        }

                        fields.Insert(0, joinFields(fields));
                        writer.WriteLine($"\t\tpublic static class xmls {{");
                        fields.ForEach(it => writer.WriteLine(it.Item2));
                        writer.WriteLine("\t\t}");
                    }
                    else
                    {
                        writer.WriteLine("\t\tpublic static class xmls {");
                        writer.WriteLine("\t\t\tpublic const string Names = \"\";");
                        writer.WriteLine("\t\t}");
                    }
                }

                void generateSpacialXmls()
                {
                    var unusedTypes = new List<ResTypes> { ResTypes.String, ResTypes.Int32, ResTypes.Boolean, ResTypes.Color };
                    var paths = xmls.Where(it => it.Item1 != ResTypes.XmlDocument);
                    if (paths.Any())
                    {
                        TextAsset asset;
                        var fields = new List<(string, string)>();
                        var fieldName = string.Empty;
                        var fieldSignature = string.Empty;
                        foreach (var it in paths)
                        {
                            // ONLY ONE FILE FOR EACH SPECIAL XML RESOURCE
                            if (!unusedTypes.Contains(it.Item1))
                                continue;

                            unusedTypes.Remove(it.Item1);
                            asset = AssetDatabase.LoadAssetAtPath<TextAsset>(it.Item2);
                            fields.Clear();
                            EditorUtils.IterateXML(asset.text, node =>
                            {
                                fieldName = CodeGenerationUtils.MakeIdentifier(node.Attributes[0].Value);
                                if (fields.Any(field => field.Item1 == fieldName))
                                {
                                    Debug.LogWarning(string.Format(Strings.ResourceDuplication, it.Item1, asset.name));
                                }
                                else
                                {
                                    var value = node.FirstChild.Value;
                                    fieldSignature = addSpecialXmlResource(it.Item1, fieldName, value);
                                    fields.Add((fieldName, fieldSignature));
                                }
                            });

                            fields.Insert(0, joinFields(fields));

                            var className = it.Item1 == ResTypes.String ? "strings" : it.Item1.ToString().ToLower();
                            writer.WriteLine($"\t\tpublic static class {className} {{");
                            fields.ForEach(field => writer.WriteLine(field.Item2));
                            writer.WriteLine("\t\t}");
                        }
                    }

                    foreach (var type in unusedTypes)
                    {
                        var className = type == ResTypes.String ? "strings" : type.ToString().ToLower();
                        writer.WriteLine($"\t\tpublic static class {className} {{");
                        writer.WriteLine("\t\t\tpublic const string Names = \"\";");
                        writer.WriteLine("\t\t}");
                    }

                }

                writer.WriteLine("#pragma warning disable IDE1006 // Naming Styles");
                writer.WriteLine("using UnityEngine;");
                writer.WriteLine("using System.Xml;");

                writer.WriteLine("namespace InfinityEngine.ResourceManagement {");

                writer.WriteLine($"\t/// <summary>{Strings.AutoGeneratedComment}</summary>");
                writer.WriteLine("\tpublic static class R {");

                resources.ForEach((key, value) =>
                {
                    if (key.IsXmlResource())
                        return;

                    writer.WriteLine($"\t\tpublic static class {key.ToString().ToLower()} {{");

                    var names = string.Join(",", value.Select(elem => elem.Key));
                    writer.WriteLine($"\t\t\tpublic const string Names = \"{names}\";");

                    value.ForEach(it =>
                    {
                        var fieldName = CodeGenerationUtils.MakeIdentifier(it.Key);
                        var signature = $"\t\t\tpublic static {key} {fieldName} => ISIResource.Find<{key}>(ResTypes.{key}, \"{it.Key}\");";
                        writer.WriteLine(signature);
                    });

                    writer.WriteLine("\t\t}");
                });

                generateXmls();
                generateSpacialXmls();
                generateTags();
                generateLayerMasks();

                writer.WriteLine("\t}");
                writer.WriteLine("}");
            }
        }

        private static void EndBuild()
        {
            // GENERATE ISI RESOURCE SCRIPTABLE OBJECT AND PREFAB
            AssetDatabase.SaveAssets();
            var ouputPath = ScriptableObjectPath.Replace($"/{ISIResource.ScriptableObjectName}.asset", string.Empty);
            if (!Directory.Exists(ouputPath))
                Directory.CreateDirectory(ouputPath);

            var asset = AssetDatabase.LoadAssetAtPath<ISIResource>(ScriptableObjectPath);
            if (asset == null)
                AssetDatabase.CreateAsset(ISIResource.Instance, ScriptableObjectPath);
            else
                EditorUtility.CopySerialized(ISIResource.Instance, asset);

            ReleaseDatabase.CreatePrefab();
            PrefabUtility.CreatePrefab(PrefabPath, ReleaseDatabase.Instance.gameObject);
            DestroyImmediate(ReleaseDatabase.Instance.gameObject);

            GenerateResources();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (instance != null)
            {
                instance.ShowNotification(new GUIContent(Strings.Updated));
            }
            else
            {
                Debug.Log(Strings.Updated);
            }

            EditorUtility.ClearProgressBar();
            isBuilding = false;
        }

        private static void Build()
        {
            void addResourceIfNeeded(ResTypes resType, UnityEngine.Object asset)
            {
                if (IsHiddenResourceName(asset.name))
                    return;

                var fieldName = string.Empty;
                try
                {
                    fieldName = CodeGenerationUtils.MakeIdentifier(asset.name);
                }
                catch
                {
                    Debug.LogError(string.Format(Strings.LetterRequiredInAssetName, asset.name), asset);
                    return;
                }

                if (!ISIResource.Add(resType, asset.name, asset))
                    Debug.LogWarning(string.Format(Strings.ResourceDuplication, resType, fieldName), asset);
            }

            if (isBuilding)
                return;

            isBuilding = true;

            ISIResource.Clear();

            IEnumerator findResources()
            {
                step = 0;
                Type systemType;
                string[] paths;

                foreach (var resType in ISIResource.AllResTypes)
                {
                    progress = step / (float)TotalSteps;
                    EditorUtility.DisplayProgressBar(Strings.Update, string.Empty, progress);
                    if (resType.IsUnityResource() && resType.IsToInclude())
                    {
                        systemType = resType.ToSystemType();
                        paths = EditorUtils.FindAssetPaths(systemType)
                                     .Where(path => IsPathToInclude(path) && !IsPathToExclude(path))
                                     .ToArray();

                        foreach (var path in paths)
                        {
                            if (resType == ResTypes.Sprite)
                            {
                                var assets = AssetDatabase.LoadAllAssetsAtPath(path)
                                                          .Where(it => it.GetType() == systemType);

                                foreach (var asset in assets)
                                {
                                    addResourceIfNeeded(resType, asset);
                                    yield return null;
                                }
                            }
                            else
                            {
                                var asset = AssetDatabase.LoadAssetAtPath(path, systemType);
                                addResourceIfNeeded(resType, asset);
                            }
                            yield return null;
                        }
                    }
                    step++;
                    yield return null;
                }

                EndBuild();
            }

            EditorCoroutine.Start(findResources());
        }

        [MenuItem("Tools/Infinity Engine/Resources/Editor &r", false, -7)]
        private static void MenuEditor()
        {
            var editor = GetWindow<ISIResourceEditor>(nameof(ISIResource), true);
            editor.maxSize = new Vector2(Width, Height);
            instance = editor;
            editor.Show();
        }

        /// <summary>
        /// Searchs all resources and updates <see cref="ISIResource"/>.
        /// </summary>
        [MenuItem("Tools/Infinity Engine/Resources/Update &u")]
        private static void MenuUpdate()
        {
            if (!isBuilding)
                Build();
        }

        #endregion Methods

    }
}