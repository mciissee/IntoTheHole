/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                              *
*************************************************************************************************************************************/

using System;
using InfinityEngine.Utils;

//// <summary>
//// This namespace provides access to serialization tools.
//// </summary>
namespace InfinityEngine.Serialization
{
    public static class PrefTypesExtensions
    {
        /// <summary>
        /// Gets the <c>System.Type</c> corresponding to the given <seealso cref="PrefTypes"/>
        /// </summary>
        /// <param name="type">The type of the preference</param>
        /// <returns>System.Type object</returns>
        public static Type ToSystemType(this PrefTypes type)
        {
            switch (type)
            {
                case PrefTypes.Bool:
                    return TypeOF.Boolean;
                case PrefTypes.Integer:
                    return TypeOF.Int32;
                case PrefTypes.Float:
                    return TypeOF.Float;
                case PrefTypes.Long:
                    return TypeOF.Long;
                case PrefTypes.Double:
                    return TypeOF.Double;
                case PrefTypes.String:
                    return TypeOF.String;
                case PrefTypes.Vector2:
                    return TypeOF.Vector2;
                case PrefTypes.Vector3:
                    return TypeOF.Vector3;
                case PrefTypes.Vector4:
                    return TypeOF.Vector4;
                case PrefTypes.Quaternion:
                    return TypeOF.Quaternion;
                case PrefTypes.Color:
                    return TypeOF.Color;

                default:
                    return null;
            }
        }

    }

}