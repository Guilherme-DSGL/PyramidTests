using Pyramid.Core;
using System.Collections;
using Xunit.Priority;

namespace Pyramid.Tests.SystemTests;

public class TravelFixture
{
    public Travel Travel { get; private set; }

    public TravelFixture()
    {
        var travelId = 1;
        List<Department> departments = [new Department(1, "Origin"), new Department(2, "Destination")];
        Travel = new Travel(travelId, 2, departments, DateTime.Now, null, null);

        var seat1 = new TravelSeat(1, new BitArray(departments.Count), travelId, 1);
        var seat2 = new TravelSeat(2, new BitArray(departments.Count), travelId, 2);

        Travel.AddSeat(seat1);
        Travel.AddSeat(seat2);
    }
}

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class TravelSystemTests : IClassFixture<TravelFixture>
{
    private readonly Travel _travel;

    public TravelSystemTests(TravelFixture fixture)
    {
        _travel = fixture.Travel;
    }

    [Fact, Priority(1)]
    public void SeatShouldBeAvailable_AfterCreation()
    {
        var contains = _travel.Seats.Any(s => s.IsSeatAvailable());
        Assert.True(contains);
    }

    [Fact, Priority(2)]
    public void ReserveSeat_ShouldMarkItAsOccupied()
    {
        var seat = _travel.Seats.First();
        var ticket = new Ticket(1, seat.Id, _travel.Id, _travel.DepartmentRoute.First().Id, _travel.DepartmentRoute.Last().Id);

        _travel.AddTicket(ticket);

        int startLocation = _travel.GetBitmapLocationFromDepartmentRoute(_travel.DepartmentRoute.First().Id);
        int endLocation = _travel.GetBitmapLocationFromDepartmentRoute(_travel.DepartmentRoute.Last().Id);

        Assert.False(seat.IsSeatAvailableFor(startLocation, endLocation));
    }

    [Fact, Priority(3)]
    public void AttemptToReserveOccupiedSeat_ShouldThrowException()
    {
        var seat = _travel.Seats.First();
        var invalidTicket = new Ticket(2, seat.Id, _travel.Id, _travel.DepartmentRoute.First().Id, _travel.DepartmentRoute.Last().Id);

        Assert.Throws<InvalidOperationException>(() => _travel.AddTicket(invalidTicket));
    }

    [Fact, Priority(4)]
    public void WhenAllSeatsAreOccupied_TravelShouldBeFull()
    {
        foreach (var seat in _travel.Seats)
        {
            if (seat.IsSeatAvailable())
            {
                var ticket = new Ticket(seat.Id, seat.Id, _travel.Id, _travel.DepartmentRoute.First().Id, _travel.DepartmentRoute.Last().Id);
                _travel.AddTicket(ticket);
            }
        }

        Assert.True(_travel.IsFull());
    }
}
