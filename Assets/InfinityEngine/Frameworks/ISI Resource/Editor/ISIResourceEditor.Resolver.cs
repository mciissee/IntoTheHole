#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;
using InfinityEngine.ResourceManagement;
using System.Linq;

namespace InfinityEditor
{
    public partial class ISIResourceEditor
    {

        [InitializeOnLoad]
        internal class Resolver : MonoBehaviour
        {
            private static float interval = 15.0f;
            private static float nextTime;

            private static readonly string[] paths =
            {
                "InfinityEngine/Gen/Scripts/R.cs",
                "InfinityEngine/Gen/Resources/ISIResource.asset",
                "InfinityEngine/Gen/Resources/ISIResourcePrefab.prefab"
            };

            static Resolver()
            {
                nextTime = Time.realtimeSinceStartup + interval;

                EditorApplication.update -= Update;
                EditorApplication.update += Update;

                for (var i = 0; i < paths.Length; i++)
                {
                    paths[i] = Path.Combine(Application.dataPath, paths[i]);
                }
            }

            private static void Update()
            {
                if (ISIResource.MissingResource > 0 || IsMissingAsset)
                {
                    if (!isBuilding && Time.realtimeSinceStartup >= nextTime)
                    {
                        Debug.Log(Strings.UpdateRequired);
                        MenuUpdate();
                        nextTime = Time.realtimeSinceStartup + interval;
                    }
                }
            }

            /// <summary>
            /// Checks if there is a missing asset in the database.
            /// </summary>
            public static bool IsMissingAsset => paths.Any(it => !File.Exists(it));
        }
    }
}