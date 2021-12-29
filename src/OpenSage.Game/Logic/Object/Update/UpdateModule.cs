﻿namespace OpenSage.Logic.Object
{
    public abstract class UpdateModule : BehaviorModule
    {
        private uint _frameSomething;

        internal override void Load(StatePersister reader)
        {
            reader.ReadVersion(1);

            base.Load(reader);

            // Maybe some kind of frame timer? But sometimes it's -2.
            reader.ReadFrame(ref _frameSomething);
        }
    }

    public abstract class UpdateModuleData : ContainModuleData
    {
        public override ModuleKinds ModuleKinds => ModuleKinds.Update;
    }
}
