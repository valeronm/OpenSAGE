﻿namespace OpenSage.Logic.Object
{
    public sealed class Animation
    {
        public readonly AnimationTemplate Template;

        private ushort _currentImageIndex;
        private uint _lastUpdatedFrame;

        private ushort _unknown;

        private ushort _lastImageIndex;
        private uint _animationDelayFrames;

        public Animation(AnimationTemplate template)
        {
            Template = template;
        }

        internal void Load(StatePersister reader)
        {
            reader.ReadVersion(1);

            reader.ReadUInt16(ref _currentImageIndex);
            reader.ReadFrame(ref _lastUpdatedFrame);
            reader.ReadUInt16(ref _unknown);

            reader.SkipUnknownBytes(1);

            reader.ReadUInt16(ref _lastImageIndex);
            reader.ReadUInt32(ref _animationDelayFrames);

            var unknownFloat = 1.0f;
            reader.ReadSingle(ref unknownFloat);
            if (unknownFloat != 1.0f)
            {
                throw new InvalidStateException();
            }
        }
    }
}
