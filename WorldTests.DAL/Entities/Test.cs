using System;
using System.Collections.Generic;

namespace WorldTests.DAL.Entities
{
    public class Test
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
