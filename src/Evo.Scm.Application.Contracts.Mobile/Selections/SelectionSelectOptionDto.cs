using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Scm.Selections
{
    public class SelectionSelectOptionDto
    {
        public SelectionSelectOptionDto(Guid id, string sn)
        {
            this.Id = id;
            this.Sn = sn;
        }

        /// <summary>
        /// 选品Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 选品编号
        /// </summary>
        public string Sn { get; set; }
    }
}
