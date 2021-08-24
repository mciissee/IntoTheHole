/************************************************************************************************************************************													   *
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Extensions;
using InfinityEngine.Interpolations;
using UnityEngine.UI;

namespace InfinityEngine.Interpolations
{
    /// <summary>
    ///  Fade the screen image
    /// </summary>
    public class ScreenFader : MonoBehaviour
    {

        public Image fader;
        public float duration;
        public EaseTypes ease;

        private void Start()
        {
            FadeOut();
        }

        public void FadeIn()
        {
            fader.gameObject.SetActive(true);
            fader.fillClockwise = true;
            fader.DOFade(0.0f, 1.0f, duration)
                .SetEase(ease)
                .Start();
        }

        public void FadeOut()
        {
            fader.gameObject.SetActive(true);
            fader.fillClockwise = false;
            fader.DOFade(1.0f, 0.0f, duration)
                .SetEase(ease)
                .OnTerminate(arg => fader.gameObject.SetActive(false))
                .Start();

        }
    }
}