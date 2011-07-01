using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nen.Data.ObjectRelationalMapper;

namespace SocialChoose.Domain.Entities
{
    public class Base
    {
        public int Id { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
