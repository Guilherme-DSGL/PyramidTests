using Pyramid.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Tests.UnitTests;

public class DepartmentTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        int id = 1;
        string name = "Crateús";

        var department = new Department(id, name);

        Assert.Equal(id, department.Id);
        Assert.Equal(name, department.Name);
    }
}
