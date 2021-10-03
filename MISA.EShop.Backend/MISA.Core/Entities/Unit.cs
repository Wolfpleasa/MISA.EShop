using MISA.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class Unit
    {
        #region Property
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        [MISARequired("Tên đơn vị")]
        public string UnitName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
