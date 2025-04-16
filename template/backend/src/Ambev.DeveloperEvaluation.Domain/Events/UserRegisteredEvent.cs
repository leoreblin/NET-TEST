using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class UserRegisteredEvent : IEvent
    {
        public User User { get; }

        public UserRegisteredEvent(User user)
        {
            User = user;
        }
    }
}
