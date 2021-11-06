using Microsoft.AspNetCore.Http;
using MISA.Core.Entities;
using MISA.Core.Enumeration;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using MISA.Core.MISAAttribute;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class ProductService : IProductService
    {
        #region DECLARE
        IProductRepository _productRepository;
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public ProductService(IProductRepository productRepository)
        {
            _serviceResult = new ServiceResult();
            _productRepository = productRepository;
        }

        #endregion

        #region methods
        public ServiceResult GetById(Guid productId)
        {
            _serviceResult.Data = _productRepository.GetById(productId);
            return _serviceResult;
        }
        public ServiceResult Add(Product product, Picture picture)
        {
            bool isValid = true;
            isValid = Validate(Guid.Empty, product);

            if (product?.ProductDetails?.Count > 0 && isValid)
            {
                var productDetails = product.ProductDetails;
                for (int i = 0; i < productDetails.Count(); i++)
                {   
                    isValid = Validate(Guid.Empty, productDetails[i]);
                }            
            }

            if (isValid)
            {
                _serviceResult.Data = _productRepository.Add(product, picture);
            }
            return _serviceResult;
        }

        public ServiceResult Edit(Product product, Guid productId, Picture picture)
        {
            bool isValid = true;
            isValid = Validate(productId, product);

            if (product?.ProductDetails?.Count > 0 && isValid)
            {
                var productDetails = product.ProductDetails;
                for (int i = 0; i < productDetails.Count(); i++)
                {   
                    if(productDetails[i].FlagMode == (int)MISAEnum.FlagMode.Add)
                    {
                        isValid = Validate(Guid.Empty, productDetails[i]);
                    }else if(productDetails[i].FlagMode == (int)MISAEnum.FlagMode.Edit)
                    {
                        isValid = Validate(productDetails[i].ProductId, productDetails[i]);
                    }
                }
            }

            if (isValid)
            {
                _serviceResult.Data = _productRepository.Edit(product, productId, picture);
            }
            return _serviceResult;
        }

        public ServiceResult AutoGenSKUCode(string productName)
        {
            string skucode = _productRepository.AutoGenSKUCode(productName);
            productName = CommonFn.CommonFn.utf8Convert1(productName);
            string[] arrListStr = productName.Split(' ');
            string name = "";
            for(int i = 0; i<arrListStr.Length; i++)
            {
                name += arrListStr[i].Substring(0, 1).ToUpper();
            }
            if (string.IsNullOrEmpty(skucode))
            {   
                skucode = name + "01";
            }
            else
            {
                int code = Convert.ToInt32(skucode.Substring(name.Length)) + 1;
                if(code < 10)
                {
                    skucode = name + "0" + code;
                }
                else
                {
                    skucode = name + code;
                }
            }
            _serviceResult.Data = skucode;
            return _serviceResult;
        }
        #endregion

        #region Validate
        /// <summary>
        /// Hàm kiểm tra xem hàng hóa có hợp lệ không
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Validate(Guid productId, Product product)
        {
            bool isValid = true;
            isValid = ValidateRequired(product);

            if (isValid)
            {
                isValid = CheckCodeExist(productId,  product.SKUCode);
            }

            if (isValid)
            {
                isValid = ValidateCode(productId, product.ProductCode);
            }


            return isValid;
        }
        /// <summary>
        /// Hàm kiểm tra nhập các trường bắt buộc
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// Created By : Ngọc 25/09/2021
        public bool ValidateRequired(Product product)
        {
            var isValid = true;
            var properties = product.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var requiredProp = prop.GetCustomAttributes(typeof(MISARequired), true);

                if (requiredProp.Length > 0)
                {
                    var propName = (requiredProp[0] as MISARequired).Name;
                    var propValue = prop.GetValue(product).ToString();
                    if (string.IsNullOrEmpty(propValue))
                    {
                        isValid = false;
                        _serviceResult.Message = string.Format(Properties.ResourceVN.Empty_Field, propName);
                        _serviceResult.isValid = false;
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Hàm check trùng mã SKU
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="SKUCode"></param>
        /// <returns></returns>
        /// Created By : Ngọc 03/10/2021
        public bool CheckCodeExist(Guid productId, string SKUCode)
        {
            var isValid = true;

            isValid = _productRepository.CheckCodeExist(productId, SKUCode);
            if (!isValid)
            {
                _serviceResult.isValid = false;
                _serviceResult.Data = SKUCode;
                _serviceResult.Message = string.Format(Properties.ResourceVN.Duplicate_SKUCode, SKUCode);
            }

            return isValid;
        }

        /// <summary>
        /// Hàm check trùng mã vạch
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        /// Created By : Ngọc 03/10/2021
        public bool ValidateCode(Guid productId, string productCode)
        {
            var isValid = true;

            isValid = _productRepository.ValidateCode(productId, productCode);
            if (!isValid)
            {
                _serviceResult.isValid = false;
                _serviceResult.Data = productCode;
                _serviceResult.Message = string.Format(Properties.ResourceVN.Duplicate_ProductCode, productCode);
            }

            return isValid;
        }
        #endregion
    }
}

