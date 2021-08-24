/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    * 
*************************************************************************************************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;

namespace InfinityEditor
{
    public partial class VPEditor
    {
        /// <summary>
        /// Regenerates automatically <see cref="R2"/> class if it not exists.
        /// </summary>
        [InitializeOnLoad]
        public static class Resolver
        {
            static Resolver()
            {
                if (!File.Exists(Path.Combine(Application.dataPath, EditorUtils.GenScriptFolder.Replace("Assets/", "") + "R2.cs")))
                {
                    GeneratePreferences();
                    LoadPreferences();
                }
            }
        }
    }
}