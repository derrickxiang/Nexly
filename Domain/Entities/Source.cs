using Nexly.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Domain.Entities;

public class Source : Entity
{
    public string Name { get; private set; }
    public string BaseUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Source() { }

    public Source(string name, string baseUrl)
    {
        Name = name;
        BaseUrl = baseUrl;
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkUpdated();
    }
}
