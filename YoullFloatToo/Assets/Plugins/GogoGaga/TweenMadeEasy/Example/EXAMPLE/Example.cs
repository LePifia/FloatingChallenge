using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GogoGaga.TME
{
    public class Example : MonoBehaviour
    {
        public GameObject[] UiExamples;
        public GameObject[] Examples;
        public void Play()
        {
            LeantweenCustomAnimator[] Animations = FindObjectsOfType<LeantweenCustomAnimator>();
            for (int i = 0; i < Animations.Length; i++)
            {
                Animations[i].PlayAnimation();
            }
        }

        bool isUI = false;
        public void StartUiExample()
        {
            isUI = true;
            Current = 0;
            UiExamples[0].SetActive(true);
        }
        public void StartExample()
        {
            isUI = false;
            Current = 0;
            Examples[0].SetActive(true);
        }

        int Current;
        public void ChangeLeftRight(bool right)
        {
            if (isUI)
            {
                if (right)
                {
                    if (Current < UiExamples.Length - 1)
                        Current++;

                }
                else
                {
                    if (Current > 0)
                        Current--;

                }


                for (int i = 0; i < UiExamples.Length; i++)
                {
                    UiExamples[i].SetActive(false);
                }

                UiExamples[Current].SetActive(true);

            }
            else
            {
                if (right)
                {
                    if (Current < Examples.Length - 1)
                        Current++;

                }
                else
                {
                    if (Current > 0)
                        Current--;

                }

                for (int i = 0; i < Examples.Length; i++)
                {
                    Examples[i].SetActive(false);
                }

                Examples[Current].SetActive(true);
            }
        }
    }
}