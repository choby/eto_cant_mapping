using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Scm.Oss;

public class PolicyConfig
{
    public string expiration { get; set; }
    public List<List<Object>> conditions { get; set; }
}
