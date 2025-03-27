using Pyramid.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Tests.IntegrationTests;

public class TravelSeatIntegrationTests
{
    private readonly List<Department> departments = [
        new Department(id: 1, name: "Crateús"),
        new Department(id: 2, name: "Nova Russas"),
        new Department(id: 3, name: "Fortaleza")
    ];

    [Fact]
    public void Bitmap_ShouldHaveSameCountAsDepartments()
    {
        var travelSeat = new TravelSeat(1, departments, 1, 2);

        Assert.Equal(departments.Count, travelSeat.Bitmap.Count);
    }
}
