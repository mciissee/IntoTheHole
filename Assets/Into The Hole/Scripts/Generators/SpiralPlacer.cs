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
    ///  Places Pipe Items in spiral
    /// </summary>
    public class SpiralPlacer : PipeItemGenerator
    {
        private float start;
        private float direction;
        private float angleStep;
        private float pipeRotation;

        /// <summary>
        /// Creates a spiral shape by placing random number of <see cref="BaseItem"/> objects into the given <paramref name="pipe"/>
        /// </summary>
        /// <param name="pipe">The pipe</param>
        public override void GenerateItems(Pipe pipe)
        {
            start = (Random.Range(0, pipe.PipeSegmentCount) + 0.5f);
            direction = Random.value < 0.5f ? 1f : -1f;

            angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
            for (int i = 0; i < pipe.CurveSegmentCount; i++)
            {
                pipeRotation = (start + i * direction) * 360f / pipe.PipeSegmentCount;
                pipe.RandomItem.Position(pipe, i * angleStep, pipeRotation);
            }
        }
    }
}