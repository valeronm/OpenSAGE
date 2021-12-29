﻿using System.Numerics;

namespace OpenSage.Logic.AI.AIStates
{
    internal sealed class AttackState : State
    {
        private readonly AttackStateMachine _stateMachine;

        private bool _unknownBool;
        private Vector3 _unknownPosition;

        public AttackState()
        {
            _stateMachine = new AttackStateMachine();
        }

        internal override void Load(StatePersister reader)
        {
            reader.ReadVersion(1);

            reader.ReadBoolean(ref _unknownBool);
            reader.ReadVector3(ref _unknownPosition);

            _stateMachine.Load(reader);
        }
    }

    internal sealed class AttackStateMachine : StateMachineBase
    {
        public AttackStateMachine()
        {
            AddState(0, new AttackMoveTowardsTargetState());
            AddState(1, new AttackMoveTowardsTargetState());
            AddState(2, new AttackAimWeaponState());
            AddState(3, new AttackFireWeaponState());
        }

        internal override void Load(StatePersister reader)
        {
            reader.ReadVersion(1);

            base.Load(reader);
        }

        private sealed class AttackMoveTowardsTargetState : MoveTowardsState
        {
            private Vector3 _unknownPosition;
            private uint _unknownFrame;
            private bool _unknownBool1;
            private bool _unknownBool2;
            private bool _unknownBool3;
            private bool _unknownBool4;

            internal override void Load(StatePersister reader)
            {
                reader.ReadVersion(1);

                base.Load(reader);

                reader.ReadVector3(ref _unknownPosition);
                reader.ReadFrame(ref _unknownFrame);
                reader.ReadBoolean(ref _unknownBool1);
                reader.ReadBoolean(ref _unknownBool2);
                reader.ReadBoolean(ref _unknownBool3);
                reader.ReadBoolean(ref _unknownBool4);
            }
        }

        private sealed class AttackAimWeaponState : State
        {
            private bool _unknownBool1;
            private bool _unknownBool2;

            internal override void Load(StatePersister reader)
            {
                reader.ReadVersion(1);

                reader.ReadBoolean(ref _unknownBool1);
                reader.ReadBoolean(ref _unknownBool2);
            }
        }

        private sealed class AttackFireWeaponState : State
        {
            internal override void Load(StatePersister reader)
            {
                reader.ReadVersion(1);
            }
        }
    }
}
