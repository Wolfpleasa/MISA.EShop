using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IProductRepository
    {
        /// <summary>
        /// Hàm lấy tất cả hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        List<Product> GetAll();

        /// <summary>
        /// Hàm lấy hàng hóa theo id
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        List<Product> GetById(Guid ProductId);

        /// <summary>
        /// Hàm thêm mới hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        IEnumerable<Product> Add(Product product);

        /// <summary>
        /// Hàm sửa hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        IEnumerable<Product> Edit(Product product, Guid ProductId);

        /// <summary>
        /// Hàm xóa hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        IEnumerable<Product> Delete(Guid ProductId);

        /// <summary>
        /// Hàm phân trang, tìm kiếm
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        PagingResult GetPagingResult(List<PagingParam> pagingParams,int pageSize, int pageNumber);

        /// <summary>
        /// Hàm lấy hàng hóa theo id, để hàng hóa chi tiết là 1 thuộc tính của hàng hóa cha
        /// </summary>
        /// <returns>hàng hóa</returns>
        /// Created By: Ngọc 01/10/2021
        Product GetProductWithDetail(Guid productId);
    }
}
