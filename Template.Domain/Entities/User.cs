using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
    }
}
