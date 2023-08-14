using controle_de_estacionamento.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Models;

namespace controle_de_estacionamento.Repositories;

public class ParkingRepository : BaseRepository<Parking>, IParkingRepository
{
    public ParkingRepository(Context context) : base(context)
    {
    }
}