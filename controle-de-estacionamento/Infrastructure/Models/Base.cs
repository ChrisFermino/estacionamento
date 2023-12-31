﻿using Infrastructure.Models.Interfaces;

namespace Infrastructure.Models;

public abstract class Base : IBase
{
    public long Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }
}

