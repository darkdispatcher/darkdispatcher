﻿using DarkDispatcher.Core.Events;
using DarkDispatcher.Domain.Projects.Ids;

namespace DarkDispatcher.Domain.Projects.Events.v1;

public record EnvironmentUpdated
  (ProjectId ProjectId, string Id, string Name, string Description, string Color) : DomainEvent;