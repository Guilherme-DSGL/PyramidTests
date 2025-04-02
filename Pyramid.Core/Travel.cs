namespace Pyramid.Core;

public class Travel
{
    public int Id { get; private set; }
    public int MaxSeatsCount { get; private set; }
    public List<Department> DepartmentRoute { get; private set; } = new List<Department>();
    private List<TravelSeat> _seats = new List<TravelSeat>();
    private List<Ticket> _tickets = new List<Ticket>();
    public IReadOnlyList<TravelSeat> Seats => _seats.AsReadOnly();
    public IReadOnlyList<Ticket> Tickets => _tickets.AsReadOnly();
    public DateTime TravelDate { get; private set; }

    public Travel(int id, int maxSeatsCount, List<Department> departmentRoute, DateTime travelDate, List<TravelSeat>? seats, List<Ticket>? ticket)
    {
        Id = id;
        MaxSeatsCount = maxSeatsCount;
        if (seats?.Count > maxSeatsCount) throw new ArgumentException("A quantidade de assentos ultrapassa o valor máximo de assentos da viagem");
        _seats = seats ?? [];
        _tickets = ticket ?? [];
        if (departmentRoute == null)  throw new ArgumentNullException(nameof(departmentRoute));
        if (departmentRoute.Count == 0) throw new ArgumentException("A lista de departamentos não pode ser vazia");
        DepartmentRoute = departmentRoute;
        TravelDate = travelDate;
    }

    public int ReservedSeats()
    {
        return Seats.Count(seat => !seat.IsSeatAvailable());
    }


    public List<TravelSeat> GetAllAvailableSeats(int startDeptId, int endDeptId)
    {
        var availableSeats = new List<TravelSeat>();
        int startLocation = GetBitmapLocationFromDepartmentRoute(startDeptId);
        int endLocation = GetBitmapLocationFromDepartmentRoute(endDeptId);

        for (int seatNumber = 1; seatNumber <= MaxSeatsCount; seatNumber++)
        {
            var existingSeat = _seats.FirstOrDefault(s => s.ArmchairNumber == seatNumber);

            if (existingSeat == null)
            {
                availableSeats.Add(new TravelSeat(
            id: _seats.Count + availableSeats.Count(s => !_seats.Contains(s)) + 1,
            DepartmentRoute,
            Id,
            seatNumber));
            } else
            {
                if (existingSeat.IsSeatAvailableFor(startLocation, endLocation))
                {
                    availableSeats.Add(existingSeat);
                }
            }
           
        }
        return availableSeats.OrderBy(s => s.ArmchairNumber).ToList();
    }

    public int GetBitmapLocationFromDepartmentRoute(int departmentId)
    {
        int index = DepartmentRoute.FindIndex(d => d.Id == departmentId);
        if (index == -1)
            throw new ArgumentException($"Departamento {departmentId} não encontrado na rota.");
        return index;
    }

    public int GetBitmapLocationFromDepartmentRoute(String departmentName)
    {
        int index = DepartmentRoute.FindIndex(d => d.Name == departmentName);
        if (index == -1)
            throw new ArgumentException($"Departamento {departmentName} não encontrado na rota.");
        return index;
    }
    public bool IsFull()
    {
        return Seats.All(seat => !seat.IsSeatAvailable());
    }

    public void ReserveSeat(Ticket ticket, TravelSeat seat)
    {
        if (!_seats.Contains(seat) && _seats.Count < MaxSeatsCount)
        {
            AddSeat(seat);
        }
            AddTicket(ticket);
    }

    public void AddSeat(TravelSeat seat)
    {
        if (Seats.Count == MaxSeatsCount)
        {
            throw new InvalidOperationException("Número máximo de assentos preenchidos");
        }
        if (Seats.Any(s => s.ArmchairNumber == seat.ArmchairNumber))
        {
            throw new InvalidOperationException("Número de assento já existe.");
        }
        _seats.Add(seat);
    }
    public void AddTicket(Ticket ticket)
    {
        var seat = _seats.FirstOrDefault(s => s.Id == ticket.SeatId);
        if (seat == null)
        {
            throw new InvalidOperationException("Assento não encontrado.");
        }
        int startLocation = GetBitmapLocationFromDepartmentRoute(ticket.StartDepartmentId);
        int endLocation = GetBitmapLocationFromDepartmentRoute(ticket.EndDepartmentId);
        if (!seat.IsSeatAvailableFor(startLocation, endLocation))
        {
            throw new InvalidOperationException("O assento já está ocupado para a rota do ticket.");
        }

        _tickets.Add(ticket);

        seat.UpdateBitmap(startLocation, endLocation);
    }
}
