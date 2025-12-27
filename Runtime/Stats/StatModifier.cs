
using System;
using UnityUtils.Timer;

namespace Dave6.StatSystem.Stat
{
    /*
        effect와 다르게 이건 스텟에 종속된 클래스기 때문에 ScriptableObject로 만들면 안좋은
    */

    public enum StatValueType
    {
        Flat,               // Base 에 합하는 고정치 (+15 +25)
        Percent,            // 합연산 요소  (+15% +25%)
        finalMultiplier,    // 곱연산 요소  (x1.15 x1.25)
        //Override,           // 값 덮어쓰기
    }
    // SO로 만들어야할까?
    public class BaseContribution
    {
        public StatValueType valueType { get; private set;}
        public float magnitude { get; private set;}

        public BaseContribution(StatValueType valueType, float magnitude)
        {
            this.valueType = valueType;
            this.magnitude = magnitude;
        }
    }

    // dispose 패턴을 적용할까말까
    public class StatModifier
    {
        public object source { get; set; }
        public float magnitude { get; set; }
        public float duration { get; private set; }
        public StatValueType operationType { get; private set; }

        Countdown m_Timer;
        public event Action<StatModifier> onExpired;


        public StatModifier(object source, StatValueType operationType, float value, float duration = -1)
        {
            this.source = source;
            this.operationType = operationType;
            magnitude = value;

            // 유지시간이 있다면 타이머 설정
            this.duration = duration;

            if (duration >= 0)
            {
                m_Timer = new Countdown(duration);
                m_Timer.OnTimerStop += () => onExpired?.Invoke(this);
                m_Timer.Start();
            }
        }

        public override string ToString() => $"{operationType} {magnitude} frome {source}.";
    }
}
