using System;
using System.Collections.Generic;
using System.Linq;

public enum MessengerMode
{
	DONT_REQUIRE_LISTENER,
	REQUIRE_LISTENER,
}

internal class MessengerInternal
{
	public readonly Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
	public readonly MessengerMode DEFAULT_MODE = MessengerMode.DONT_REQUIRE_LISTENER;

	internal static MessengerInternal Instance
	{
		get
		{
			if (instance == null)
				instance = new MessengerInternal();
			return instance;
		}
	}
	private static MessengerInternal instance;

	private MessengerInternal()
    {
        this.eventTable = new Dictionary<string, Delegate>(); ;
    }

    public void AddListener(string eventType, Delegate callback)
	{
		OnListenerAdding(eventType, callback);
		eventTable[eventType] = Delegate.Combine(eventTable[eventType], callback);
	}

	 public void RemoveListener(string eventType, Delegate handler)
	{
		OnListenerRemoving(eventType, handler);
		eventTable[eventType] = Delegate.Remove(eventTable[eventType], handler);
		OnListenerRemoved(eventType);
	}

	 public T[] GetInvocationList<T>(string eventType)
	{
		Delegate d;
		if (eventTable.TryGetValue(eventType, out d))
		{
			try
			{
				return d.GetInvocationList().Cast<T>().ToArray();
			}
			catch
			{
				throw CreateBroadcastSignatureException(eventType);
			}
		}
		return new T[0];
	}

	 public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
	{
		if (!eventTable.ContainsKey(eventType))
		{
			eventTable.Add(eventType, null);
		}

		var d = eventTable[eventType];
		if (d != null && d.GetType() != listenerBeingAdded.GetType())
		{
			throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
		}
	}

	 public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
	{
		if (eventTable.ContainsKey(eventType))
		{
			var d = eventTable[eventType];

			if (d == null)
			{
				throw new ListenerException(string.Format("Attempting to remove listener with for event type {0} but current listener is null.", eventType));
			}
			else if (d.GetType() != listenerBeingRemoved.GetType())
			{
				throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
			}
		}
		else
		{
			throw new ListenerException(string.Format("Attempting to remove listener for type {0} but Messenger doesn't know about this event type.", eventType));
		}
	}

	 public void OnListenerRemoved(string eventType)
	{
		if (eventTable[eventType] == null)
		{
			eventTable.Remove(eventType);
		}
	}

	 public void OnBroadcasting(string eventType, MessengerMode mode)
	{
		if (mode == MessengerMode.REQUIRE_LISTENER && !eventTable.ContainsKey(eventType))
		{
			throw new BroadcastException(string.Format("Broadcasting message {0} but no listener found.", eventType));
		}
	}

	 public BroadcastException CreateBroadcastSignatureException(string eventType)
	{
		return new BroadcastException(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", eventType));
	}

	public class BroadcastException : Exception
	{
		public BroadcastException(string msg)
			: base(msg)
		{
		}
	}

	public class ListenerException : Exception
	{
		public ListenerException(string msg)
			: base(msg)
		{
		}
	}
}

// No parameters
public static class Messenger
{
	
	internal static MessengerInternal Instance { get {
			if (instance == null)
				instance = MessengerInternal.Instance;
			return instance;
		} }
	private static MessengerInternal instance;

	static public void AddListener(string eventType, Action handler)
	{
		Instance.AddListener(eventType, handler);
	}

	static public void AddListener<TReturn>(string eventType, Func<TReturn> handler)
	{
		Instance.AddListener(eventType, handler);
	}

	static public void RemoveListener(string eventType, Action handler)
	{
		Instance.RemoveListener(eventType, handler);
	}

	static public void RemoveListener<TReturn>(string eventType, Func<TReturn> handler)
	{
		Instance.RemoveListener(eventType, handler);
	}

	static public void Broadcast(string eventType)
	{
		Broadcast(eventType, Instance.DEFAULT_MODE);
	}

	static public void Broadcast<TReturn>(string eventType, Action<TReturn> returnCall)
	{
		Broadcast(eventType, returnCall, Instance.DEFAULT_MODE);
	}

	static public void Broadcast(string eventType, MessengerMode mode)
	{
		Instance.OnBroadcasting(eventType, mode);
		var invocationList = Instance.GetInvocationList<Action>(eventType);

		foreach (var callback in invocationList)
			callback.Invoke();
	}

	static public void Broadcast<TReturn>(string eventType, Action<TReturn> returnCall, MessengerMode mode)
	{
		Instance.OnBroadcasting(eventType, mode);
		var invocationList = Instance.GetInvocationList<Func<TReturn>>(eventType);

		foreach (var result in invocationList.Select(del => del.Invoke()).Cast<TReturn>())
		{
			returnCall.Invoke(result);
		}
	}
}

// One parameter
public class Messenger<T>
{

	internal static MessengerInternal Instance
	{
		get
		{
			if (instance == null)
				instance = MessengerInternal.Instance;
			return instance;
		}
	}
	private static MessengerInternal instance;

	static public void AddListener(string eventType, Action<T> handler)
	{
		Instance.AddListener(eventType, handler);
	}

	static public void AddListener<TReturn>(string eventType, Func<T, TReturn> handler)
	{
		Instance.AddListener(eventType, handler);
	}

	static public void RemoveListener(string eventType, Action<T> handler)
	{
		Instance.RemoveListener(eventType, handler);
	}

