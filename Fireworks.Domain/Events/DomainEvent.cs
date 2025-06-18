using MediatR;

namespace Fireworks.Domain.Events;

public abstract record DomainEvent : IDomainEvent, INotification
{
    public DateTime RaiseOn { get; protected set; } = DateTime.UtcNow;
}