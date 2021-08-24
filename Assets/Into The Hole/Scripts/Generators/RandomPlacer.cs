/************************************************************************************************************************************													   *
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
namespace IntoTheHole
{
    using UnityEngine;

    /// <summary>
    ///   Places Pipe Items in random
    /// </summary>
    public class RandomPlacer : PipeItemGenerator
    {

        private float angleStep;
        private float pipeRotation;

        /// <summary>
        /// Creates a random shape by placing random number of <see cref="BaseItem"/> objects into the given <paramref name="pipe"/>
        /// </summary>
        /// <param name="pipe">The pipe</param>
        public override void GenerateItems(Pipe pipe)
        {
            angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
            for (var i = 0; i < pipe.CurveSegmentCount; i++)
            {
                pipeRotation = (Random.Range(0, pipe.PipeSegmentCount) + 0.5f) * 360f / pipe.PipeSegmentCount;
                pipe.RandomItem.Position(pipe, i * angleStep, pipeRotation);
            }
        }
    }
}