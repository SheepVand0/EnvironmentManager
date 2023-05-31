using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core
{
    internal struct AnimationKey
    {
        internal AnimationKey(float p_Time, float p_Key)
        {
            Time = p_Time; 
            Key = p_Key;
        }

        internal float Time;
        internal float Key;

    }

    internal class AnimationCurve
    {
        protected List<AnimationKey> m_Keys = new List<AnimationKey>();

        internal void AddKey(AnimationKey p_Key)
        {
            if (p_Key.Time <= m_Keys.First().Time)
            {
                m_Keys.Insert(0, p_Key);
            } else if (p_Key.Time >= m_Keys.Last().Time) {
                m_Keys.Add(p_Key);
            } else
            {
                for (int l_i = 0; l_i < m_Keys.Count - 1; l_i++)
                {
                    if (p_Key.Time < m_Keys[l_i + 1].Time && p_Key.Time > m_Keys[l_i].Time)
                    {
                        m_Keys.Insert(l_i + 1, p_Key);
                        break;
                    }
                }
            }
        }

        internal float Duration()
        {
            return m_Keys.Last().Time;
        }

        internal List<AnimationKey> GetKeys()
        {
            List<AnimationKey> l_Keys = new List<AnimationKey>();
            l_Keys.AddRange(m_Keys);
            return l_Keys;
        }
    }

    internal class Animation : MonoBehaviour
    {
        internal event Action<float> OnUpdate;
        internal event Action OnFinished;

        protected bool m_IsPlaying;

        protected float m_StartTime;
        protected float m_Duration;

        protected bool m_HasTempCurve;
        protected AnimationCurve m_Curve;
        protected AnimationCurve m_TempCurve;

        internal float CurrentValue { get; private set; }

        internal void Update()
        {
            float l_Time = Time.realtimeSinceStartup - m_StartTime;

            AnimationKey l_CurrentKey = new AnimationKey(0, 0);
            AnimationKey l_NextKey = new AnimationKey(0, 0);
            List<AnimationKey> l_Keys = m_Curve.GetKeys();
            for (int l_i = 0; l_i < l_Keys.Count(); l_i++)
            {
                if (l_i == l_Keys.Count() - 1)
                {
                    Stop();
                    break;
                }

                if (l_Keys[l_i].Time >= l_Time && l_Time <= l_Keys[l_i + 1].Time)
                {
                    l_CurrentKey = l_Keys[l_i];
                    l_NextKey = l_Keys[l_i + 1];
                }

                float l_TimeBetweenKeys = l_Time - l_CurrentKey.Time;

                float l_Prct = l_TimeBetweenKeys / l_NextKey.Time;

                float l_Value = l_CurrentKey.Key + ((l_NextKey.Key - l_CurrentKey.Key) * l_Prct);
                CurrentValue = l_Value;
                OnUpdate?.Invoke(l_Value);
            }

        }

        internal void SetCurve(AnimationCurve p_Curve)
        {
            if (m_IsPlaying)
            {
                m_TempCurve = p_Curve;
                m_HasTempCurve = true;
            } else
            {
                m_Curve = p_Curve;
                m_HasTempCurve = false;
            }
        }

        internal void Play()
        {
            m_StartTime = Time.realtimeSinceStartup;
            m_Duration = m_Curve.Duration();
            m_IsPlaying = true;
        }

        internal void Stop() {
            m_IsPlaying = false;
            OnFinished?.Invoke();
            if (m_HasTempCurve)
            {
                m_Curve = m_TempCurve;
                m_HasTempCurve = false;
            }
        }
    }

    internal class Vector3Animation : MonoBehaviour
    {
        Vector3 CurrentValue = new Vector3();

        internal Action<Vector3> OnUpdate;
        internal Action OnFinish;

        internal Animation XAnim;
        internal Animation YAnim;
        internal Animation ZAnim;

        protected int m_FinishedAnimCount;

        protected void Awake()
        {
            XAnim = gameObject.AddComponent<Animation>();
            YAnim = gameObject.AddComponent<Animation>();
            ZAnim = gameObject.AddComponent<Animation>();

            XAnim.OnUpdate += (p_Value) => CurrentValue.x = p_Value;
            YAnim.OnUpdate += (p_Value) => CurrentValue.y = p_Value;
            ZAnim.OnUpdate += (p_Value) => CurrentValue.z = p_Value;

            ForeachAnims((x) => x.OnFinished += () => m_FinishedAnimCount += 1);
        }

        protected void Update()
        {
            OnUpdate.Invoke(CurrentValue);
        }

        internal void ForeachAnims(Action<Animation> p_Action)
        {
            p_Action(XAnim);
            p_Action(YAnim);
            p_Action(ZAnim);
        }

        internal void Play()
        {
            ForeachAnims((x) => x.Play());
        }

        internal void CheckFinishedAnimCount()
        {
            if (m_FinishedAnimCount == 3)
                OnFinish.Invoke();
        }

        internal void Stop() {
            ForeachAnims((x) => x.Stop());
        }

        internal void SetCurves(AnimationCurve p_XCurve, AnimationCurve p_YCurve, AnimationCurve p_ZCurve)
        {
            XAnim.SetCurve(p_XCurve);
            YAnim.SetCurve(p_YCurve);
            ZAnim.SetCurve(p_ZCurve);
        }
    }
}
