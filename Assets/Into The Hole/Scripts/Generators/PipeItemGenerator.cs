/************************************************************************************************************************************													   *
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
namespace IntoTheHole
{
    using System.Collections.Generic;
    using UnityEngine;


    /// <summary>
    ///   Base class for all <see cref="TrapItem"/> generators
    ///         
    /// </summary>
    public abstract class PipeItemGenerator : MonoBehaviour
    {

        /// <summary>
        /// List of items prefab to generate
        /// </summary>
        [SerializeField]
        protected List<BaseItem> itemPrefabs;

        /// <summary>
        /// Genetates random items into the given <paramref name="pipe"/>.
        /// </summary>
        /// <param name="pipe">The pipe</param>
        public abstract void GenerateItems(Pipe pipe);

    }
}