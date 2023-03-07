using System;
using System.Linq;
using System.Reflection;

public static class EventUtility
{
    public static void UnsubscribeAll<T>(object obj)
    {
        // Get all fields of the object instance that are of the specified type.
        var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(f => f.FieldType.GetInterfaces().Contains(typeof(T)));

        foreach (var field in fields)
        {
            // Get the field value, which should be an object with events.
            var fieldValue = field.GetValue(obj);

            if (fieldValue != null)
            {
                // Get all the events of the field value object.
                var events = fieldValue.GetType().GetEvents();

                foreach (var evt in events)
                {
                    // Get the event handler and unsubscribe from it.
                    var handler = field.FieldType.GetField(evt.Name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(fieldValue) as MulticastDelegate;

                    if (handler != null)
                    {
                        foreach (var subscriber in handler.GetInvocationList())
                        {
                            evt.RemoveEventHandler(fieldValue, subscriber);
                        }
                    }
                }
            }
        }
    }
}
