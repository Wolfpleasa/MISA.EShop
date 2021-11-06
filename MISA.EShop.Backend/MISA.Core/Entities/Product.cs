using MISA.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class Product
    {
        #region Property
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Tên hàng hóa
        /// </summary>
        [MISARequired("Tên hàng hóa")]
        public string ProductName { get; set; }

        /// <summary>
        /// Mã SKU
        /// </summary>
        public string SKUCode { get; set; }

        /// <summary>
        /// Mã vạch
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Giá mua
        /// </summary>
        public double? PurchasePrice { get; set; }
        
        /// <summary>
        /// Giá bán
        /// </summary>
        public double? SellingPrice { get; set; }
        
        /// <summary>
        /// Hiện thị trên màn hình bán hàng
        /// </summary>
        public int? Display { get; set; }
        
        /// <summary>
        /// Trạng thái kinh doanh
        /// </summary>
        public int? StatusBusiness { get; set; }

        /// <summary>
        /// Màu sắc
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Màu sắc
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public double? CreateDate { get; set; }

        /// <summary>
        /// Khóa ngoại 
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Đơn vị tính
        /// </summary>
        public Guid? UnitId { get; set; }

        /// <summary>
        /// Nhóm hàng hóa
        /// </summary>
        public Guid? ProductGroupId { get; set; }

        /// <summary>
        /// Ảnh hàng hóa
        /// </summary>
        public Guid? PictureId { get; set; }

        /// <summary>
        /// List các hàng hóa chi tiết
        /// </summary>
        [MISANotMap]
        public List<Product> ProductDetails { get; set; }

        /// <summary>
        /// Trạng thái hàng hóa chi tiết khi sửa
        /// </summary>
        [MISANotMap("Cờ")]
        public int? FlagMode { get; set; }

        #endregion
    }
}
