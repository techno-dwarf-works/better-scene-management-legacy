using System;
using Better.Internal.Core.Runtime;
using Better.ProjectSettings.Runtime;
using Better.SceneManagement.Runtime.Sequences;
using Better.Singletons.Runtime.Attributes;

namespace Better.SceneManagement.Runtime
{
    [ScriptableCreate(PrefixConstants.BetterPrefix + "/" + nameof(SceneManagement))]
    public class SceneSystemSettings : ScriptableSettings<SceneSystemSettings>
    {
        public bool TryGetSequence(Type sequenceType, out Sequence sequence)
        {
            throw new NotImplementedException();
        }

        public bool TryGetSequence<TSequence>(out TSequence sequence)
            where TSequence : Sequence
        {
            var sequenceType = typeof(TSequence);
            if (TryGetSequence(sequenceType, out var derivedSequence)
                && derivedSequence is TSequence castedSequence)
            {
                sequence = castedSequence;
                return true;
            }

            sequence = default;
            return false;
        }
        
        public Sequence GetDefaultSequence()
        {
            throw new NotImplementedException();
        }

        public bool Validate()
        {
            return false;
        }
    }
}