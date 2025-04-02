using System;
using System.Collections.Generic;
using Xunit;

using Pyramid.Core;

namespace Pyramid.Tests;

public class TravelSeatTests
{

    private List<Department> departmentsMock = [new Department(id: 1, name: "Crateús"), new Department(id: 2, name: "Nova Russas"), new Department(id: 3, name: "Fortaleza"),];
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {      
        var travelSeat = new TravelSeat(1, departmentsMock, 1, 2);

        Assert.Equal(2, travelSeat.ArmchairNumber );
        Assert.Equal(1, travelSeat.TravelId);
        Assert.Equal(departmentsMock.Count, travelSeat.Bitmap.Count);
    }

    [Fact]
    public void IsSeatAvailableFor_ShouldReturnTrue_WhenBitMapIsEmpty()
    {
        var travelSeat = new TravelSeat(1, departmentsMock, 1, 1);

        var firtLocation = 0;
        var lastLocation = departmentsMock.Count - 1;

        bool isAvailable = travelSeat.IsSeatAvailableFor(firtLocation, lastLocation);

        Assert.True(isAvailable);
    }
    [Fact]
    public void IsSeatAvailableFor_ShouldReturnTrue_WhenBitMapIsMissingOne()
    {
        var travelSeat = new TravelSeat(1, departmentsMock, 1, 1);
        travelSeat.UpdateBitmap(1, 2);
        var firtLocation = 0;
        var lastLocation = 1;
        bool isAvailable = travelSeat.IsSeatAvailableFor(firtLocation, lastLocation);

        Assert.True(isAvailable);
    }

    [Fact]
    public void IsSeatAvailableFor_ShouldReturnFalse_WhenBitmapIsFilledWithSameParameters()
    {
        var travelSeat = new TravelSeat(1, departmentsMock, 1, 1);
        var firtLocation = 0;
        var lastLocation = departmentsMock.Count - 1;
        travelSeat.UpdateBitmap(firtLocation, lastLocation);

        bool isAvailable = travelSeat.IsSeatAvailableFor(firtLocation, lastLocation);

        Assert.False(isAvailable);
    }

    [Fact]
    public void IsSeatAvailable_ShouldReturnTrue_WhenBitMapIsEmpty()
    {
        var travelSeat = new TravelSeat(1, departmentsMock, 1, 1);

        bool isAvailable = travelSeat.IsSeatAvailable();

        Assert.True(isAvailable);
    }
    [Fact]
    public void IsSeatAvailable_ShouldReturnTrue_WhenBitMapIsMissingOne()
    {
        var travelSeat = new TravelSeat(1, departmentsMock, 1, 1);
        bool isAvailable = travelSeat.IsSeatAvailable();

        Assert.True(isAvailable);
    }


    [Fact]
    public void IsSeatAvailable_ShouldReturnFalse_WhenSeatIsFullFiled()
    {
        var travelSeat = new TravelSeat(1, departmentsMock, 1, 1);
        travelSeat.UpdateBitmap(0, departmentsMock.Count - 1);

        bool isAvailable = travelSeat.IsSeatAvailable();

        Assert.False(isAvailable);
    }


    [Fact]
    public void UpdateBitmap_ShouldThrowArgumentException_WhenInvalidRangeIsProvided()
    {
        var travelSeat = new TravelSeat(1, departmentsMock, 1, 1);

        Assert.Multiple();
    
        Assert.Throws<ArgumentException>(() => travelSeat.UpdateBitmap(-1, 2));
        Assert.Throws<ArgumentException>(() => travelSeat.UpdateBitmap(2, 3));
        Assert.Throws<ArgumentException>(() => travelSeat.UpdateBitmap(2, 1));
        Assert.Throws<ArgumentException>(() => travelSeat.UpdateBitmap(0, departmentsMock.Count + 1));
    }

    [Fact]
    public void UpdateBitmap_ShouldUpdateBitmapCorrectly()
    {
        var travelSeat = new TravelSeat(1, departmentsMock, 1, 1);

        travelSeat.UpdateBitmap(1, 2);

        Assert.False(travelSeat.IsSeatAvailableFor(1, 2));
        Assert.True(travelSeat.IsSeatAvailableFor(0, 1));
    }
}