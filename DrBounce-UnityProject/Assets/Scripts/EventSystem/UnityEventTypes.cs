using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class IntEvent : UnityEvent<int> { };
[Serializable] public class FloatEvent : UnityEvent<float> { };
[Serializable] public class StringEvent : UnityEvent<string> { };
[Serializable] public class BoolEvent : UnityEvent<bool> { };
[Serializable] public class Vector2Event : UnityEvent<Vector2> { };
[Serializable] public class Vector3Event : UnityEvent<Vector3> { };
[Serializable] public class GameObjectEvent : UnityEvent<GameObject> { };