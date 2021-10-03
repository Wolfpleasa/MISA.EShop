using MISA.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class ProductGroup
    {
        #region Property
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid ProductGroupId { get; set; }

        /// <summary>
        /// Tên nhóm hàng hóa
        /// </summary>
        [MISARequired("Tên nhóm hàng hóa")]
        public string ProductGroupName { get; set; }

        /// <summary>
        /// Mã nhóm hàng hóa
        /// </summary>
        [MISARequired("Mã nhóm hàng hóa")]
        public string ProductGroupCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
