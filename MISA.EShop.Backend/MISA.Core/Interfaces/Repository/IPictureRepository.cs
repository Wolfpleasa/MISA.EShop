using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IPictureRepository
    {
        /// <summary>
        /// Hàm lấy ảnh theo id
        /// </summary>
        /// <returns>Ảnh</returns>
        /// Created By: Ngọc 25/09/2021
        Picture GetById(Guid pictureId);
    }
}
