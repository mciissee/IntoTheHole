/************************************************************************************************************************************													   *
* Developed by Mamadou Cisse  and Jasper Flick                                                                                      *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
namespace IntoTheHole
{
    using InfinityEngine.Extensions;
    using UnityEngine;

    /// <summary>
    ///   Class in charge to manage <see cref="Pipe"/> instances      
    /// </summary>
    public class PipeManager : MonoBehaviour
    {

        #region Fields
        [SerializeField]
        private int pipeCount = 6;
        [SerializeField]
        private Pipe pipePrefab;

        private Pipe[] pipes;

        #endregion Fields

        #region Methods

        private void Awake()
        {
            pipes = new Pipe[pipeCount];
            for (int i = 0; i < pipes.Length; i++)
            {
                Pipe pipe = pipes[i] = Instantiate<Pipe>(pipePrefab);
                pipe.transform.SetParent(transform, false);
            }
        }

        /// <summary>
        /// Places the first pipe of '<see cref="pipes"/>' array and align others pipes with him
        /// </summary>
        /// <returns>pipes[1]</returns>
        public Pipe SetupFirstPipe()
        {

            Pipe pipe;
            for (int i = 0; i < pipes.Length; i++)
            {
                pipe = pipes[i];
                pipe.RebuildMesh();

                if (i > 0)
                {
                    pipe.AlignWith(pipes[i - 1]);
                }
            }
            AlignNextPipeWithOrigin();
            transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius);
            return pipes[1];
        }

        /// <summary>
        /// Shifts <see cref="pipes"/> array to place the first pipe in last position.<br/>
        /// Exemple by reasoning on this int array '[1, 2, 3, 4, 5]' , we get [2, 3, 4, 5, 1].
        /// Align <see cref="pipe"/>[1] with the origin
        /// </summary>
        /// <returns><see cref="pipes"/>[1]</returns>
        public Pipe SetupNextPipe()
        {

            pipes.Shift();
            AlignNextPipeWithOrigin();
            pipes[pipes.Length - 1].RebuildMesh();
            pipes[pipes.Length - 1].AlignWith(pipes[pipes.Length - 2]);

            transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius);
            return pipes[1];
        }

        /// <summary>
        /// this method align <see cref="pipe"/> pipe[1] with the origin.
        /// There is 3 steps :<br/>
        /// - Set all pipe in <see cref="pipe"/> array  child of <see cref="pipe"/> [1].<br/>
        /// - Align  <see cref="pipe"/>[1] with origin.<br/>
        /// - Makes this Transform parent of all children in pipes[1].<br/>
        /// </summary>
        private void AlignNextPipeWithOrigin()
        {
            Transform transformToAlign = pipes[1].transform;
            ChangePipesParent(transformToAlign);

            transformToAlign.localPosition = Vector3.zero;
            transformToAlign.localRotation = Quaternion.identity;

            ChangePipesParent(transform);
        }

        /// <summary>
        /// Makes the Transform '<paramref name="newParent"/>' the parent of
        /// all pipes in <see cref="pipes"/> except the pipe at the index 1.
        /// </summary>
        /// <param name="newParent">New parent of all pipes except <see cref="pipe"/>[1]</param>
        private void ChangePipesParent(Transform newParent)
        {
            int index = 0;
            pipes.ForEach(pipe =>
            {
                if (index != 1)
                    pipe.transform.SetParent(newParent);

                index++;
            });
        }

        #endregion Methods
    }
}