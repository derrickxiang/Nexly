using Nexly.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Domain.Entities;

public class NewsCluster : Entity
{
    public string Topic { get; private set; }
    public string Description { get; private set; }

    public NewsCluster(string topic, string description)
    {
        Topic = topic;
        Description = description;
    }
}