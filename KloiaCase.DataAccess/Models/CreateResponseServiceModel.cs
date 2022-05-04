using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.DataAccess.Models
{
    public class CreateResponseServiceModel : ResponseServiceModelBase
    {
        public int CreatedEntityId { get; set; }
    }
}
