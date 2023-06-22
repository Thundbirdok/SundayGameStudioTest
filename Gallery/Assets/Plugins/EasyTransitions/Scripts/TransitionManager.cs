using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace EasyTransition
{

    public class TransitionManager : MonoBehaviour
    {        
        [SerializeField] 
        private Transition transitionTemplate;

        public bool RunningTransition { get; private set; }

        public UnityAction OnTransitionBegin;
        public UnityAction OnTransitionCutPointReached;
        public UnityAction OnTransitionEnd;

        private static TransitionManager _instance;

        private void Awake()
        {
            _instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        public static TransitionManager Instance()
        {
            if (_instance == null)
            {
                Debug.LogError("You tried to access the instance before it exists.");
            }

            return _instance;
        }

        /// <summary>
        /// Starts a transition without loading a new level.
        /// </summary>
        /// <param name="transition">The settings of the transition you want to use.</param>
        /// <param name="startDelay">The delay before the transition starts.</param>
        public void Transition(TransitionSettings transition, float startDelay)
        {
            if (transition == null || RunningTransition)
            {
                Debug.LogError("You have to assign a transition.");
                return;
            }

            RunningTransition = true;
            StartCoroutine(Timer(startDelay, transition));
        }

        /// <summary>
        /// Loads the new Scene with a transition.
        /// </summary>
        /// <param name="sceneName">The name of the scene you want to load.</param>
        /// <param name="transition">The settings of the transition you want to use to load you new scene.</param>
        /// <param name="startDelay">The delay before the transition starts.</param>
        public void Transition(string sceneName, TransitionSettings transition, float startDelay)
        {
            if (transition == null || RunningTransition)
            {
                Debug.LogError("You have to assign a transition.");
                return;
            }

            RunningTransition = true;
            StartCoroutine(Timer(sceneName, startDelay, transition));
        }

        /// <summary>
        /// Loads the new Scene with a transition.
        /// </summary>
        /// <param name="sceneIndex">The index of the scene you want to load.</param>
        /// <param name="transition">The settings of the transition you want to use to load you new scene.</param>
        /// <param name="startDelay">The delay before the transition starts.</param>
        public void Transition(int sceneIndex, TransitionSettings transition, float startDelay)
        {
            if (transition == null || RunningTransition)
            {
                Debug.LogError("You have to assing a transition.");
                return;
            }

            RunningTransition = true;
            StartCoroutine(Timer(sceneIndex, startDelay, transition));
        }

        private IEnumerator Timer(string sceneName, float startDelay, TransitionSettings transitionSettings)
        {
            yield return new WaitForSecondsRealtime(startDelay);

            OnTransitionBegin?.Invoke();

            var template = Instantiate(transitionTemplate);
            template.transitionSettings = transitionSettings;

            var transitionTime = transitionSettings.transitionTime;
            
            if (transitionSettings.autoAdjustTransitionTime)
            {
                transitionTime = transitionTime / transitionSettings.transitionSpeed;
            }

            yield return new WaitForSecondsRealtime(transitionTime);

            OnTransitionCutPointReached?.Invoke();

            SceneManager.LoadScene(sceneName);

            yield return new WaitForSecondsRealtime(transitionSettings.destroyTime);

            RunningTransition = false;
            OnTransitionEnd?.Invoke();
        }

        private IEnumerator Timer(int sceneIndex, float startDelay, TransitionSettings transitionSettings)
        {
            yield return new WaitForSecondsRealtime(startDelay);

            OnTransitionBegin?.Invoke();

            var template = Instantiate(transitionTemplate);
            template.transitionSettings = transitionSettings;

            var transitionTime = transitionSettings.transitionTime;
            
            if (transitionSettings.autoAdjustTransitionTime)
            {
                transitionTime /= transitionSettings.transitionSpeed;
            }

            yield return new WaitForSecondsRealtime(transitionTime);

            OnTransitionCutPointReached?.Invoke();

            SceneManager.LoadScene(sceneIndex);

            yield return new WaitForSecondsRealtime(transitionSettings.destroyTime);

            RunningTransition = false;
            OnTransitionEnd?.Invoke();
        }

        private IEnumerator Timer(float delay, TransitionSettings transitionSettings)
        {
            yield return new WaitForSecondsRealtime(delay);

            OnTransitionBegin?.Invoke();

            var template = Instantiate(transitionTemplate);
            template.transitionSettings = transitionSettings;

            var transitionTime = transitionSettings.transitionTime;
            
            if (transitionSettings.autoAdjustTransitionTime)
            {
                transitionTime /= transitionSettings.transitionSpeed;
            }

            yield return new WaitForSecondsRealtime(transitionTime);

            OnTransitionCutPointReached?.Invoke();

            template.OnSceneLoad(SceneManager.GetActiveScene(), LoadSceneMode.Single);

            yield return new WaitForSecondsRealtime(transitionSettings.destroyTime);

            RunningTransition = false;
            
            OnTransitionEnd?.Invoke();
        }

        private IEnumerator Start()
        {
            while (gameObject.activeInHierarchy)
            {
                //Check for multiple instances of the Transition Manager component
                var managerCount = GameObject.FindObjectsOfType<TransitionManager>(true).Length;
                if (managerCount > 1)
                {
                    Debug.LogError($"There are {managerCount.ToString()} Transition Managers in your scene. Please ensure there is only one Transition Manager in your scene or overlapping transitions may occur.");
                }

                yield return new WaitForSecondsRealtime(1f);
            }
        }
    }

}
