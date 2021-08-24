/************************************************************************************************************************************													   *
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using UnityEngine;

namespace IntoTheHole
{


    /// <summary>
    ///   Places PipeItems in circle
    /// </summary>
    public class CirclePlacer : PipeItemGenerator
    {
        private float start = 0;
        private float pipeRotation = 0;

        /// <summary>
        /// Creates a circular shape by placing random number of <see cref="BaseItem"/> objects into the given <paramref name="pipe"/>
        /// </summary>
        /// <param name="pipe">The pipe</param>
        public override void GenerateItems(Pipe pipe)
        {
            start = (Random.Range(0, pipe.PipeSegmentCount) + 0.5f);
            for (int i = 0; i < pipe.CurveSegmentCount; i += 2)
            {
                pipeRotation = (start + i) * 360f / pipe.PipeSegmentCount;
                pipe.RandomItem.Position(pipe, 0, pipeRotation);
            }
        }
    }
}