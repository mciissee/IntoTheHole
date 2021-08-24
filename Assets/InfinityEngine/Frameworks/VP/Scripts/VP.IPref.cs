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
    public partial class VP
    {
        /// <summary>
        ///   Implemented by <see cref="Pref{T}"/> class, 
        ///   This interface allows to use the class <see cref="Pref{T}"/> 
        ///   with a generic List and Dictionary
        /// </summary>
        public interface IPref
        {

            /// <summary>
            /// This preference type
            /// </summary>
            PrefTypes Type { get; }

            /// <summary>
            /// This preference key
            /// </summary>
            string Key { get; set; }

            /// <summary>
            /// Return <c>true</c> if this key name starts and ends with '___' .
            /// </summary>
            bool IsHiddenPref { get; }

            /// <summary>
            /// Check if this preference key contains an error
            /// </summary>
            /// <returns></returns>
            bool IsValid { get; }

            /// <summary>
            /// An error message if there is an error in this key name
            /// </summary>
            string ErrorMessage { get; set; }
        }
    }
}