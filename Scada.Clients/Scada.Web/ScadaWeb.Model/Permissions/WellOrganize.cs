using ScadaWeb.DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaWeb.Model
{
    [Table("WellOrganize")]
    public class WellOrganizeModel
    {
        public long OrganizeId { get; set; }
        public long WellId { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        [DapperExtensions.Key(true)]
        public long Id { get; set; }
    }
}
