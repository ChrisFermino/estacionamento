﻿using controle_de_estacionamento.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Models;

namespace controle_de_estacionamento.Repositories;

public class PriceRepository : BaseRepository<Price>, IPriceRepository
{
    public PriceRepository(Context context) : base(context)
    {
    }
}
