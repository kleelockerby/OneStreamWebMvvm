using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class WeakAction
    {
        private Action? staticAction;

        protected MethodInfo? Method { get; set; }

        public virtual string MethodName
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

        protected WeakReference? ActionReference { get; set; }

        protected object? LiveReference { get; set; }

        protected WeakReference? Reference { get; set; }

        public bool IsStatic { get { return staticAction != null; } }

        protected WeakAction() { }
        public WeakAction(Action action, bool keepTargetAlive = false) : this(action == null ? null : action.Target, action, keepTargetAlive) { }
        public WeakAction(object? target, Action action, bool? keepTargetAlive = false)
        {
            //Method should fail with an exception if action is null
            if (action.GetMethodInfo().IsStatic)
            {
                staticAction = action;
                if (target != null)
                {
                    Reference = new WeakReference(target);
                }
                return;
            }
            Method = action?.GetMethodInfo();
        }

        public virtual bool IsAlive
        {
            get
            {
                if (staticAction == null && Reference == null && LiveReference == null)
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

        protected object ActionTarget
        {
            get
            {
                if (LiveReference != null)
                {
                    return LiveReference;
                }

                if (ActionReference == null)
                {
                    return null;
                }

                return ActionReference.Target;
            }
        }

        public void Execute()
        {
            if (staticAction != null)
            {
                staticAction();
                return;
            }

            var actionTarget = ActionTarget;

            if (IsAlive)
            {
                if (Method != null && (LiveReference != null || ActionReference != null) && actionTarget != null)
                {
                    Method.Invoke(actionTarget, null);
                    return;
                }
            }
        }

        public void MarkForDeletion()
        {
            Reference = null;
            ActionReference = null;
            LiveReference = null;
            Method = null;
            staticAction = null;
        }
    }
}
