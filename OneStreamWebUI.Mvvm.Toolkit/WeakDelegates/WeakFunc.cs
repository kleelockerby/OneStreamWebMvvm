using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class WeakFunc<TResult>
    {
        private Func<TResult> staticFunc;

        protected MethodInfo Method { get; set; }

        public bool IsStatic { get { return staticFunc != null; } }

        public virtual string MethodName
        {
            get
            {
                if (staticFunc != null)
                {
                    return staticFunc.GetMethodInfo().Name;
                }
                return Method.Name;
            }
        }

        protected WeakReference FuncReference { get; set; }

        protected object LiveReference { get; set; }

        protected WeakReference Reference { get; set; }

        protected WeakFunc() { }

        public WeakFunc(Func<TResult> func, bool keepTargetAlive = false) : this(func == null ? null : func.Target, func, keepTargetAlive) { }

        public WeakFunc(object target, Func<TResult> func, bool keepTargetAlive = false)
        {
            if (func.GetMethodInfo().IsStatic)
            {
                staticFunc = func;

                if (target != null)
                {
                    Reference = new WeakReference(target);
                }
                return;
            }
            Method = func.GetMethodInfo();
            LiveReference = keepTargetAlive ? func.Target : null;
            Reference = new WeakReference(target);
        }

        public virtual bool IsAlive
        {
            get
            {
                if (staticFunc == null && Reference == null && LiveReference == null)
                {
                    return false;
                }

                if (staticFunc != null)
                {
                    if (Reference != null)
                    {
                        return Reference.IsAlive;
                    }
                    return true;
                }

                if (LiveReference != null)
                {
                    return true;
                }

                if (Reference != null)
                {
                    return Reference.IsAlive;
                }
                return false;
            }
        }

        public object Target
        {
            get
            {
                if (Reference == null)
                {
                    return null;
                }
                return Reference.Target;
            }
        }

        protected object FuncTarget
        {
            get
            {
                if (LiveReference != null)
                {
                    return LiveReference;
                }

                if (FuncReference == null)
                {
                    return null;
                }

                return FuncReference.Target;
            }
        }

        public TResult Execute()
        {
            if (staticFunc != null)
            {
                return staticFunc();
            }

            var funcTarget = FuncTarget;

            if (IsAlive)
            {
                if (Method != null && (LiveReference != null || FuncReference != null) && funcTarget != null)
                {
                    return (TResult)Method.Invoke(funcTarget, null);
                }
            }

            return default(TResult);
        }

        public void MarkForDeletion()
        {
            Reference = null;
            FuncReference = null;
            LiveReference = null;
            Method = null;
            staticFunc = null;
        }
    }
}
