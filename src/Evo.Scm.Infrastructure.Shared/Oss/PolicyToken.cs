using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Scm.Oss;

public class PolicyToken
{
    public string accessId { get; set; }
    public string policy { get; set; }
    public string signature { get; set; }
    public string dir { get; set; }
    public string host { get; set; }
    public string expire { get; set; }
    public string callback { get; set; }
}
