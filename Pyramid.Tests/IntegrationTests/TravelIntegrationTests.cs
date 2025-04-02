using Pyramid.Core;
namespace Pyramid.Tests.IntegrationTests;

public class TravelFixture {
    public static List<Department> getDepartments()
    {
        List<Department> departments = [new Department(id: 1, name: "Crateús"), new Department(id: 2, name: "Nova Russas"), new Department(id: 3, name: "Fortaleza"),];

        return departments;
    }
}

public class TravelIntegrationTests
{
    [Fact]
    public void Seats_ShouldHaveBitmapSizeEqualToDepartmentRouteCount()
    {
        var travel = new Travel(1, 10, TravelFixture.getDepartments(), DateTime.Now, null, null);

        var seat1 = new TravelSeat(1, TravelFixture.getDepartments(), 1, 1);
        var seat2 = new TravelSeat(1, TravelFixture.getDepartments(), 1, 2);

        travel.AddSeat(seat1);
        travel.AddSeat(seat2);

        foreach (var seat in travel.Seats)
        {
            Assert.Equal(travel.DepartmentRoute.Count, seat.Bitmap.Length);
        }
    }

    [Fact]
    public void SeatsCount_ShouldNotExceedMaxSeatsCount()
    {

        var travel = new Travel(1, 3, TravelFixture.getDepartments(), DateTime.Now, null, null);
        int index = 0;
        foreach (var dep in TravelFixture.getDepartments())
        {
            travel.AddSeat(new TravelSeat(index, TravelFixture.getDepartments(), 1, index));
            index++;
        }
        Assert.True(travel.Seats.Count == travel.MaxSeatsCount);

        var seat = new TravelSeat(index, TravelFixture.getDepartments(), 1, index);

        Assert.Throws<InvalidOperationException>(() => travel.AddSeat(seat));
    }

    [Fact]
    public void Seats_ShouldNotHaveDuplicateArmchairNumbers()
    {

        var travel = new Travel(1, 10, TravelFixture.getDepartments(), DateTime.Now, null, null);

        var seat1 = new TravelSeat(1, TravelFixture.getDepartments(), 1, 1);
        var seat2 = new TravelSeat(2, TravelFixture.getDepartments(), 1, 1);

        travel.AddSeat(seat1);

        Assert.Throws<InvalidOperationException>(() => travel.AddSeat(seat2));
    }
    [Fact]
    public void Tickets_ShouldHaveValidSeats()
    {

        var travel = new Travel(1, 10, TravelFixture.getDepartments(), DateTime.Now, null, null);

        var seat1 = new TravelSeat(1, TravelFixture.getDepartments(), 1, 1);
        var ticket1 = new Ticket(1, 1, 1, 1, 2);

        travel.AddSeat(seat1);
        travel.AddTicket(ticket1);

        Assert.Contains(ticket1, travel.Tickets);
    }
    [Fact]
    public void IsFull_ShouldReturnTrue_WhenAllSeatsAreOccupied()
    {

        var travel = new Travel(1, 2, TravelFixture.getDepartments(), DateTime.Now, null, null);

        var seat1 = new TravelSeat(1, TravelFixture.getDepartments(), 1, 1);
        var seat2 = new TravelSeat(2, TravelFixture.getDepartments(), 1, 2);

        seat1.UpdateBitmap(0, TravelFixture.getDepartments().Count - 1);
        seat2.UpdateBitmap(0, TravelFixture.getDepartments().Count - 1);

        travel.AddSeat(seat1);
        travel.AddSeat(seat2);

        Assert.Multiple(() =>
        {
            Assert.False(seat1.IsSeatAvailable());
            Assert.False(seat2.IsSeatAvailable());
            Assert.True(travel.IsFull());
        });
    }
    [Fact]
    public void AddTicket_ShouldThrowException_WhenSeatIsAlreadyOccupied()
    {

        var travel = new Travel(1, 10, TravelFixture.getDepartments(), DateTime.Now, null, null);

        var seat1 = new TravelSeat(1, TravelFixture.getDepartments(), 1, 1);
        var ticket1 = new Ticket(1, 1, 1, 1, 2);
        var ticket2 = new Ticket(2, 1, 1, 1, 2);

        travel.AddSeat(seat1);
        travel.AddTicket(ticket1);

        Assert.Throws<InvalidOperationException>(() => travel.AddTicket(ticket2));
    }

    [Fact]
    public void GetAllAvailableSeats_ShoulAddExistingSeatIfExistsToAvaliableSeats()
    {
        var travel = new Travel(1, 10, TravelFixture.getDepartments(), DateTime.Now, null, null);
        var startDepId = 1;
        var endDepId = 2;
        var existingSeat = new TravelSeat(1, TravelFixture.getDepartments(), 1, 1);
        travel.AddSeat(existingSeat);

        var avaliableSeats = travel.GetAllAvailableSeats(startDepId, endDepId);

        Assert.Contains(existingSeat, avaliableSeats);
    }

    [Fact]
    public void GetAllAvailableSeats_ShoulCreateNewSeatsIfDoesNotExists()
    {
        var travel = new Travel(1, 10, TravelFixture.getDepartments(), DateTime.Now, null, null);
        var startDepId = 1;
        var endDepId = 2;

        var avaliableSeats = travel.GetAllAvailableSeats(startDepId, endDepId);

        Assert.True(travel.Seats.Count() == 0);
        Assert.True(avaliableSeats.Count() > 0);
    }

    [Fact]
    public void ReserveSeat_ShouldAddTicketToTravel()
    {

        var travel = new Travel(1, 10, TravelFixture.getDepartments(), DateTime.Now, null, null);
        var avaliableSeats = travel.GetAllAvailableSeats(1, 2);
        var seat = avaliableSeats.First();
        var ticket1 = new Ticket(1, seat.Id, 1, 1, 2);

        travel.ReserveSeat(ticket1, seat);

        Assert.Contains(ticket1, travel.Tickets);
    }

    [Fact]
    public void GetAllAvailableSeats_ShoudNotShowSeatIfSeatIsNotAvaliable()
    {
        var travel = new Travel(1, 10, TravelFixture.getDepartments(), DateTime.Now, null, null);
        var startDepId = 1;
        var endDepId = 2;

        var avaliableSeats1 = travel.GetAllAvailableSeats(startDepId, endDepId);
        var seat1 = avaliableSeats1.First();
        var ticket1 = new Ticket(1, seat1.Id, travel.Id, startDepId, endDepId);

        int startLocation = travel.GetBitmapLocationFromDepartmentRoute(startDepId);
        int endLocation = travel.GetBitmapLocationFromDepartmentRoute(endDepId);

        Assert.DoesNotContain(seat1, travel.Seats);

        travel.ReserveSeat(ticket1, seat1);

        Assert.Contains(ticket1, travel.Tickets);
        Assert.Contains(seat1, travel.Seats);
        Assert.False(seat1.IsSeatAvailableFor(startLocation, endLocation));


        var avaliableSeats2 = travel.GetAllAvailableSeats(startDepId, endDepId);
        var seat2 = avaliableSeats2.First();

        Assert.True(seat2.ArmchairNumber != seat1.ArmchairNumber);


    }
}
