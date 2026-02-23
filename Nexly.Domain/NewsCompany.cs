using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Domain
{
    public class NewsCompany
    {
        public Guid Id { get; set; }

        public Guid NewsId { get; set; }

        public string CompanyName { get; set; } = default!;
    }
}
