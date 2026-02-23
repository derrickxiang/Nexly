using Nexly.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Application.Interfaces;
public interface INewsRepository : IRepositoryBase<News>, IRepositoryBase2<News, Guid>
{
    Task<bool> ExistsByHashAsync(string hash);
}
