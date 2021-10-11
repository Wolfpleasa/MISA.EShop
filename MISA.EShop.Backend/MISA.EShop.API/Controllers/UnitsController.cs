using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.CommonFn;
using MISA.Core.Enumeration;
using MISA.Core.Entities;
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
    public class UnitsController : ControllerBase
    {
        #region DECLARE
        IUnitRepository _unitRepository;
        IUnitService _unitService;
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public UnitsController(IUnitService unitService, IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
            _unitService = unitService;
            _serviceResult = new ServiceResult();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả đơn vị
        /// </summary>
        /// <returns>Danh sách đơn vị tính</returns>
        /// Created By: Ngọc 25/09/2021
        [HttpGet]
        public IActionResult GetAllUnit()
        {
            try
            {
                var units = _unitRepository.GetAll();

                if (units.Count == 0)
                {
                    return NoContent();
                }

                return Ok(units);
            }
            catch (Exception ex)
            {
                var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
            }
        }

        /// <summary>
        /// Lấy đơn vị theo tên
        /// </summary>
        /// <returns> đơn vị tính</returns>
        /// Created By: Ngọc 25/09/2021
        [HttpGet("{unitName}")]
        public IActionResult GetByName(string unitName)
        {
            try
            {                        
                var unit = _unitRepository.GetUnitByName(unitName);

                if (unit == null)
                {
                    return NoContent();
                }

                return Ok(unit);
            }
            catch (Exception ex)
            {
                var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
            }
        }

        /// <summary>
        /// Thêm mới đơn vị tính
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        [HttpPost]
        public IActionResult InsertUnit(Unit unit)
        {
            try
            {
                var serviceResult = _unitService.Add(unit);
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
