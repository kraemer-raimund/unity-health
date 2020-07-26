/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using System;
using UnityEngine.Events;

namespace Rakrae.Unity.Health.Events
{
    [Serializable]
    public class HealthInitializedEvent : UnityEvent<HealthInitializedEventArgs>
    {
    }
}
