using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace starterpack.Models
{
    public class BaseModel
    {
        public Int64 Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
