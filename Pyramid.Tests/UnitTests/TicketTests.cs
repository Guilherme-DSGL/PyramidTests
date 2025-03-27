using System;
using System.Collections.Generic;
using Xunit;

using Pyramid.Core;

namespace Pyramid.Tests;

public class TicketTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        int id = 1;
        int seatId = 1;
        int travelId = 1;
        int startDepartmentId = 10;
        int endDepartmentId = 20;

        var ticket = new Ticket(id, seatId, travelId, startDepartmentId, endDepartmentId);

        Assert.Equal(ticket.Id, id);
        Assert.Equal(seatId, ticket.SeatId);
        Assert.Equal(startDepartmentId, ticket.StartDepartmentId);
        Assert.Equal(endDepartmentId, ticket.EndDepartmentId);
    }
}