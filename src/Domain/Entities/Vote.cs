﻿using Domain.Abstractions;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Vote : BaseEntity
{
    public VoteType Type { get; set; }

    public VotableContentType ContentType { get; set; }

    public User? User { get; set; }

    public Guid UserId { get; set; }

    public IVotableContent? Content { get; set; }

    public Guid ContentId { get; set; }
}