using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface IUnitService
    {
        #region Methods
        /// <summary>
        /// Nghiệp vụ thêm mới đơn vị tính
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>ServiceResult: Kết quả xử lý nghiệp vụ</returns>
        /// Created By: Ngọc 25/09/2021
        ServiceResult Add(Unit unit);
        #endregion
    }
}
