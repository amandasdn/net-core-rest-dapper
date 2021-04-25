using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Interfaces
{
    public interface IUser
    {
        string Name { get; }

        Guid GetUserId();

        string GetUserEmail();

        bool IsAuthenticated();
    }
}
