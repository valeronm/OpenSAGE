﻿using OpenSage.Logic.Object;

namespace OpenSage.Tests.Logic.Object.Update;

public abstract class UpdateModuleTest<TModule, TData> : BehaviorModuleTest<TModule, TData> where TModule : UpdateModule where TData : BehaviorModuleData, new()
{
    private readonly byte[] _nextUpdateFrame;

    protected UpdateModuleTest()
    {
        _nextUpdateFrame = [(byte)UpdateOrder, 0x00, 0x00, 0x00];
    }

    protected override byte[] ModuleData() => [V1, .. base.ModuleData(), .. _nextUpdateFrame];

    protected virtual UpdateOrder UpdateOrder => UpdateOrder.Order2;
}
