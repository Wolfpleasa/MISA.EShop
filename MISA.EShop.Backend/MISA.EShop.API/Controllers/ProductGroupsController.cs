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
    public class ProductGroupsController : ControllerBase
    {
        #region DECLARE
        IProductGroupRepository _productGroupRepository;
        IProductGroupService _productGroupService;
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public ProductGroupsController(IProductGroupService productGroupService, IProductGroupRepository productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
            _productGroupService = productGroupService;
            _serviceResult = new ServiceResult();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả nhóm hàng hóa
        /// </summary>
        /// <returns>Danh sách nhóm hàng hóa</returns>
        /// Created By: Ngọc 25/09/2021
        [HttpGet]
        public IActionResult GetAllProductGroup()
        {
            try
            {
                var productGroups = _productGroupRepository.GetAll();

                if (productGroups.Count == 0)
                {
                    return NoContent();
                }

                return Ok(productGroups);
            }
            catch (Exception ex)
            {
                   var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
            }
        }

        /// <summary>
        /// Lấy nhóm hàng hóa theo tên
        /// </summary>
        /// <returns>nhóm hàng hóa</returns>
        /// Created By: Ngọc 25/09/2021
        [HttpGet("{productGroupName}")]
        public IActionResult GetByName(string productGroupName)
        {
            try
            {
                var productGroup = _productGroupRepository.GetProductGroupByName(productGroupName);

                if (productGroup == null)
                {
                    return NoContent();
                }

                return Ok(productGroup);
            }
            catch (Exception ex)
            {
                   var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
            }
        }

        /// <summary>
        /// Thêm mới nhóm hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        [HttpPost]
        public IActionResult InsertProductGroup(ProductGroup productGroup)
        {
            try
            {
                var serviceResult = _productGroupService.Add(productGroup);
                if (serviceResult.isValid)
                {
                    return StatusCode(201, serviceResult.Data);
                }
                else
                {
                    var newObj = new
                    {
                        userMsg = serviceResult.Message
                    };
                    return BadRequest(newObj);
                }
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
