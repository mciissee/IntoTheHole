/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
************************************************************************************************************************************/

using System;
using InfinityEngine.Attributes;
using UnityEngine.Events;

namespace InfinityEngine.Interpolations
{
    [Serializable]
    [CustomDrawer]
    public class InterpolationMethodCallbacks
    {
        [Message("Do an action before the begenning of the interpolation", MessageTypes.Info)]
        public bool enableOnStartCallback;

        [VisibleIf(nameof(enableOnStartCallback), MemberTypes.Field)]
        public UnityEvent onStart;

        [Message("Do an action during the execution of the interpolation", MessageTypes.Info)]
        public bool enableOnUpdateCallback;

        [VisibleIf(nameof(enableOnUpdateCallback), MemberTypes.Field)]
        public UnityEvent onUpdate;

        [Message("Do an action after the completion of each repetition (or only 1 time at the completion if the option 'Repeat' is sets to 0) of the interpolation", MessageTypes.Info)]
        public bool enableOnCompleteCallback;

        [VisibleIf(nameof(enableOnCompleteCallback), MemberTypes.Field)]
        public UnityEvent onComplete;

        [Message("Do an action after the of the interpolation", MessageTypes.Info)]
        public bool enableOnTerminateCallback;

        [VisibleIf(nameof(enableOnStartCallback), MemberTypes.Field)]
        public UnityEvent onTerminate;

        public void ApplyCallbacksToInterpolable(Interpolable interpolable)
        {
            if (enableOnStartCallback && onStart != null)
            {
                interpolable.OnStart(arg => onStart.Invoke());
            }
            if (enableOnCompleteCallback && onComplete != null)
            {
                interpolable.OnComplete(arg => onComplete.Invoke());
            }
            if (enableOnUpdateCallback && onUpdate != null)
            {
                interpolable.OnUpdate(arg => onUpdate.Invoke());
            }
            if (enableOnTerminateCallback && onTerminate != null)
            {
                interpolable.OnTerminate(arg => onTerminate.Invoke());
            }
        }
    }
}