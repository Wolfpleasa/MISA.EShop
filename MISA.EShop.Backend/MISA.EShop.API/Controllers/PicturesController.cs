using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.CommonFn;
using MISA.Core.Entities;
using MISA.Core.Enumeration;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.EShop.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        #region DECLARE
        IPictureRepository _pictureRepository;
        IPictureService _pictureService;
        ServiceResult _serviceResult;
        #endregion

        public PicturesController(IPictureService pictureService, IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
            _pictureService = pictureService;
            _serviceResult = new ServiceResult();
        }

        #region Methods
        /// <summary>
        /// Lấy danh sách hàng hóa theo id 
        /// </summary>
        /// <returns>Hình ảnh</returns>
        /// Created By: Ngọc 05/10/2021
        [HttpGet("{pictureId}")]
        public IActionResult GetProductById(Guid pictureId)
        {
            try
            {
                var picture = _pictureRepository.GetById(pictureId);

                if (picture == null)
                {
                    return NoContent();
                }

                return Ok(picture);
            }
            catch (Exception ex)
            {
                var newObj = CommonFn.ObjError(ex.Message);
                return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
            }
        }

        #endregion
    }
}
