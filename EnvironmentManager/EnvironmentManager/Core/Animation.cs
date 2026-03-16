using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GuildSaber.Mod.Helpers
{

    public class FloatAnimation : MonoBehaviour
    {
        private float m_Duration;

        private float m_End;

        private float m_Start;

        private bool m_Started;

        private float m_StartTime;

        private float m_ValueDuration;

        ////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Get value from current time, start and end
        /// </summary>
        public void Update()
        {
            if (m_Started == false) return;

            var l_Prct = (Time.realtimeSinceStartup - m_StartTime) / m_Duration;

            var l_Value = m_Start + m_ValueDuration * l_Prct;

            OnChange?.Invoke(l_Value);

            if (l_Prct > 1)
            {
                m_Started = false;
                OnFinished?.Invoke(l_Value);
            }
        }

        public event Action<float> OnChange = delegate { };

        public event Action<float> OnFinished = delegate { };

        public float GetStart() => m_Start;
        public float GetEnd() => m_End;
        public bool IsPlaying() => m_Started;
        public float Duration() => m_Duration;

        protected virtual void OnPlay()
        {
        }

        protected virtual void OnStop()
        {
        }

        protected virtual void OnInit()
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Init anim
        /// </summary>
        /// <param name="p_Start"></param>
        /// <param name="p_Value"></param>
        /// <param name="p_Duration"></param>
        public void Init(float p_Start, float p_Value, float p_Duration)
        {
            m_Start = p_Start;
            m_End = p_Value;
            m_ValueDuration = m_End - m_Start;
            m_Duration = p_Duration;
            OnInit();
        }

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Start animation
        /// </summary>
        public void Play()
        {
            m_StartTime = Time.realtimeSinceStartup;

            if (m_Duration == 0 || float.IsPositiveInfinity(m_Duration) || float.IsNegativeInfinity(m_Duration))
            {
                OnChange?.Invoke(m_End);
                OnFinished?.Invoke(m_End);
                DestroyImmediate(gameObject);
                return;
            }

            m_Started = true;
            OnPlay();
        }

        /// <summary>
        ///     Stop current animation
        /// </summary>
        public void Stop()
        {
            m_Started = false;
            OnStop();
        }
    }

    public class Vector3Animation : MonoBehaviour
    {
        public Vector3 m_Start;

        public Vector3 m_End;

        public float m_Duration;

        private Vector3 m_Current;

        private int m_FinishedAnimCount;

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        private FloatAnimation m_XAnim;
        private FloatAnimation m_YAnim;
        private FloatAnimation m_ZAnim;

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public event Action<Vector3> OnVectorChange;

        public event Action<Vector3> OnFinished;

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Create an animation in GameObject if no existing, else return existing
        /// </summary>
        /// <param name="p_GameObject">Target GameObject</param>
        /// <param name="p_Animation">Returned animation</param>
        public static void AddAnim(GameObject p_GameObject, out Vector3Animation p_Animation)
        {
            var l_ExistingAnim = p_GameObject.GetComponent<Vector3Animation>();
            if (l_ExistingAnim != null)
                p_Animation = l_ExistingAnim;
            else
                p_Animation = p_GameObject.AddComponent<Vector3Animation>();
        }

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Init animation
        /// </summary>
        /// <param name="p_Start">Start value</param>
        /// <param name="p_Value">End value</param>
        /// <param name="p_Duration">Animation duration</param>
        public void Init(Vector3 p_Start, Vector3 p_Value, float p_Duration)
        {
            m_FinishedAnimCount = 0;
            m_Start = p_Start;
            m_End = p_Value;

            m_Duration = p_Duration;
        }

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     If all float animations have finished, invoke OnFinished event
        /// </summary>
        private void CheckFinishedAnims()
        {
            if (m_FinishedAnimCount == 3)
                OnFinished?.Invoke(m_Current);
        }

        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     Play animation
        /// </summary>
        public void Play()
        {
            if (m_XAnim is null)
            {
                m_XAnim = gameObject.AddComponent<FloatAnimation>();
                m_XAnim.OnChange += p_Val =>
                {
                    m_Current = new Vector3(p_Val, m_Current.y, m_Current.z);
                    OnVectorChange?.Invoke(m_Current);
                };
                m_XAnim.OnFinished += p_Val =>
                {
                    m_FinishedAnimCount += 1;
                    CheckFinishedAnims();
                };
            }

            if (m_YAnim is null)
            {
                m_YAnim = gameObject.AddComponent<FloatAnimation>();
                m_YAnim.OnChange += p_Val =>
                {
                    m_Current = new Vector3(m_Current.x, p_Val, m_Current.z);
                    OnVectorChange?.Invoke(m_Current);
                };
                m_YAnim.OnFinished += p_Val =>
                {
                    m_FinishedAnimCount += 1;
                    CheckFinishedAnims();
                };
            }

            if (m_ZAnim is null)
            {
                m_ZAnim = gameObject.AddComponent<FloatAnimation>();
                m_ZAnim.OnChange += p_Val =>
                {
                    m_Current = new Vector3(m_Current.x, m_Current.y, p_Val);
                    OnVectorChange?.Invoke(m_Current);
                };
                m_ZAnim.OnFinished += p_Val =>
                {
                    m_FinishedAnimCount += 1;
                    CheckFinishedAnims();
                };
            }

            m_XAnim.Init(m_Start.x, m_End.x, m_Duration);
            m_YAnim.Init(m_Start.y, m_End.y, m_Duration);
            m_ZAnim.Init(m_Start.z, m_End.z, m_Duration);

            m_XAnim.Play();
            m_YAnim.Play();
            m_ZAnim.Play();
        }

        /// <summary>
        ///     Stop current animation
        /// </summary>
        public void Stop()
        {
            foreach (var l_Current in gameObject.GetComponents<FloatAnimation>())
                l_Current.Stop();
        }
    }

    internal class FastAnimator : MonoBehaviour
    {
        private static FastAnimator s_Instance;

        ////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        protected List<FloatAnimData> m_FloatAnimations = new List<FloatAnimData>();
        protected List<FloatAnimData> m_FloatAnimationsToEnd = new List<FloatAnimData>();
        protected List<Vector3AnimData> m_Vector3Animations = new List<Vector3AnimData>();

        ////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        internal static FastAnimator Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;

                s_Instance = new GameObject("GuildSaberFastAnimator").AddComponent<FastAnimator>();
                DontDestroyOnLoad(s_Instance);
                return s_Instance;
            }
            set => s_Instance = value;
        }

        ////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        public void Update()
        {
            for (var l_I = 0; l_I < m_FloatAnimations.Count; l_I++)
            {
                var l_Item = m_FloatAnimations[l_I];
                ParseFloatAnimData(l_Item, l_Item.AddDeltaTime, Time.realtimeSinceStartup, l_I);
            }

            if (!m_FloatAnimationsToEnd.Any()) return;

            foreach (var l_Item in m_FloatAnimationsToEnd)
            {
                m_FloatAnimations.Remove(l_Item);
            }

            m_FloatAnimationsToEnd.Clear();
        }

        ////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        public static void Animate(List<FloatAnimKey> p_Keys, Action<float> p_Callback, Action p_OnFinished = null)
        {
            if (p_Keys.Count < 2)
            {
                throw new Exception("Not enough keys to run an animation, 2 required");
            }

            var l_NewAnimation = new FloatAnimData(p_Keys, p_Callback, p_OnFinished);
            Instance.m_FloatAnimations.Add(l_NewAnimation);
        }

        ////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        private float ParseFloatAnimData(FloatAnimData p_FloatAnimData, float p_StartTime, float p_DeltaTime, int p_IndexInList)
        {
            var l_Time = p_DeltaTime - p_StartTime;

            if (l_Time > p_FloatAnimData.LastKey.Time)
            {
                p_FloatAnimData.OnFinished?.Invoke();
                p_FloatAnimData.Callback.Invoke(p_FloatAnimData.LastKey.Value);

                m_FloatAnimationsToEnd.Add(p_FloatAnimData);

                return p_FloatAnimData.LastKey.Value;
            }

            ////////////////////////////////////////////////

            if (p_FloatAnimData.NextKey.Time == 0 || l_Time > p_FloatAnimData.NextKey.Time)
            {
                var l_KeysCount = p_FloatAnimData.Keys.Count;
                var l_Keys = p_FloatAnimData.Keys;

                ////////////////////////////////////////////////

                for (var l_I = 0; l_I < l_KeysCount; l_I++)
                {
                    if (!(l_Keys[l_I].Time > l_Time))
                        continue;

                    p_FloatAnimData.ActualKey = p_FloatAnimData.NextKey;
                    p_FloatAnimData.NextKey = l_Keys[l_I];

                    m_FloatAnimations[p_IndexInList] = p_FloatAnimData;
                    break;
                }
            }

            var l_ActualKey = p_FloatAnimData.ActualKey;
            var l_NextKey = p_FloatAnimData.NextKey;
            var l_KeyIntervalTime = l_Time - l_ActualKey.Time;
            var l_KeyIntervalDuration = l_NextKey.Time - l_ActualKey.Time;

            var l_Value = CalculateFloatValue(l_ActualKey.Value, l_NextKey.Value, l_ActualKey.Exponent, l_KeyIntervalTime, l_KeyIntervalDuration);

            p_FloatAnimData.Callback.Invoke(
                l_Value
            );

            return l_Value;
        }

        private static float CalculateFloatValue(float p_Start, float p_End, float p_Exponent, float p_Time, float p_Duration)
            => p_Start + (float)Math.Pow(p_Time / p_Duration, p_Exponent) * (p_End - p_Start);

        internal enum EAnimType
        {
            Float,
            Vector
        }

        internal struct FloatAnimKey
        {
            public float Value;
            public float Exponent;
            public float Time;

            public FloatAnimKey(float p_Value, float p_Time, float p_Exponent = 1)
            {
                Value = p_Value;
                Time = p_Time;
                Exponent = p_Exponent;
            }
        }

        internal struct FloatAnimData
        {
            public List<FloatAnimKey> Keys;
            public Action<float> Callback;
            public Action OnFinished;
            public FloatAnimKey NextKey;
            public FloatAnimKey ActualKey;
            public FloatAnimKey LastKey;
            public float AddDeltaTime;

            public FloatAnimData(List<FloatAnimKey> p_Keys, Action<float> p_Callback, Action p_OnFinished)
            {
                Keys = p_Keys;
                Callback = p_Callback;
                OnFinished = p_OnFinished;
                NextKey = new FloatAnimKey(p_Keys[0].Value, 0);
                ActualKey = NextKey;
                AddDeltaTime = Time.realtimeSinceStartup;
                LastKey = p_Keys.Any() ? p_Keys.Last() : default(FloatAnimKey);
            }
        }

        internal struct Vector3AnimKey
        {
            public Vector3 Start;
            public Vector3 End;
            public Vector3 Exponents;
            public float Duration;

            public Vector3AnimKey(Vector3 p_Start, Vector3 p_End, float p_Duration)
            {
                Start = p_Start;
                End = p_End;
                Duration = p_Duration;
                Exponents = new Vector3(1, 1, 1);
            }

            public Vector3AnimKey(Vector3 p_Start, Vector3 p_End, float p_Duration, Vector3 p_Exponents)
            {
                Start = p_Start;
                End = p_End;
                Duration = p_Duration;
                Exponents = p_Exponents;
            }
        }

        internal struct Vector3AnimData
        {
            internal List<Vector3AnimKey> Keys;
            internal Action<float> Callback;
            internal Action OnFinished;

            public Vector3AnimData(List<Vector3AnimKey> p_Keys, Action<float> p_Callback, Action p_OnFinished)
            {
                Keys = p_Keys;
                Callback = p_Callback;
                OnFinished = p_OnFinished;
            }
        }
    }
}