namespace Pyramid.Core;



public class Ticket
{
    public int Id { get;  }
    public int SeatId { get; }
    public int StartDepartmentId { get;  }
    public int EndDepartmentId { get; }
    public int TravelId { get;  }
    public decimal Value { get; set; }

    public Ticket(int id, int seatId, int travelId,  int startDepartmentId, int endDepartmentId)
    {
        Id = id;
        SeatId = seatId;
        TravelId = travelId;
        StartDepartmentId = startDepartmentId;
        EndDepartmentId = endDepartmentId;
    }
}
