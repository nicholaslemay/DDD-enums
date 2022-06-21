using System.Collections.Generic;

namespace DDDEnums.Support;

public static class EventDispatcher
{
    public static List<string> ReceivedEvents = new();

    public static void Dispatch(string eventName) => ReceivedEvents.Add(eventName);
}