using Microsoft.AspNetCore.Http;
using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    public interface IProductService
    {
        #region Methods
        /// <summary>
        /// Nghiệp vụ thêm mới khách hàng 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>ServiceResult: Kết quả xử lý nghiệp vụ</returns>
        /// Created By: Ngọc 25/09/2021
        ServiceResult Add(Product product);

        /// <summary>
        /// Nghiệp vụ sửa thông tin hàng hóa
        /// </summary>
        /// <param name="product"></param>
        /// <returns>ServiceResult: Kết quả xử lý nghiệp vụ</returns>
        /// Created By: Ngọc 25/09/2021
        ServiceResult Edit(Product product, Guid ProductId);

        /// <summary>
        /// Nghiệp vụ sửa thông tin hàng hóa
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>ServiceResult: Kết quả xử lý nghiệp vụ</returns>
        /// Created By: Ngọc 25/09/2021
        ServiceResult GetById(Guid productId);
        #endregion
    }
}
