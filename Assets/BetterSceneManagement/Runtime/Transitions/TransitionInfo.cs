using System;
using Better.Extensions.Runtime;
using Better.SceneManagement.Runtime.Sequences;

namespace Better.SceneManagement.Runtime.Transitions
{
    public abstract class TransitionInfo : IDisposable
    {
        protected bool Mutable { get; private set; }
        public bool SequenceOverriden { get; protected set; }
        public Type SequenceType { get; protected set; }

        protected TransitionInfo()
        {
            Mutable = true;
        }

        public abstract Sequence.OperationData[] CollectFromOperations();
        public abstract Sequence.OperationData[] CollectToOperations();

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

            SequenceOverriden = true;
            SequenceType = typeof(TSequence);
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