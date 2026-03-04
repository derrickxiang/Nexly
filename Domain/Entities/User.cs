using Nexly.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }
    public string Name { get; private set; }

    private User() { }

    public User(string email, string name)
    {
        Email = email;
        Name = name;
    }
}
