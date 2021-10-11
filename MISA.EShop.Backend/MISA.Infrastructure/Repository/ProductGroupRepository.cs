using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        #region Declare
        IDbConnection _dbConnection;
        public readonly string _connectionString;
        //ServiceResult _serviceResult;
        #endregion


        #region Constructor
        public ProductGroupRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LocalDatabase");
            _dbConnection = new MySqlConnection(_connectionString);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thêm mới đơn vi tính
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        public int Add(ProductGroup productGroup)
        {
            try
            {
                var dynamicParam = new DynamicParameters();
                var properties = productGroup.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    //Lấy tên thuộc tính
                    var propName = prop.Name;
                    //Lấy giá trị thuộc tính
                    var propValue = prop.GetValue(productGroup);

                    if (propName == "ProductGroupId")
                    {
                        propValue = Guid.NewGuid();
                    }

                    dynamicParam.Add($"${propName}", propValue);
                }
                var rowEffects = _dbConnection.Execute("Proc_InsertProductGroup", param: dynamicParam, commandType: CommandType.StoredProcedure);
                return rowEffects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public List<ProductGroup> GetAll()
        {
            try
            {
                var productGroups = _dbConnection.Query<ProductGroup>("Proc_GetAllProductGroup", commandType: CommandType.StoredProcedure);
                return (List<ProductGroup>)productGroups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public ProductGroup GetProductGroupByName(string productGroupName)
        {
            {
                try
                {
                    var dynamicParam = new DynamicParameters();
                    dynamicParam.Add("$ProductGroupName", productGroupName);
                    var productGroup = _dbConnection.QueryFirstOrDefault<ProductGroup>("Proc_GetProductGroupByName", param: dynamicParam, commandType: CommandType.StoredProcedure);
                    return productGroup;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        public bool CheckNameExist(string productGroupName)
        {
            try
            {
                var productGroup = GetProductGroupByName(productGroupName);
                if (productGroup != null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        #endregion
    }
}
