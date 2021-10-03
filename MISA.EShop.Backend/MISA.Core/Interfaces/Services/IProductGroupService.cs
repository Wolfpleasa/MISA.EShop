using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface IProductGroupService
    {
        #region Methods
        /// <summary>
        /// Nghiệp vụ thêm mới nhóm hàng hóa
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns>ServiceResult: Kết quả xử lý nghiệp vụ</returns>
        /// Created By: Ngọc 25/09/2021
        ServiceResult Add(ProductGroup productGroup);

        #endregion
    }
}
