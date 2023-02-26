using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GRV.ToolsModule.BroTools
{
    /// <summary>
    /// Simple tween system
    /// </summary>
    public class BroTweener : MonoBehaviour
    {
        /// <summary>
        /// Animation types
        /// </summary>
        public enum AnimTypeEnum
        {
            One,
            Loop,
            PingPong
        }

        [Serializable] public class TweenOnFinish : UnityEvent { }

        /// <summary>
        /// Type of tweener animation
        /// </summary>
        public AnimTypeEnum AnimType { get => _animType; set => _animType = value; }

        /// <summary>
        /// Curve of tweener animation
        /// </summary>
        public AnimationCurve Curve { get => _curve; set => _curve = value; }

        /// <summary>
        /// Duration of whole animation
        /// </summary>
        public float Duration { get => _duration; set => _duration = value; }

        /// <summary>
        /// Delay before start of playing
        /// </summary>
        public float Delay { get => _delay; set => _delay = value; }

        /// <summary>
        /// Start playing in OnEnable
        /// </summary>
        public bool PlayOnEnable { get => _playOnEnable; set => _playOnEnable = value; }

        /// <summary>
        /// Depending of Time.timeScale;
        /// </summary>
        public bool IgnoreTimeScale { get => _ignoreTimescale; set => _ignoreTimescale = value; }

        public Transform Target { get => _target; set => _target = value; }

        public bool IsRandomStart { get => _randomStart; set => _randomStart = value; }

        /// <summary>
        /// Current progress of tweener animation (0f to 1f)
        /// </summary>
        public float Progress { get; protected set; } = 0f;

        /// <summary>
        /// Current direction of tweener animation.
        /// 1 is forward; -1 is backward; 0 is not playing
        /// </summary>
        public int Direction { get; private set; } = 0;

        /// <summary>
        /// Multiplier for playing speed
        /// </summary>
        public float SpeedMultiplier { get; set; } = 1;

        /// <summary>
        /// Duration after multiplier applying
        /// </summary>
        public float RealDuration => Duration / SpeedMultiplier;

        public bool IsPlaying => Direction != 0;


        public List<TweenData> TweenDataList { get => _tweenDataList; set => _tweenDataList = value; }
        /// <summary>
        /// Finish of not loop animation
        /// </summary>
        public TweenOnFinish OnFinish => _onFinish;


        [SerializeField] private AnimTypeEnum _animType = AnimTypeEnum.One;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private float _duration = 1;
        [SerializeField] private float _delay = 0;
        [SerializeField] private bool _playOnEnable = true;
        [SerializeField] private Transform _target;
        [SerializeField] private bool _ignoreTimescale = false;
        [SerializeField] private bool _randomStart;

        [SerializeField] private List<TweenData> _tweenDataList = new List<TweenData>();
        [SerializeField] private TweenOnFinish _onFinish = new TweenOnFinish();


        /// <summary>
        /// Links from serialized data to realizations
        /// </summary>
        private Dictionary<TweenData, ITween> _tweens = new Dictionary<TweenData, ITween>();

        /// <summary>
        /// Timer for delaying before play
        /// </summary>
        private float _delayTimer = 0;
        /// <summary>
        /// Set for caching running tweens
        /// Need for resolving conflicts of same tweens
        /// </summary>
        private HashSet<int> runningTweenSet = new HashSet<int>();


        private void OnEnable()
        {
            if (PlayOnEnable)
            {
                float startProgress = IsRandomStart ? UnityEngine.Random.Range(0, 1f) : 0;
                Play(true, startProgress);
            }
        }

        private void OnDisable()
        {
            if (AnimType == AnimTypeEnum.One && IsPlaying)
            {
                SetProgress(Direction);
                Stop();
            }
        }


        public void Update()
        {
            if (Direction == 0) return;
            if (RealDuration == 0) return;

            float deltaTime = IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

            if (deltaTime == 0) return;

            // Waiting for delay
            if (_delayTimer > 0)
            {
                _delayTimer -= deltaTime;
                return;
            }


            float deltaProgress = Direction * (deltaTime / RealDuration);

            SetProgress(Progress + deltaProgress);

            // If we get end or start of animation, 
            // handle animation type
            if (Progress == 1 || Progress == 0)
            {
                switch (AnimType)
                {
                    case AnimTypeEnum.One:
                        // Stop tweener
                        Stop();
                        // If animation is not looped or pingpong, invoke events
                        OnFinish?.Invoke();
                        OnFinish.RemoveAllListeners();
                        break;

                    case AnimTypeEnum.Loop:
                        _delayTimer = Delay;
                        // Reset progress
                        Progress -= Direction;
                        break;

                    case AnimTypeEnum.PingPong:
                        // Just changing direction
                        Direction = -Direction;
                        break;
                }
            }
        }


        /// <summary>
        /// Set progress for tweener (from 0f to 1f)
        /// </summary>
        public void SetProgress(float progressValue)
        {
            Progress = Mathf.Clamp01(progressValue);

            runningTweenSet.Clear();

            //// Направление твинера задаёт направление итерации твинов
            //int first = Direction >= 0 ? 0 : _tweenDataList.Count - 1;
            //int last = _tweenDataList.Count - 1 - first;
            //int dir = Direction >= 0 ? 1 : -1;

            //for (int i = first; i >= 0 && i < _tweenDataList.Count; i += dir)

            for (int i = 0; i < _tweenDataList.Count; i++)
            {
                // Проставляем базовые значения твинера
                TweenData data = _tweenDataList[i];
                Transform target = Target;
                AnimationCurve curve = Curve;
                float leftEdge = 0;
                float rightEdge = 1;

                // Меняем значения, если они перезаписываются
                if (data.overrideTarget)
                {
                    target = data.target;
                }

                if (data.overrideScopes)
                {
                    leftEdge = data.leftEdge;
                    rightEdge = data.rightEdge;
                }

                if (data.overrideCurve)
                {
                    curve = data.curve;
                }

                // Если ни один таргет не задан, твинер работает на свой же трансформ
                if (target == null) target = transform;


                int hash = data.GetHashCode();

                // Если твиннер находится вне области твина
                if (Progress < leftEdge || Progress > rightEdge)
                {
                    if (!data.fillEmpty) continue;

                    // Если подобный твин уже запущен, пропускаем обработку,
                    // Иначе применяем стартовое или конечное значение твина

                    if (runningTweenSet.Contains(hash))
                    {
                        continue;
                    }
                    else
                    {
                        bool existInFuture = false;
                        for (int j = i + 1; j < _tweenDataList.Count; j++)
                        {
                            if (_tweenDataList[j].GetHashCode() == hash)
                            {
                                existInFuture = true;
                                break;
                            }
                        }
                        if (existInFuture) continue;
                    }
                }

                float progress = Mathf.InverseLerp(leftEdge, rightEdge, Progress);
                // Вычисляем значение твина на кривой
                float value = curve.Evaluate(progress);

                // Получаем нужный обработчик и применяем значение
                GetITweenByData(data).ApplyValue(target, data, value);

                // Сохраняем хэш проигрывающегося твина
                runningTweenSet.Add(hash);
            }
        }

        /// <summary>
        /// Set OnFinish event with removing previous listeners
        /// </summary>
        public void SetOnFinish(UnityAction action)
        {
            OnFinish.RemoveAllListeners();
            OnFinish.AddListener(action);
        }

        /// <summary>
        /// Set OnFinish event with removing previous listeners
        /// </summary>
        public void AddOnFinish(UnityAction action)
        {
            OnFinish.AddListener(action);
        }

        /// <summary>
        /// Play tweens forward
        /// </summary>
        /// <param name="withReset"> Reset tween progress, else continues from last state </param>
        /// <param name="startProgress"> Reset to this value </param>
        public void Play(bool withReset = true, float startProgress = 0)
        {
            if (withReset) SetProgress(startProgress);
            _delayTimer = Delay;
            Direction = 1;

            if (!enabled) enabled = true;
        }

        /// <summary>
        /// Play tweens backward
        /// </summary>
        public void PlayReverse(bool withReset = true, float startProgress = 1)
        {
            if (withReset) SetProgress(startProgress);
            _delayTimer = Delay;
            Direction = -1;

            if (!enabled) enabled = true;
        }

        /// <summary>
        /// Play tweens in direction
        /// </summary>
        /// <param name="dir"> Direction of playing </param>
        /// <param name="withReset"> Reset tween progress, else continues from last state </param>
        public void PlayDirectly(int dir, bool withReset = true)
        {
            Direction = (int)Mathf.Sign(dir);
            if (withReset) SetProgress(Direction >= 0 ? 0 : 1);
            _delayTimer = Delay;

            if (!enabled) enabled = true;
        }

        /// <summary>
        /// Stop playing
        /// </summary>
        public void Stop()
        {
            Direction = 0;

            if (!PlayOnEnable && enabled) enabled = false;
        }


        /// <summary>
        /// Add new tween to the list
        /// </summary>
        public TweenData AddTween(TweenData tweenData, bool setLast = false)
        {
            if (!setLast)
            {
                _tweenDataList.Add(null);
            }
            _tweenDataList[_tweenDataList.Count - 1] = tweenData;
            return tweenData;
        }


        /// <summary>
        /// Get tween data by the tag,
        /// which also can be set through right click in inspector
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public TweenData GetTween(string tag)
        {
            foreach (var data in _tweenDataList)
            {
                if (data.tag == tag) return data;
            }
            return null;
        }

        public TweenData GetTween(int index = 0)
        {
            if (index >= _tweenDataList.Count) return null;
            return _tweenDataList[index];
        }



        /// <summary>
        /// Get tween realization by tween data
        /// </summary>
        private ITween GetITweenByData(TweenData data)
        {
            if (!_tweens.ContainsKey(data))
            {
                ITween tween = TweensConfig.tweensTable[data.Type].GetBehaviourInstance();// (ITween)Activator.CreateInstance(tweensTable[data.Type]);
                _tweens.Add(data, tween);
            }
            return _tweens[data];
        }
    }
}
