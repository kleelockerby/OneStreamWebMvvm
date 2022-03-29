using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class WeakFunc<TItem, TResult> : WeakFunc<TResult>, IExecuteWithObjectAndResult
    {
        private Func<TItem, TResult>? staticFunc;

        public override string MethodName
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

        public override bool IsAlive
        {
            get
            {
                if (staticFunc == null && Reference == null)
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

                return Reference.IsAlive;
            }
        }

        public WeakFunc(Func<TItem, TResult> func, bool keepTargetAlive = false) : this(func == null ? null : func.Target, func, keepTargetAlive) { }
        
        public WeakFunc(object target, Func<TItem, TResult> func, bool keepTargetAlive = false)
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
        }

        public new TResult Execute()
        {
            return Execute(default(TItem));
        }

        public TResult Execute(TItem? parameter)
        {
            if (staticFunc != null)
            {
                return staticFunc(parameter);
            }

            var funcTarget = FuncTarget;

            if (IsAlive)
            {
                if (Method != null && (LiveReference != null || FuncReference != null) && funcTarget != null)
                {
                    return (TResult)Method.Invoke(funcTarget, new object[] { parameter! })!;
                }
            }
            return default(TResult)!;
        }

        public object ExecuteWithObject(object? parameter)
        {
            var parameterCasted = (TItem?)parameter;
            return Execute(parameterCasted)!;
        }

        public new void MarkForDeletion()
        {
            staticFunc = null;
            base.MarkForDeletion();
        }
    }
}
