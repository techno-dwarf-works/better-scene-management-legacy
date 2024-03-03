using System;
using Better.Internal.Core.Runtime;
using Better.ProjectSettings.Runtime;
using Better.SceneManagement.Runtime.Sequences;
using Better.Singletons.Runtime.Attributes;
using UnityEngine;

namespace Better.SceneManagement.Runtime
{
    [ScriptableCreate(PrefixConstants.BetterPrefix + "/" + nameof(SceneManagement))]
    public class SceneSystemSettings : ScriptableSettings<SceneSystemSettings>
    {
        [SerializeReference] private Sequence[] _sequences;
        [SerializeReference] private Sequence _defaultSequence;

        public bool TryGetSequence(Type sequenceType, out Sequence sequence)
        {
            for (var i = 0; i < _sequences.Length; i++)
            {
                sequence = _sequences[i];
                if (sequence.GetType() == sequenceType)
                {
                    return true;
                }
            }

            sequence = null;
            return false;
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

            sequence = null;
            return false;
        }

        public Sequence GetDefaultSequence()
        {
            return _defaultSequence;
        }
    }
}