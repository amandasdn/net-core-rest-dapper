using System;

namespace Project.Domain
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public bool Active { get; set; } = true;

        public bool Removed { get; set; } = false;
    }
}
