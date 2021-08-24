/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/
using System;
using InfinityEngine.Extensions;
using System.Linq;
using InfinityEngine.Utils;

//// <summary>
//// This namespace provides access to resource management tools.
//// </summary>
namespace InfinityEngine.ResourceManagement
{
    /// <summary>
    /// Extension methods for <see cref="ISIResource"/>
    /// </summary>
    public static class ISIResourceExtensions
    {
        /// <summary>
        /// Get the <see cref="ResTypes"/> corresponding to the given <seealso cref="System.Type"/>
        /// </summary>
        /// <param name="type">The type of the resource</param>
        /// <returns><c>Res</c> object</returns>
        public static ResTypes ToResType(this Type type)
        {
            return ISIResource.AllResTypes.First(res => res.ToString() == type.Name);
        }

        /// <summary>
        /// Checks if this resource is a resource of type UnityEngine.Object
        /// </summary>
        /// <param name="res">this resource</param>
        /// <returns></returns>
        public static bool IsUnityResource(this ResTypes res)
        {
            return ISIResource.UnityResTypes.Contains(res);
        }

        /// <summary>
        /// Checks if this resource is a xml resource
        /// </summary>
        /// <param name="res">this resource</param>
        /// <returns></returns>
        public static bool IsXmlResource(this ResTypes res)
        {
            return ISIResource.XmlResTypes.Contains(res);
        }

        /// <summary>
        /// Checks if this resource is in the list of resources to include
        /// </summary>
        /// <param name="res">this resource</param>
        /// <returns></returns>
        public static bool IsToInclude(this ResTypes res)
        {
            return ISIResource.ResToInclude.Contains(res);
        }

        /// <summary>
        /// Checks if this resource is in the list of resources to exclude
        /// </summary>
        /// <param name="res">this resource</param>
        /// <returns><c>true</c> if the resource type should be included <c>false</c> otherwise</returns>
        public static bool IsToExclude(this ResTypes res)
        {
            return ISIResource.ResToExclude.Contains(res);
        }

        /// <summary>
        /// Gets the <c>System.Type</c> corresponding to the given <seealso cref="ResTypes"/>
        /// </summary>
        /// <param name="type">The type of the resource</param>
        /// <returns>System.Type object</returns>
        public static Type ToSystemType(this ResTypes type)
        {
            return TypeOF.Find(type.ToString());
        }
    }

}