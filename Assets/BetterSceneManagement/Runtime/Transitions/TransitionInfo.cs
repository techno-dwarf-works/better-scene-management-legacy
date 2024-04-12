using System;
using Better.Commons.Runtime.Utility;
using Better.SceneManagement.Runtime.Sequences;

namespace Better.SceneManagement.Runtime.Transitions
{
    public abstract class TransitionInfo : IDisposable
    {
        protected bool Mutable { get; private set; }
        public bool OverridenSequence { get; private set; }
        public Type SequenceType { get; private set; }
        public bool AllowLogs { get; private set; }

        protected TransitionInfo(bool allowLogs)
        {
            Mutable = true;
            AllowLogs = allowLogs;
        }

        public abstract Sequence.OperationData[] CollectUnloadOperations();
        public abstract Sequence.OperationData[] CollectLoadOperations();

        protected void MakeImmutable()
        {
            Mutable = false;
        }

        protected void OverrideSequence<TSequence>()
            where TSequence : Sequence
        {
            if (!ValidateMutable())
            {
                return;
            }

            OverridenSequence = true;
            SequenceType = typeof(TSequence);
        }

        protected void OverrideAllowingLogs(bool value)
        {
            if (!ValidateMutable())
            {
                return;
            }

            AllowLogs = value;
        }

        protected bool ValidateMutable(bool logException = true)
        {
            if (!Mutable && logException)
            {
                var message = "Is immutable";
                DebugUtility.LogException<AccessViolationException>(message);
            }

            return Mutable;
        }

        public void Dispose()
        {
            MakeImmutable();
        }
    }
}