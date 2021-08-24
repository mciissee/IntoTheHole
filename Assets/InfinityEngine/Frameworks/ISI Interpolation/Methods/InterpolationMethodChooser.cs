/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
************************************************************************************************************************************/

using UnityEngine;
using System;

namespace InfinityEngine.Interpolations
{
    /// <summary>
    /// Allows to choose the method to invokes using UnityEvent in the inspector in the case there is more
    /// than one <see cref="InterpolationMethod"/> component of the same type
    /// </summary>
    public class InterpolationMethodChooser : MonoBehaviour
    {
        private InterpolationMethod[] components;

        /// <summary>
        /// Invokes the method <see cref="InterpolationMethod.Invoke"/> of the component at the given index
        /// int the array returned by the method 'GetComponents&lt;InterpolationMethod&gt;();' of the MonoBehaviour.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Throwed when the index is not valid</exception>
        /// <param name="index">The index of the component in the list of the components of the same type</param>
        public void Invoke(int index)
        {
            if(components == null)
            {
                components = GetComponents<InterpolationMethod>();
            }
            try
            {
                components[index].Invoke();
            }
            catch
            {
                Debug.LogError("The index of the component is not valid", gameObject);
                ;
                throw new IndexOutOfRangeException();
            }
        }


        /// <summary>
        /// Invokes the method <see cref="InterpolationMethod.Invoke"/> of the <see cref="InterpolationMethod"/> component in the gameObject with the given name
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Throwed when the index is not valid</exception>
        /// <param name="index">The name of the component to select</param>
        public void Invoke(string name)
        {
            if (components == null)
            {
                components = GetComponents<InterpolationMethod>();
            }
            var invoked = false;
            foreach(var component in components)
            {
                if(component.Name == name)
                {
                    component.Invoke();
                    invoked = true;
                    break;
                }
            }
            if (!invoked)
            {
                Debug.LogError("The gameobject does not contain an InterpolationMethod component with the name " + name);
            }
        }
        /// <summary>
        /// Invokes the method <see cref="InterpolationMethod.InvokeReverse"/> of the <see cref="InterpolationMethod"/> component in the gameObject with the given name
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Throwed when the index is not valid</exception>
        /// <param name="index">The name of the component to select</param>
        public void InvokeReverse(string name)
        {
            if (components == null)
            {
                components = GetComponents<InterpolationMethod>();
            }
            var invoked = false;
            foreach (var component in components)
            {
                if (component.Name == name)
                {
                    component.InvokeReverse();
                    invoked = true;
                    break;
                }
            }
            if (!invoked)
            {
                Debug.LogError("The gameobject does not contain an InterpolationMethod component with the name " + name);
            }
        }

        /// <summary>
        /// Stop invoke the method <see cref="InterpolationMethod.Invoke"/> of the component at the given index
        /// int the array returned by the method 'GetComponents&lt;InterpolationMethod&gt;();' of the MonoBehaviour.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Throwed when the index is not valid</exception>
        /// <param name="index">The index of the component in the list of the components of the same type</param>
        public void StopInvoke(int index)
        {
            if (components == null)
            {
                components = GetComponents<InterpolationMethod>();
            }
            try
            {
                components[index].StopInvoke();
            }
            catch
            {
                Debug.LogError("The index of the component is not valid", gameObject);
                ;
                throw new IndexOutOfRangeException();
            }
        }


        /// <summary>
        /// Stop invoke the method <see cref="InterpolationMethod.Invoke"/> of the <see cref="InterpolationMethod"/> component in the gameObject with the given name
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Throwed when the index is not valid</exception>
        /// <param name="index">The name of the component to select</param>
        public void StopInvoke(string name)
        {
            if (components == null)
            {
                components = GetComponents<InterpolationMethod>();
            }
            var invoked = false;
            foreach (var component in components)
            {
                if (component.Name == name)
                {
                    component.StopInvoke();
                    invoked = true;
                    break;
                }
            }
            if (!invoked)
            {
                Debug.LogError("The gameobject does not contain an InterpolationMethod component with the name " + name);
            }
        }
    }
}