using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Xlune.Animation
{
    class XTween
    {
        //Easing Type
        public enum Easing
        {
            EaseNoneIn,
            EaseNoneOut,
            EaseNoneInOut,
            EaseSineIn,
            EaseSineOut,
            EaseSineInOut,
            EaseCircleIn,
            EaseCircleOut,
            EaseCircleInOut,
            EaseQuadraticIn,
            EaseQuadraticOut,
            EaseQuadraticInOut,
            EaseCubicIn,
            EaseCubicOut,
            EaseCubicInOut,
            EaseQuarticIn,
            EaseQuarticOut,
            EaseQuarticInOut,
            EaseQuinticIn,
            EaseQuinticOut,
            EaseQuinticInOut,
            EaseBackIn,
            EaseBackOut,
            EaseBackInOut,
            EaseBounceIn,
            EaseBounceOut,
            EaseBounceInOut,
            EaseElasticIn,
            EaseElasticOut,
            EaseElasticInOut,
            EaseExpoIn,
            EaseExpoOut,
            EaseExpoInOut,
            EasePowerIn,
            EasePowerOut,
            EasePowerInOut
        };

        private enum GroupType
        {
            Serial,
            Parallel
        };
        private DispatcherTimer timer_;
        private object target_;
        private string[] properties_;
        private double from_ = 0;
        private bool useCurrentFrom_ = false;
        private bool isSetCurrentFrom_ = false;
        private double to_ = 0;
        private double duration_;
        private EasingFunctionBase easeFunction_;
        private double delay_ = 0;

        private double currentTime_ = 0;
        private XTween[] children_;
        private GroupType groupType_ = GroupType.Parallel;
        private long preTime_;
        private bool isPlaying_ = false;
        private bool isComplete_ = false;

        private event EventHandler onStarted_;
        private event EventHandler onPaused_;
        private event EventHandler onResumed_;
        private event EventHandler onStopped_;
        private event EventHandler onCompleted_;

        /*================================================
         * Constructor
         *===============================================*/
        //Tween Constructor
        public XTween(object target, string property=null, double from=0, double to=0, double duration=0, Easing easing = Easing.EaseNoneInOut, double delay = 0)
        {
            target_ = target;
            SetProperties(property);
            SetFrom(from);
            SetTo(to);
            SetDuration(duration);
            SetEasingFunction(easing);
            SetDelay(delay);
        }
        //Tween Constructor Not Set FromValue
        public XTween(object target, string property = null, double to = 0, double duration = 0, Easing easing = Easing.EaseNoneInOut, double delay = 0)
        {
            target_ = target;
            SetProperties(property);
            ClearFrom();
            SetTo(to);
            SetDuration(duration);
            SetEasingFunction(easing);
            SetDelay(delay);
        }
        //Group Constructor
        private XTween(params XTween[] args)
        {
            children_ = args;
        }
        //Serial Group
        public static XTween SerialTweens(params XTween[] args)
        {
            XTween t = new XTween(args);
            t.groupType_ = GroupType.Serial;
            return t;
        }
        //Serial Group By List
        public static XTween SerialTweensWithList(List<XTween> list)
        {
            XTween t = new XTween(list.ToArray());
            t.groupType_ = GroupType.Serial;
            return t;
        }
        //Parallel Group
        public static XTween ParallelTweens(params XTween[] args)
        {
            XTween t = new XTween(args);
            t.groupType_ = GroupType.Parallel;
            return t;
        }
        //Parallel Group By List
        public static XTween ParallelTweensWithList(List<XTween> list)
        {
            XTween t = new XTween(list.ToArray());
            t.groupType_ = GroupType.Parallel;
            return t;
        }

        /*================================================
         * Tween Event Accessor
         *===============================================*/
        //Started
        public event EventHandler OnStarted
        {
            add
            {
                onStarted_ += new EventHandler(value);
            }
            remove
            {
                onStarted_ -= new EventHandler(value);
            }
        }
        //Paused
        public event EventHandler OnPaused
        {
            add
            {
                onPaused_ += new EventHandler(value);
            }
            remove
            {
                onPaused_ -= new EventHandler(value);
            }
        }
        //Resumed
        public event EventHandler OnResumed
        {
            add
            {
                onResumed_ += new EventHandler(value);
            }
            remove
            {
                onResumed_ -= new EventHandler(value);
            }
        }
        //Stopped
        public event EventHandler OnStopped
        {
            add
            {
                onStopped_ += new EventHandler(value);
            }
            remove
            {
                onStopped_ -= new EventHandler(value);
            }
        }
        //Completed
        public event EventHandler OnCompleted
        {
            add
            {
                onCompleted_ += new EventHandler(value);
            }
            remove
            {
                onCompleted_ -= new EventHandler(value);
            }
        }

        /*================================================
         * Tween Setting Methods
         *===============================================*/
        //Properties
        public XTween SetProperties(params string[] args)
        {
            if (target_ != null)
            {
                properties_ = args;
            }
            return this;
        }
        //From
        public XTween SetFrom(double from)
        {
            if (target_ != null)
            {
                useCurrentFrom_ = false;
                isSetCurrentFrom_ = false;
                from_ = from;
            }
            return this;
        }
        //Clear From Value
        public XTween ClearFrom()
        {
            if (target_ != null)
            {
                useCurrentFrom_ = true;
                isSetCurrentFrom_ = false;
                from_ = 0;
            }
            return this;
        }
        //To
        public XTween SetTo(double to)
        {
            if (target_ != null)
            {
                to_ = to;
            }
            return this;
        }
        //Duration
        public XTween SetDuration(double duration)
        {
            if (target_ != null)
            {
                duration_ = duration * 1000;
            }
            return this;
        }
        //Easing
        public XTween SetEasingFunction(Easing easing)
        {
            if (target_ != null)
            {
                easeFunction_ = GetEasingFunction(easing);
            }
            return this;
        }
        //Easing(Origin)
        public XTween SetEasingFunction(EasingFunctionBase easing)
        {
            if (target_ != null && easing != null)
            {
                easeFunction_ = easing;
            }
            return this;
        }
        //Delay 
        public XTween SetDelay(double d)
        {
            delay_ = d * 1000;
            return this;
        }
        //OnStarted
        public XTween SetStarted(Action<object, EventArgs> func)
        {
            this.OnStarted += (sender, e) =>
            {
                func(sender, e);
            };
            return this;
        }
        //OnPaused
        public XTween SetPaused(Action<object, EventArgs> func)
        {
            this.OnPaused += (sender, e) =>
            {
                func(sender, e);
            };
            return this;
        }
        //OnResumed
        public XTween SetResumed(Action<object, EventArgs> func)
        {
            this.OnResumed += (sender, e) =>
            {
                func(sender, e);
            };
            return this;
        }
        //OnStopped
        public XTween SetStopped(Action<object, EventArgs> func)
        {
            this.OnStopped += (sender, e) =>
            {
                func(sender, e);
            };
            return this;
        }
        //OnComplete
        public XTween SetComplete(Action<object, EventArgs> func)
        {
            this.OnCompleted += (sender, e) =>
            {
                func(sender, e);
            };
            return this;
        }

        /*================================================
         * Tween Action Methods
         *===============================================*/
        //Tween Play
        public void Play()
        {
            if (timer_ == null)
            {
                timer_ = new DispatcherTimer();
                timer_.Interval = TimeSpan.FromMilliseconds(1);
                timer_.Tick += TimerCycleEngine;
            }
            if (!timer_.IsEnabled)
            {
                ResetAllChildTime(this);
                preTime_ = DateTime.Now.Ticks;
                timer_.Start();
                isPlaying_ = true;
                EventHandler handler = onStarted_;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        //Tween Pause
        public void Pause()
        {
            if (timer_ != null && timer_.IsEnabled  && isPlaying_)
            {
                timer_.Stop();

                EventHandler handler = onPaused_;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        //Tween Resume
        public void Resume()
        {
            if (timer_ != null && !timer_.IsEnabled && isPlaying_)
            {
                preTime_ = DateTime.Now.Ticks;
                timer_.Start();

                EventHandler handler = onResumed_;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        //Tween Stop
        public void Stop()
        {
            if (timer_ != null && isPlaying_)
            {
                timer_.Stop();
                isPlaying_ = false;
                ResetAllChildTime(this);
                EventHandler handler = onStopped_;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        //Check Playing
        public bool IsPlaying()
        {
            if (timer_ != null)
            {
                return timer_.IsEnabled;
            }
            return false;
        }

        /*================================================
         * Tween Private Methods
         *===============================================*/
        //Tween finish flag
        private bool IsFinish()
        {
            return  currentTime_ >= duration_ + delay_;
        }
        //All Tween finish flag
        private bool IsAllChildFinish(XTween tween)
        {
            bool isAllFinish = true;
            if (tween.children_ != null && tween.children_.Length > 0)
            {
                foreach(XTween child in tween.children_)
                {
                    if (!IsAllChildFinish(child))
                    {
                        isAllFinish = false;
                        break;
                    }
                }
            }
            else
            {
                isAllFinish = tween.IsFinish();
            }
            return isAllFinish;
        }
        //clear current time
        private void ResetAllChildTime(XTween tween)
        {
            if (tween.children_ != null && tween.children_.Length > 0)
            {
                foreach (XTween child in tween.children_)
                {
                    ResetAllChildTime(child);
                }
            }
            tween.isSetCurrentFrom_ = false;
            tween.isComplete_ = false;
            tween.currentTime_ = 0;
        }
        //Add Time
        private void AddTime(double delta)
        {
            if(useCurrentFrom_ && !isSetCurrentFrom_){
                isSetCurrentFrom_ = true;
                from_ = 0;
                if (properties_.Length > 0)
                {
                    Type t = target_.GetType();
                    PropertyInfo p = t.GetRuntimeProperty(properties_[0]);
                    if(p != null){
                        from_ = Convert.ToDouble(p.GetValue(target_));
                    }
                }
            }

            currentTime_ += delta;
            currentTime_ = Math.Min(duration_ + delay_, currentTime_);
            if(currentTime_ > delay_){
                double diffSize = to_ - from_;
                double timeScale = Math.Min(Math.Max((currentTime_ - delay_) / duration_, 0), 1);
                double currentValue;
                if (easeFunction_ == null)
                {
                    currentValue = timeScale * diffSize + from_;
                }
                else
                {
                    currentValue = easeFunction_.Ease(timeScale) * diffSize + from_;
                }
                
                Type t = target_.GetType();
                foreach (string property in properties_)
                {
                    PropertyInfo p = t.GetRuntimeProperty(property);
                    if(p != null && p.CanWrite){
                        p.SetValue(target_, currentValue);
                    }
                }
            }
        }
        //CycleEngine
        private void TimerCycleEngine(object sender, object e)
        {
            long now_ = DateTime.Now.Ticks;
            long delta = (now_ - preTime_) / 10000;
            preTime_ = now_;
            TweenAllPlay(this, delta);
        }
        //Recursive processing
        private void TweenAllPlay(XTween tween, double delta)
        {
            if (tween.children_ != null && tween.children_.Length > 0)
            {
                if (tween.IsFinish())
                {
                    for (int i = 0; i < tween.children_.Length; ++i)
                    {
                        XTween child = tween.children_[i];
                        TweenAllPlay(child, delta);
                        if (!IsAllChildFinish(child) && tween.groupType_ == GroupType.Serial)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    tween.AddTime(delta);
                }
            }
            else
            {
                if (!tween.IsFinish())
                {
                    tween.AddTime(delta);
                }
            }
            //Check Complete
            if (!tween.isComplete_ && IsAllChildFinish(tween))
            {
                tween.isComplete_ = true;
                tween.Stop();
                EventHandler handler = tween.onCompleted_;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        //Get Easing Function
        private EasingFunctionBase GetEasingFunction(Easing type)
        {
            EasingFunctionBase func = null;
            switch (type)
            {
                case Easing.EaseSineIn:
                case Easing.EaseSineOut:
                case Easing.EaseSineInOut:
                    func = new SineEase();
                    break;
                case Easing.EaseCircleIn:
                case Easing.EaseCircleOut:
                case Easing.EaseCircleInOut:
                    func = new CircleEase();
                    break;
                case Easing.EaseQuadraticIn:
                case Easing.EaseQuadraticOut:
                case Easing.EaseQuadraticInOut:
                    func = new QuadraticEase();
                    break;
                case Easing.EaseCubicIn:
                case Easing.EaseCubicOut:
                case Easing.EaseCubicInOut:
                    func = new CubicEase();
                    break;
                case Easing.EaseQuarticIn:
                case Easing.EaseQuarticOut:
                case Easing.EaseQuarticInOut:
                    func = new QuarticEase();
                    break;
                case Easing.EaseQuinticIn:
                case Easing.EaseQuinticOut:
                case Easing.EaseQuinticInOut:
                    func = new QuinticEase();
                    break;
                case Easing.EaseBackIn:
                case Easing.EaseBackOut:
                case Easing.EaseBackInOut:
                    func = new BackEase();
                    break;
                case Easing.EaseBounceIn:
                case Easing.EaseBounceOut:
                case Easing.EaseBounceInOut:
                    func = new BounceEase();
                    break;
                case Easing.EaseElasticIn:
                case Easing.EaseElasticOut:
                case Easing.EaseElasticInOut:
                    func = new ElasticEase();
                    break;
                case Easing.EaseExpoIn:
                case Easing.EaseExpoOut:
                case Easing.EaseExpoInOut:
                    func = new ExponentialEase();
                    break;
                case Easing.EasePowerIn:
                case Easing.EasePowerOut:
                case Easing.EasePowerInOut:
                    func = new PowerEase();
                    break;
                default:
                    break;
            }
            if (func != null)
            {
                switch ((int)type % 3)
                {
                    case 0:
                        func.EasingMode = EasingMode.EaseIn;
                        break;
                    case 1:
                        func.EasingMode = EasingMode.EaseOut;
                        break;
                    default:
                        func.EasingMode = EasingMode.EaseInOut;
                        break;
                }
            }
            return func;
        }
    }
}
