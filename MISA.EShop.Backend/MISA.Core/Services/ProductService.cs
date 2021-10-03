using Microsoft.AspNetCore.Http;
using MISA.Core.Entities;
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
        public ServiceResult Add(Product product)
        {
            bool isValid = true;
            isValid = ValidateRequired(product);

            if (product?.ProductDetails?.Count > 0)
            {
                var productDetails = product.ProductDetails;
                for (int i = 0; i < productDetails.Count(); i++)
                {
                    isValid = ValidateRequired(productDetails[i]);
                }
            }

            if (isValid)
            {
                _serviceResult.Data = _productRepository.Add(product);
            }
            return _serviceResult;
        }

        public ServiceResult Edit(Product product, Guid productId)
        {
            bool isValid = true;
            isValid = ValidateRequired(product);

            if (product?.ProductDetails?.Count > 0)
            {
                var productDetails = product.ProductDetails;
                for (int i = 0; i < productDetails.Count(); i++)
                {
                    isValid = ValidateRequired(productDetails[i]);
                }
            }

            if (isValid)
            {
                _serviceResult.Data = _productRepository.Edit(product, productId);
            }
            return _serviceResult;
        }
        #endregion

        #region Validate
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
        #endregion
    }
}

