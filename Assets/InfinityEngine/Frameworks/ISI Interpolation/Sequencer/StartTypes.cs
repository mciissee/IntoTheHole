using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinityEngine.Interpolations
{

    /// <summary>
    ///  Animation start type.
    /// </summary>
    [Serializable]
    public enum StartTypes
    {
        /// <summary>
        /// Dont play animation at start
        /// </summary>
        None,
        /// <summary>
        /// Play only In animation on start.
        /// </summary>
        PlayInOnStart,
        /// <summary>
        /// Play only Out animation on start
        /// </summary>
        PlayOutOnStart,
        /// <summary>
        /// Play only Loop animation on start
        /// </summary>
        PlayLoopOnStart,
        /// <summary>
        ///  Play In and Out animation on start (use a start delay for Out animation)
        /// </summary>
        PlayInAndOutOnStart,
        /// <summary>
        /// Play In and Loop animation on start (use a start delay for Loop animation)
        /// </summary>
        PlayInAndLoopOnStart
    }

}
