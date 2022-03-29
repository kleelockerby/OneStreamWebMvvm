using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class WeakAction<TItem> : WeakAction, IExecuteWithObject
    {
        private Action<TItem>? staticAction;

        public override string MethodName
        {
            get
            {
                if (staticAction != null)
                {
                    return staticAction.GetMethodInfo().Name;
                }
                return Method.Name;
            }
        }

        public override bool IsAlive
        {
            get
            {
                if (staticAction == null && Reference == null)
                {
                    return false;
                }

                if (staticAction != null)
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

        public WeakAction(Action<TItem> action, bool keepTargetAlive = false) : this(action == null ? null : action.Target, action, keepTargetAlive) { }

        public WeakAction(object? target, Action<TItem> action, bool keepTargetAlive = false)
        {
            if (action.GetMethodInfo().IsStatic)
            {
                staticAction = action;
                if (target != null)
                {
                    Reference = new WeakReference(target);
                }
                return;
            }
            Method = action.GetMethodInfo();
        }

        public new void Execute()
        {
            Execute(default(TItem)!);
        }

        public void Execute(TItem parameter)
        {
            if (staticAction != null)
            {
                staticAction(parameter);
                return;
            }

            var actionTarget = ActionTarget;

            if (IsAlive)
            {
                if (Method != null && (LiveReference != null || ActionReference != null) && actionTarget != null)
                {
                    Method.Invoke( actionTarget, new object[] { parameter });
                }
            }
        }

        public void ExecuteWithObject(object parameter)
        {
            var parameterCasted = (TItem)parameter;
            Execute(parameterCasted);
        }

        public new void MarkForDeletion()
        {
            staticAction = null!;
            base.MarkForDeletion();
        }
    }
}
