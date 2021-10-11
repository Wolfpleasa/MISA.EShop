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
        int Add(Product product, Picture picture);

        /// <summary>
        /// Hàm sửa hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        int Edit(Product product, Guid ProductId, Picture picture);

        /// <summary>
        /// Hàm xóa hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        int Delete(Guid ProductId);

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

        /// <summary>
        /// Hàm check trùng mã vạch
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="SKUCode"></param>
        /// <returns></returns>
        bool CheckCodeExist(Guid productId, string SKUCode);

        /// <summary>
        /// Hàm check trùng mã SKU
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        bool ValidateCode(Guid productId, string productCode);

        /// <summary>
        /// Hàm sinh mã SKU tự động
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        string AutoGenSKUCode(string productName);
    }
}
