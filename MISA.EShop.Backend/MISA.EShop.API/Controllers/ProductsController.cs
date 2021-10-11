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
using System.IO;
using Newtonsoft.Json;
using MISA.Core.CommonFn;
using MISA.Core.Enumeration;

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

                if (products.Count == 0)
                {
                    return NoContent();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                var newObj = CommonFn.ObjError(ex.Message);
                return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
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
                var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
            }
        }

        /// <summary>
        /// Hàm lấy hàng hóa(các hàng hóa chi tiết nằm trong thuộc tính của cha)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
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
                var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
            }
        }

        [HttpPost("getNewCode")]
        public IActionResult AutoGenSKUCode(Product product)
        {
            try
            {
                ServiceResult serviceResult = _productService.AutoGenSKUCode(product.ProductName);
                return Ok(serviceResult.Data);
            }
            catch (Exception ex)
            {
                var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
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
                var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
            }
        }

        /// <summary>
        /// Thêm mới hàng hóa
        /// </summary>
        /// <returns>Danh sách hàng hóa</returns>
        /// Created By: Ngọc 25/09/2021
        [HttpPost]
        public IActionResult InsertProduct([FromForm] IFormFile formFile, [FromForm] string data)
        {
            try
            {
                Product product = JsonConvert.DeserializeObject<Product>(data);
                Picture picture = new Picture();
                if (formFile != null)
                {
                    picture.PictureId = Guid.NewGuid();
                    picture.PictureName = picture.PictureId.ToString();
                    picture.PicturePath = "images/products/";
                    picture.PictureTail = Path.GetExtension(formFile.FileName);
                    picture.Type = 1;

                    product.PictureId = picture.PictureId;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\products", picture.PictureName + picture.PictureTail);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    formFile.CopyToAsync(fileStream);
                }

                var serviceResult = _productService.Add(product, picture);
                if (serviceResult.isValid && (int)serviceResult.Data != 0)
                {
                    return StatusCode(201, serviceResult.Data);
                }
                else
                {
                    var newObj = new
                    {
                        userMsg = serviceResult.Message,
                        dataErr = serviceResult.Data,
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

        /// <summary>
        /// Thêm cập nhật hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(Guid productId, [FromForm] IFormFile formFile, [FromForm] string data)
        {
            try
            {
                // Chuyển dữ liệu về đối tượng hàng hóa
                Product product = JsonConvert.DeserializeObject<Product>(data);

                Picture picture = new Picture();
                if (formFile != null)
                {
                    picture.PictureId = Guid.NewGuid();
                    picture.PictureName = picture.PictureId.ToString();
                    picture.PicturePath = "images/products/";
                    picture.PictureTail = Path.GetExtension(formFile.FileName);
                    picture.Type = 1;

                    product.PictureId = picture.PictureId;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\products", picture.PictureName + picture.PictureTail);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    formFile.CopyToAsync(fileStream);
                }

                var serviceResult = _productService.Edit(product, productId, picture);
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
                var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
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
                var newObj = CommonFn.ObjError(ex.Message);
                  return StatusCode((int)MISAEnum.HTTPStatus.ServerError, newObj);
            }
        }
        #endregion
    }
}
