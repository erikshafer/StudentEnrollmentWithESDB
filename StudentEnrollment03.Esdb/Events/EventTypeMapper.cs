using System.Collections.Concurrent;

namespace StudentEnrollment03.Esdb.Events;

public class EventTypeMapper
{
    public static readonly EventTypeMapper Instance = new();
    
    private readonly ConcurrentDictionary<string, Type?> _typeMap = new();
    private readonly ConcurrentDictionary<Type, string> _typeNameMap = new();

    public string ToName<TEventType>() => ToName(typeof(TEventType));
    
    public string ToName(Type eventType) => _typeNameMap.GetOrAdd(eventType, _ =>
    {
        var eventTypeName = eventType.FullName!;
        _typeMap.TryAdd(eventTypeName, eventType);
        return eventTypeName;
    });
    
    public Type? ToType(string eventTypeName) => _typeMap.GetOrAdd(eventTypeName, _ =>
    {
        var type = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes().Where(x => x.FullName == eventTypeName || x.Name == eventTypeName))
            .FirstOrDefault();

        if (type == null)
            return null;

        _typeNameMap.TryAdd(type, eventTypeName);

        return type;
    });
}