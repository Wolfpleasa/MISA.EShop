using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using MISA.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class ProductGroupService : IProductGroupService
    {
        #region DECLARE
        IProductGroupRepository _productGroupRepository;
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public ProductGroupService(IProductGroupRepository productGroupRepository)
        {
            _serviceResult = new ServiceResult();
            _productGroupRepository = productGroupRepository;
        }
        #endregion

        #region Methods
        public ServiceResult Add(ProductGroup productGroup)
        {

            bool isValid = true;
            isValid = ValidateRequired(productGroup);

            if (isValid)
            {
                isValid = CheckNameExist(productGroup.ProductGroupName);
            }

            if (isValid)
            {
                _serviceResult.Data = _productGroupRepository.Add(productGroup);
            }
         
            return _serviceResult;
        }
        #endregion

        #region Validate
        /// <summary>
        /// Hàm kiểm tra nhập các trường bắt buộc
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns></returns>
        /// Created By : Ngọc 25/09/2021
        public bool ValidateRequired(ProductGroup productGroup)
        {
            var isValid = true;
            var properties = productGroup.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var requiredProp = prop.GetCustomAttributes(typeof(MISARequired), true);

                if (requiredProp.Length > 0)
                {
                    var propName = (requiredProp[0] as MISARequired).Name;
                    var propValue = prop.GetValue(productGroup).ToString();
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
        /// Hàm kiểm tra trùng tên nhóm hàng hóa 
        /// </summary>
        /// <param name="productGroupName"></param>
        /// <returns></returns>
        /// CreatedBy : Ngọc 18/8/2021
        public bool CheckNameExist(string productGroupName)
        {
            var isValid = _productGroupRepository.CheckNameExist(productGroupName);
            if (!isValid)
            {
                _serviceResult.Message = string.Format(Properties.ResourceVN.Duplicate_ProductGroupName, productGroupName);
                _serviceResult.isValid = false;
            }
            return isValid;
        }
        #endregion
    }
}
