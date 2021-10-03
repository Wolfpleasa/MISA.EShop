using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IUnitRepository
    {
        /// <summary>
        /// Hàm lấy tất cả đơn vị tính
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        List<Unit> GetAll();

        /// <summary>
        /// Hàm thêm mới đơn vị tính
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        IEnumerable<Unit> Add(Unit unit);

        /// <summary>
        /// Hàm lấy đơn vị tính theo tên
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        IEnumerable<Unit> GetUnitByName(string unitName);

        /// <summary>
        /// Hàm check trùng tên đơn vị tính
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        bool CheckNameExist(string unitName);
    }
}
