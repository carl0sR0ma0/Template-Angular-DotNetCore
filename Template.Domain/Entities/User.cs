using System;
using System.Collections.Generic;
using System.Text;
using Template.Domain.Models;

namespace Template.Domain.Entities
{
    public class User: Entity
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}
