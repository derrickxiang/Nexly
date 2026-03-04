using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Application.DTOs;

public record ArticleDto(
    Guid Id,
    string Title,
    string Summary,
    string SourceName,
    DateTime CreatedAt);