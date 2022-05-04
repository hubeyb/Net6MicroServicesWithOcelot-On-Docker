using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.DataAccess.Models
{
    public class ResponseServiceModelBase
    {
        public string ValidationdMessage { get; set; }
        public bool OperationSuccess { get; set; }
    }
}
