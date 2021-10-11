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
    public class PictureRepository : IPictureRepository
    {
        #region Declare
        IDbConnection _dbConnection;
        public readonly string _connectionString;
        #endregion

        #region Constructor
        public PictureRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LocalDatabase");
            _dbConnection = new MySqlConnection(_connectionString);
        }
        #endregion

        #region Methods
        public Picture GetById(Guid pictureId)
        {
            try
            {
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$PictureId", pictureId);
                var picture = _dbConnection.QueryFirstOrDefault<Picture>("Proc_GetPictureById", param: dynamicParam, commandType: CommandType.StoredProcedure);
                return picture;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        #endregion
    }
}
