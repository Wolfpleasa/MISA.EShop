using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                var newObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.ResourceVN.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return BadRequest(newObj);
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
                //string str = unitName.Substring(0, 1).ToUpper();
                //string str1 = unitName.Substring(1, unitName.Length);
                //unitName = str + str1;
                
                var unit = _unitRepository.GetUnitByName(unitName);

                if (!unit.Any())
                {
                    return NoContent();
                }

                return Ok(unit);
            }
            catch (Exception ex)
            {
                var newObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.ResourceVN.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return BadRequest(newObj);
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
                var newObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.ResourceVN.Error_Message_UserVN,
                    errorCode = "misa-001",
                    moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                return StatusCode(500, newObj);
            }
        }
        #endregion
    }
}
