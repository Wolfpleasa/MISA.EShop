using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class Picture
    {   
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid PictureId { get; set; }

        /// <summary>
        /// Tên ảnh
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        /// Định dạng ảnh
        /// </summary>
        public string PictureTail { get; set; }

        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string PicturePath { get; set; }

        /// <summary>
        /// Kiểu ảnh
        /// </summary>
        public int? Type { get; set; }
    }
}
