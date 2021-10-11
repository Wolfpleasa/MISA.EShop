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
    public class UnitRepository : IUnitRepository
    {
        #region Declare
        IDbConnection _dbConnection;
        public readonly string _connectionString;
        //ServiceResult _serviceResult;
        #endregion


        #region Constructor
        public UnitRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LocalDatabase");
            _dbConnection = new MySqlConnection(_connectionString);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thêm mới đơn vi tính
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        /// Created By: Ngọc 25/09/2021
        public int Add(Unit unit)
        {
            try
            {
                var dynamicParam = new DynamicParameters();
                var properties = unit.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    //Lấy tên thuộc tính
                    var propName = prop.Name;
                    //Lấy giá trị thuộc tính
                    var propValue = prop.GetValue(unit);

                    if (propName == "UnitId")
                    {
                        propValue = Guid.NewGuid();
                    }

                    dynamicParam.Add($"${propName}", propValue);
                }
                var rowEffects = _dbConnection.Execute("Proc_InsertUnit", param: dynamicParam, commandType: CommandType.StoredProcedure);
                return rowEffects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public List<Unit> GetAll()
        {
            try
            {
                var units = _dbConnection.Query<Unit>("Proc_GetAllUnit", commandType: CommandType.StoredProcedure);
                return (List<Unit>)units;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public Unit GetUnitByName(string unitName)
        {
            {
                try
                {               
                    var dynamicParam = new DynamicParameters();
                    dynamicParam.Add("$UnitName", unitName);
                    var unit = _dbConnection.QueryFirstOrDefault<Unit>("Proc_GetUnitByName", param: dynamicParam, commandType: CommandType.StoredProcedure);                
                    return unit;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        public bool CheckNameExist(string unitName)
        {
            try
            {
                var unit = GetUnitByName(unitName);
                if (unit != null)
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
