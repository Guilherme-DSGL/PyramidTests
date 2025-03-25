using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Core;

 public class Department
{
    public int Id { get; }
    public string Name { get; }

    public Department(int id, string name) { 
        Id = id;
        Name = name;
    }
}
