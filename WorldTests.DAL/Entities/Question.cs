using System;
using System.Collections.Generic;

namespace WorldTests.DAL.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid TestId { get; set; }
        public virtual Test Test { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