	static public void RemoveListener<TReturn>(string eventType, Func<T, TReturn> handler)
	{
		Instance.RemoveListener(eventType, handler);
	}

	static public void Broadcast(string eventType, T arg1)
	{
		Broadcast(eventType, arg1, Instance.DEFAULT_MODE);
	}

	static public void Broadcast<TReturn>(string eventType, T arg1, Action<TReturn> returnCall)
	{
		Broadcast(eventType, arg1, returnCall, Instance.DEFAULT_MODE);
	}

	static public void Broadcast(string eventType, T arg1, MessengerMode mode)
	{
		Instance.OnBroadcasting(eventType, mode);
		var invocationList = Instance.GetInvocationList<Action<T>>(eventType);

		foreach (var callback in invocationList)
			callback.Invoke(arg1);
	}

	static public void Broadcast<TReturn>(string eventType, T arg1, Action<TReturn> returnCall, MessengerMode mode)
	{
		Instance.OnBroadcasting(eventType, mode);
		var invocationList = Instance.GetInvocationList<Func<T, TReturn>>(eventType);

		foreach (var result in invocationList.Select(del => del.Invoke(arg1)).Cast<TReturn>())
		{
			returnCall.Invoke(result);
		}
	}
}


// Two parameters
static public class Messenger<T, U>
{
	internal static MessengerInternal Instance
	{
		get
		{
			if (instance == null)
				instance = MessengerInternal.Instance;
			return instance;
		}
	}
	private static MessengerInternal instance;
	static public void AddListener(string eventType, Action<T, U> handler)
	{
		Instance.AddListener(eventType, handler);
	}

	static public void AddListener<TReturn>(string eventType, Func<T, U, TReturn> handler)
	{
		Instance.AddListener(eventType, handler);
	}

	static public void RemoveListener(string eventType, Action<T, U> handler)
	{
		Instance.RemoveListener(eventType, handler);
	}

	static public void RemoveListener<TReturn>(string eventType, Func<T, U, TReturn> handler)
	{
		Instance.RemoveListener(eventType, handler);
	}

	static public void Broadcast(string eventType, T arg1, U arg2)
	{
		Broadcast(eventType, arg1, arg2, Instance.DEFAULT_MODE);
	}

	static public void Broadcast<TReturn>(string eventType, T arg1, U arg2, Action<TReturn> returnCall)
	{
		Broadcast(eventType, arg1, arg2, returnCall, Instance.DEFAULT_MODE);
	}

	static public void Broadcast(string eventType, T arg1, U arg2, MessengerMode mode)
	{
		Instance.OnBroadcasting(eventType, mode);
		var invocationList = Instance.GetInvocationList<Action<T, U>>(eventType);

		foreach (var callback in invocationList)
			callback.Invoke(arg1, arg2);
	}

	static public void Broadcast<TReturn>(string eventType, T arg1, U arg2, Action<TReturn> returnCall, MessengerMode mode)
	{
		Instance.OnBroadcasting(eventType, mode);
		var invocationList = Instance.GetInvocationList<Func<T, U, TReturn>>(eventType);

		foreach (var result in invocationList.Select(del => del.Invoke(arg1, arg2)).Cast<TReturn>())
		{
			returnCall.Invoke(result);
		}
	}
}


// Three parameters
static public class Messenger<T, U, V>
{
	internal static MessengerInternal Instance
	{
		get
		{
			if (instance == null)
				instance = MessengerInternal.Instance;
			return instance;
		}
	}
	private static MessengerInternal instance;
	static public void AddListener(string eventType, Action<T, U, V> handler)
	{
		Instance.AddListener(eventType, handler);
	}

	static public void AddListener<TReturn>(string eventType, Func<T, U, V, TReturn> handler)
	{
		Instance.AddListener(eventType, handler);
	}

	static public void RemoveListener(string eventType, Action<T, U, V> handler)
	{
		Instance.RemoveListener(eventType, handler);
	}

	static public void RemoveListener<TReturn>(string eventType, Func<T, U, V, TReturn> handler)
	{
		Instance.RemoveListener(eventType, handler);
	}

	static public void Broadcast(string eventType, T arg1, U arg2, V arg3)
	{
		Broadcast(eventType, arg1, arg2, arg3, Instance.DEFAULT_MODE);
	}

	static public void Broadcast<TReturn>(string eventType, T arg1, U arg2, V arg3, Action<TReturn> returnCall)
	{
		Broadcast(eventType, arg1, arg2, arg3, returnCall, Instance.DEFAULT_MODE);
	}

	static public void Broadcast(string eventType, T arg1, U arg2, V arg3, MessengerMode mode)
	{
		Instance.OnBroadcasting(eventType, mode);
		var invocationList = Instance.GetInvocationList<Action<T, U, V>>(eventType);

		foreach (var callback in invocationList)
			callback.Invoke(arg1, arg2, arg3);
	}

	static public void Broadcast<TReturn>(string eventType, T arg1, U arg2, V arg3, Action<TReturn> returnCall, MessengerMode mode)
	{
		Instance.OnBroadcasting(eventType, mode);
		var invocationList = Instance.GetInvocationList<Func<T, U, V, TReturn>>(eventType);

		foreach (var result in invocationList.Select(del => del.Invoke(arg1, arg2, arg3)).Cast<TReturn>())
		{
			returnCall.Invoke(result);
		}
	}
}