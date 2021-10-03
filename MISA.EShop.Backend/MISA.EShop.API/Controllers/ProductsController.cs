using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Properties;
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
    public class ProductsController : ControllerBase
    {
        #region DECLARE
        IProductRepository _productRepository;
        IProductService _productService;
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public ProductsController(IProductService productService, IProductRepository productRepository)
        {
            _productService = productService;
            _productRepository = productRepository;
            _serviceResult = new ServiceResult();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả hàng hóa
        /// </summary>
        /// <returns>Danh sách hàng hóa</returns>
        /// Created By: Ngọc 25/09/2021
        [HttpGet]
        public IActionResult GetAllProduct()
        {
            try
            {
                var products = _productRepository.GetAll();
                
                if(products.Count == 0)
                {
                    return NoContent();
                }

                return Ok(products);
            }catch(Exception ex)
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
        /// Lấy danh sách hàng hóa theo id hoặc khóa ngoại
        /// </summary>
        /// <returns>Danh sách hàng hóa</returns>
        /// Created By: Ngọc 25/09/2021
        [HttpGet("{productId}")]
        public IActionResult GetProductById(Guid productId)
        {
            try
            {
                var products = _productRepository.GetById(productId);

                if (products.Count == 0)
                {
                    return NoContent();
                }

                return Ok(products);
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

        [HttpGet("detail/{productId}")]
        public IActionResult GetProductDetailById(Guid productId)
        {
            try
            {
                Product products = _productRepository.GetProductWithDetail(productId);


                return Ok(products);
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

        /// <summary>
        /// Lấy danh sách hàng hóa theo id hoặc khóa ngoại
        /// </summary>
        /// <returns>Danh sách hàng hóa</returns>
        /// Created By: Ngọc 25/09/2021
        [HttpPost("{paging}")]
        public IActionResult GetPagingResullt(List<PagingParam> pagingParams, int pageSize, int pageNumber)
        {
            try
            {
                var pagingResult = _productRepository.GetPagingResult(pagingParams, pageSize, pageNumber);

                if (pagingResult.Products.Count == 0)
                {
                    return NoContent();
                }

                return Ok(pagingResult);
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

        /// <summary>
        /// Thêm mới hàng hóa
        /// </summary>
        /// <returns>Danh sách hàng hóa</returns>
        /// Created By: Ngọc 25/09/2021
        [HttpPost]
        public IActionResult InsertProduct(Product product)
        {
            try
            {
                var serviceResult = _productService.Add(product);
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

        /// <summary>
        /// Thêm cập nhật hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(Product product, Guid productId)
        {
            try
            {
                var serviceResult = _productService.Edit(product, productId);
                if (serviceResult.isValid)
                {
                    return StatusCode(200, serviceResult.Data);
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

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(Guid productId)
        {
            try
            {
                var rowEffects = _productRepository.Delete(productId);
                return Ok(rowEffects);
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
