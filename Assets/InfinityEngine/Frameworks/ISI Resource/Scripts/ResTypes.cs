using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinityEngine.ResourceManagement
{
    /// <summary>
    ///  Resources encapsulated by <see cref="ISIResource"/>.
    /// </summary>
    [Serializable]
    public enum ResTypes
    {
        /// <summary>
        /// default value
        /// </summary>
        None = 0,

        /// <summary>
        /// string resources
        /// </summary>
        String = -5,

        /// <summary>
        /// boolean resources
        /// </summary>
        Boolean = -4,

        /// <summary>
        /// color resources
        /// </summary>
        Color = -3,

        /// <summary>
        /// integer resources
        /// </summary>
        Int32 = -2,

        /// <summary>
        /// xml resources
        /// </summary>
        XmlDocument = -1,

        /// <summary>
        /// Animation Clip resource
        /// </summary>
        AnimationClip = 1,

        /// <summary>
        /// AudioClip resource
        /// </summary>
        AudioClip = 2,

        /// <summary>
        /// Fonts resource
        /// </summary>
        Font = 3,

        /// <summary>
        /// GameObject resource
        /// </summary>
        GameObject = 4,

        /// <summary>
        /// GUISkin resource
        /// </summary>
        GUISkin = 5,

        /// <summary>
        /// Material resource
        /// </summary>
        Material = 6,

        /// <summary>
        /// Mesh resources
        /// </summary>
        Mesh = 7,

        /// <summary>
        /// PhysicMaterial resources
        /// </summary>
        PhysicMaterial = 8,

        /// <summary>
        /// PhysicsMaterial2D resources
        /// </summary>
        PhysicsMaterial2D = 9,

        /// <summary>
        /// Shader resource
        /// </summary>
        Shader = 10,

        /// <summary>
        /// Sprite resource
        /// </summary>
        Sprite = 11,

        /// <summary>
        /// TextAsset resource
        /// </summary>
        TextAsset = 12,

        /// <summary>
        /// Texture2D resource
        /// </summary>
        Texture2D = 13,
    }

}
