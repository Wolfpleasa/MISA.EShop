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
    public class UnitService : IUnitService
    {
        #region DECLARE
        IUnitRepository _unitRepository;
        ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public UnitService(IUnitRepository unitRepository)
        {
            _serviceResult = new ServiceResult();
            _unitRepository = unitRepository;
        }

        #endregion

        #region methods
        public ServiceResult Add(Unit unit)
        {
            var isValid = true;
            isValid = ValidateRequired(unit);
            if (isValid)
            {
                isValid = CheckNameExist(unit.UnitName);
            }

            if (isValid)
            {
                _serviceResult.Data = _unitRepository.Add(unit);
            }
            return _serviceResult;
        }
        #endregion

        #region Validate
        /// <summary>
        /// Hàm kiểm tra nhập các trường bắt buộc
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        /// Created By : Ngọc 25/09/2021
        public bool ValidateRequired(Unit unit)
        {
            var isValid = true;
            var properties = unit.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var requiredProp = prop.GetCustomAttributes(typeof(MISARequired), true);

                if (requiredProp.Length > 0)
                {
                    var propName = (requiredProp[0] as MISARequired).Name;
                    var propValue = prop.GetValue(unit).ToString();
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
        /// Hàm kiểm tra trùng tên đơn vị tính
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        /// CreatedBy : Ngọc 18/8/2021
        public bool CheckNameExist(string unitName)
        {
            var isValid = _unitRepository.CheckNameExist(unitName);
            if (!isValid)
            {
                _serviceResult.Message = string.Format(Properties.ResourceVN.Duplicate_UnitName, unitName);
                _serviceResult.isValid = false;
            }
            return isValid;
        }
        #endregion
    }
}
