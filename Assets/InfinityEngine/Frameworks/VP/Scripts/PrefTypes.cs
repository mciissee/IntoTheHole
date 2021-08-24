/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                              *
*************************************************************************************************************************************/


//// <summary>
//// This namespace provides access to serialization tools.
//// </summary>
namespace InfinityEngine.Serialization
{
    /// <summary>
    ///   The type of the data serializables by <see cref="VP"/> class
    /// </summary>
    public enum PrefTypes
    {
        /// <summary>
        /// int preference
        /// </summary>
        Integer,
        /// <summary>
        /// float preference
        /// </summary>
        Float,
        /// <summary>
        /// double preference
        /// </summary>
        Double,
        /// <summary>
        /// long preference
        /// </summary>
        Long,
        /// <summary>
        /// string preference
        /// </summary>
        String,
        /// <summary>
        /// bool preference
        /// </summary>
        Bool,
        /// <summary>
        /// <a href="https://docs.unity3d.com/ScriptReference/Vector2.html"> Vector2 </a> preference
        /// </summary>
        Vector2,
        /// <summary>
        /// <a href="https://docs.unity3d.com/ScriptReference/Vector3.html"> Vector3 </a> preference
        /// </summary>
        Vector3,
        /// <summary>
        /// <a href="https://docs.unity3d.com/ScriptReference/Vector4.html"> Vector4 </a> preference
        /// </summary>
        Vector4,
        /// <summary>
        /// <a href="https://docs.unity3d.com/ScriptReference/Quaternion.html"> Quaternion </a> preference
        /// </summary>
        Quaternion,
        /// <summary>
        /// <a href="https://docs.unity3d.com/ScriptReference/Color.html"> Color </a> preference
        /// </summary>
        Color,
    }

}