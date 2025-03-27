using Pyramid.Core;
using System.Collections;
using Xunit;

namespace Pyramid.Tests.UnitTests;


public class TravelTests
{
    private List<Department> departments = [new Department(id: 1, name: "Crateús"), new Department(id: 2, name: "Nova Russas"), new Department(id: 3, name: "Fortaleza"),];
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {

        var travelDate = DateTime.Now;

        var travel = new Travel(1, 10, departments, travelDate, null, null);

        Assert.Equal(1, travel.Id);
        Assert.Equal(10, travel.MaxSeatsCount);
        Assert.Equal(departments, travel.DepartmentRoute);
        Assert.Equal(travelDate, travel.TravelDate);
        Assert.Empty(travel.Seats);
        Assert.Empty(travel.Tickets);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDepartmentRouteIsNull()
    {
        var travelDate = DateTime.Now;
        List<Department>? departments = null;

        Assert.Throws<ArgumentNullException>(() => new Travel(1, 10, departments!, travelDate, null, null));
    }

    [Fact] 
    public void GetBimapLocation_ShouldReturnCorrectBitmapLocationIndex()
    {
        var travelDate = DateTime.Now;
        var travel = new Travel(1, 10, departments, travelDate, null, null);

        var department = departments.First();
        Assert.Equal(0, travel.GetBitmapLocationFromDepartmentRoute(department.Id));
        Assert.Equal(0, travel.GetBitmapLocationFromDepartmentRoute(department.Name));
    }
    [Fact]
    public void GetBimapLocation_ShouldThrowArgumentErrorWhenDepartmentDoesNotExists()
    {
        var travelDate = DateTime.Now;
        var travel = new Travel(1, 10, departments, travelDate, null, null);

        var department = departments.First();
        Assert.Throws<ArgumentException>(() => travel.GetBitmapLocationFromDepartmentRoute(-99999999));
        Assert.Throws<ArgumentException>( () => travel.GetBitmapLocationFromDepartmentRoute("Nome não existente"));
    }


    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenDepartmentRouteIsEmpty()
    {
        var travelDate = DateTime.Now;

        Assert.Throws<ArgumentException>(() => new Travel(1, 10, new List<Department>(), travelDate, null, null));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenSeatsExceedMaxSeatsCount()
    {
        var travelDate = DateTime.Now;
        List<TravelSeat> seats = [
            new TravelSeat( 1,  new BitArray(departments.Count), 1,  3), 
            new TravelSeat( 2,  new BitArray(departments.Count),  1,  4),
            ];

        Assert.Throws<ArgumentException>(() => new Travel(1, 1, departments, travelDate, seats, null));
    }


}