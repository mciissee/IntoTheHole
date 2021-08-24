/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/


using UnityEngine;
using UnityEngine.UI;
using InfinityEngine.Extensions;
using InfinityEngine.Interpolations;
using System.Collections.Generic;

namespace InfinityEngine.Interpolations
{
    /// <summary>
    ///  Component used to create slider
    /// </summary>
    public class SliderManager : MonoBehaviour
    {

        #region Fields

        [SerializeField] private RectTransform slidesParent;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private float distanceBetweenSlides;
        [SerializeField] private float speed;
        [SerializeField] private EaseTypes easeType;

        [SerializeField] private bool autoHideButton;
        [SerializeField] private bool disableOnHide;
        [SerializeField] private bool startFromMiddle;



        private int slideCount;
        private int current;
        private List<RectTransform> slides;
        private Interpolable interpolable;

        #endregion Fields

        private void Start()
        {
            slideCount = slidesParent.childCount;
            if (slideCount <= 1)
            {
                leftButton.gameObject.SetActive(false);
                rightButton.gameObject.SetActive(false);
                return;
            }

            slides = new List<RectTransform>();
            for (var i = 0; i < slideCount; i++)
            {
                slides.Add(slidesParent.GetChild(i) as RectTransform);
            }

            leftButton.onClick.AddListener(() => {

                Slide(-distanceBetweenSlides);

            });

            rightButton.onClick.AddListener(() =>
            {
                Slide(distanceBetweenSlides);

            });

            slides[0].SetGameObjectActive(true);

            var position = Vector3.zero;
            slides[0].anchoredPosition3D = position;
            for (int i = 1; i < slideCount; i++)
            {
                position.x += distanceBetweenSlides;
                slides[i].SetGameObjectActive(!disableOnHide);
                slides[i].anchoredPosition3D = position;
            }

            if (startFromMiddle)
            {
                slides[0].SetGameObjectActive(false);

                current = (slideCount + 1) / 2;
                for (int i = 0; i <= current; i++)
                {
                    position = slides[i].anchoredPosition3D;
                    position.x -= distanceBetweenSlides;
                    slides[i].anchoredPosition3D = position;
                }
                current--;
                slides[current].SetGameObjectActive(true);

            }

        }

        private bool Slide(float offset)
        {
            if (interpolable != null && interpolable.IsPlaying)
            {
                return false;
            }

            var lastCurrent = current;
            current += offset == distanceBetweenSlides ? -1 : 1;

            if (current < 0 || current >= slideCount)
            {
                current = lastCurrent;
                return false;
            }
            var position = Vector3.zero;
            for (int index = 0; index < slideCount; index++)
            {
                slides[index].gameObject.SetActive(true);
                position = slides[index].anchoredPosition3D;
                position.x += offset;
                interpolable = slides[index].DOMoveAnchor3D(position, speed).SetEase(easeType).Start();
            }

            Infinity.After(speed, () => {
                for (int index = 0; index < slideCount; index++)
                {
                    slides[index].gameObject.SetActive(index == current || !disableOnHide);
                }
            });
            return true;
        }

        private void Update()
        {
            if (autoHideButton)
            {
                rightButton.gameObject.SetActive(current > 0);
                leftButton.gameObject.SetActive(current < slideCount - 1);
            }
        }

    }
}