using System;
using Better.Attributes.Runtime.Select;
using Better.Internal.Core.Runtime;
using Better.ProjectSettings.Runtime;
using Better.SceneManagement.Runtime.Sequences;
using Better.Singletons.Runtime.Attributes;
using UnityEngine;

namespace Better.SceneManagement.Runtime
{
    [ScriptableCreate(Path)]
    public class SceneSystemSettings : ScriptableSettings<SceneSystemSettings>
    {
        public const string Path = PrefixConstants.BetterPrefix + "/Scene Management";
        private readonly Sequence _fallbackSequence = new ParallelSequence();

        [Select]
        [SerializeReference] private Sequence _defaultSequence;

        [Select]
        [SerializeReference] private Sequence[] _overridenSequences;

        public bool TryGetOverridenSequence(Type sequenceType, out Sequence sequence)
        {
            for (var i = 0; i < _overridenSequences.Length; i++)
            {
                sequence = _overridenSequences[i];
                if (sequence.GetType() == sequenceType)
                {
                    return true;
                }
            }

            sequence = null;
            return false;
        }

        public Sequence GetDefaultSequence()
        {
            if (_defaultSequence == null)
            {
                var message = $"{nameof(_defaultSequence)} is null, returned {nameof(_fallbackSequence)}({_fallbackSequence})";
                Debug.LogWarning(message);

                return _fallbackSequence;
            }

            return _defaultSequence;
        }
    }
}