using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IProductGroupRepository
    {
        /// <summary>
        /// Hàm lấy tất cả nhóm hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        List<ProductGroup> GetAll();

        /// <summary>
        /// Hàm thêm mới nhóm hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        IEnumerable<ProductGroup> Add(ProductGroup productGroup);

        /// <summary>
        /// Hàm lấy nhóm hàng hóa theo tên
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        IEnumerable<ProductGroup> GetProductGroupByName(string productGroupName);

        /// <summary>
        /// Hàm check trùng tên nhóm hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        bool CheckNameExist(string productGroupName);
    }
}
