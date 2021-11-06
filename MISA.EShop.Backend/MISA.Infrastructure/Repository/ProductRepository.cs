using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Entities;
using MISA.Core.Enumeration;
using MISA.Core.Interfaces.Repository;
using MISA.Core.MISAAttribute;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MISA.Core.Enumeration.MISAEnum;

namespace MISA.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        #region Declare
        IDbConnection _dbConnection;
        public readonly string _connectionString;
        //ServiceResult _serviceResult;
        PagingResult _pagingResult;
        #endregion

        #region Constructor
        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LocalDatabase");      
            _pagingResult = new PagingResult();
        }
        #endregion

        #region Methods
        public int Add(Product product, Picture picture)
        {
            var rowEffects = 0;
            IDbTransaction transaction = null; 
            try
            {
                int count = 0;
                int res = 0;
                Guid productId = Guid.NewGuid();

                _dbConnection = new MySqlConnection(_connectionString);
                _dbConnection.Open();
                transaction = _dbConnection.BeginTransaction();

                var dynamicParam = new DynamicParameters();
                // Thêm mới ảnh
                var pictureParam = new DynamicParameters();
                if (picture.PictureId != new Guid())
                {
                    pictureParam.Add("$PictureId", picture.PictureId);
                    pictureParam.Add("$PictureName", picture.PictureName);
                    pictureParam.Add("$PictureTail", picture.PictureTail);                   
                    pictureParam.Add("$PicturePath", picture.PicturePath);
                    pictureParam.Add("$Type", picture.Type);
                    int insertPicture = _dbConnection.Execute("Proc_InsertPicture", param: pictureParam, transaction: transaction, commandType: CommandType.StoredProcedure);
                }

                var properties = product.GetType().GetProperties();
                // duyệt từng thuộc tính
                foreach (var prop in properties)
                {
                    // kiểm tra có thuộc tính nào là not map không
                    var notMapProp = prop.GetCustomAttributes(typeof(MISANotMap), true);

                    // các thuộc tính là not map sẽ ko dc thêm vào database
                    if (notMapProp.Length == 0)
                    {
                        //Lấy tên thuộc tính
                        var propName = prop.Name;
                        //Lấy giá trị thuộc tính
                        var propValue = prop.GetValue(product);

                        if (propName == "ProductId")
                        {
                            propValue = Guid.NewGuid();
                            productId = (Guid)propValue;

                        }
                        dynamicParam.Add($"${propName}", propValue);

                    }
                }
                res = _dbConnection.Execute("Proc_InsertProduct", param: dynamicParam, transaction: transaction, commandType: CommandType.StoredProcedure);
                rowEffects++;

                if (product?.ProductDetails?.Count > 0)
                {
                    List<Product> productDetails = product.ProductDetails;
                    for (int i = 0; i < productDetails.Count; i++)
                    {
                        var productDetail = productDetails[i].GetType().GetProperties();
                        foreach (var propDetail in productDetail)
                        {
                            // kiểm tra có thuộc tính nào là not map không
                            var notMapPropDetail = propDetail.GetCustomAttributes(typeof(MISANotMap), true);

                            // các thuộc tính là not map sẽ ko dc thêm vào database
                            if (notMapPropDetail.Length == 0)
                            {
                                //Lấy tên thuộc tính
                                var propName = propDetail.Name;
                                //Lấy giá trị thuộc tính
                                var propValue = propDetail.GetValue(productDetails[i]);

                                if (propName == "ProductId")
                                {
                                    propValue = Guid.NewGuid();
                                }

                                if (propName == "ParentId")
                                {
                                    propValue = productId;
                                }
                                dynamicParam.Add($"${propName}", propValue);

                            }
                        }
                        res = _dbConnection.Execute("Proc_InsertProduct", param: dynamicParam, transaction: transaction, commandType: CommandType.StoredProcedure);
                        rowEffects++;
                        count++;
                    }
                }

                if (rowEffects == count + 1)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
               
                
            }
            catch (Exception ex)
            {
                rowEffects = 0;
                Console.WriteLine(ex);
            }
            finally{
                transaction?.Dispose();
                _dbConnection?.Dispose();
            }
            return rowEffects;
        }

        public int Edit(Product product, Guid productId, Picture picture)
        {
            int rowEffects = 0;
            IDbTransaction transaction = null;
            try
            {
                rowEffects = 0;
                int count = 0;
                int res = 0;

                _dbConnection = new MySqlConnection(_connectionString);
                _dbConnection.Open();
                transaction = _dbConnection.BeginTransaction();
                var dynamicParam = new DynamicParameters();
                var dynamicParamDetail = new DynamicParameters();
                var pictureParam = new DynamicParameters();
                // thêm mới ảnh
                if (picture.PictureId != new Guid())
                {
                    pictureParam.Add("$PictureId", picture.PictureId);
                    pictureParam.Add("$PictureName", picture.PictureName);
                    pictureParam.Add("$PictureTail", picture.PictureTail);
                    pictureParam.Add("$PicturePath", picture.PicturePath);
                    pictureParam.Add("$Type", picture.Type);
                    int insertPicture = _dbConnection.Execute("Proc_InsertPicture", param: pictureParam, transaction: transaction, commandType: CommandType.StoredProcedure);
                }

                var properties = product.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    // kiểm tra có thuộc tính nào là not map không
                    var notMapProp = prop.GetCustomAttributes(typeof(MISANotMap), true);

                    // các thuộc tính là not map sẽ ko dc thêm vào database
                    if (notMapProp.Length == 0)
                    {
                        //Lấy tên thuộc tính
                        var propName = prop.Name;
                        //Lấy giá trị thuộc tính
                        var propValue = prop.GetValue(product);

                        dynamicParam.Add($"${propName}", propValue);
                    }
                }
                res = _dbConnection.Execute("Proc_UpdateProduct", param: dynamicParam, transaction: transaction, commandType: CommandType.StoredProcedure);
                rowEffects++;

                int flagMode = -1;
                if (product?.ProductDetails?.Count > 0)
                {
                    List<Product> productDetails = product.ProductDetails;
                    for (int i = 0; i < productDetails.Count; i++)
                    {
                        var productDetail = productDetails[i].GetType().GetProperties();
                        foreach (var propDetail in productDetail)
                        {
                            // kiểm tra có thuộc tính nào là not map không
                            var notMapPropDetail = propDetail.GetCustomAttributes(typeof(MISANotMap), true);

                            // các thuộc tính là not map sẽ ko dc thêm vào database
                            if (notMapPropDetail.Length == 0)
                            {
                                //Lấy tên thuộc tính
                                var propName = propDetail.Name;
                                //Lấy giá trị thuộc tính
                                var propValue = propDetail.GetValue(productDetails[i]);

                                dynamicParamDetail.Add($"${propName}", propValue);
                            }
                            else
                            {
                                var propName = (notMapPropDetail[0] as MISANotMap).Name;
                                if (propName == "Cờ")
                                {
                                    flagMode = (int)propDetail.GetValue(productDetails[i]);
                                }
                            }
                        }

                        if (flagMode == 0)
                        {
                            dynamicParamDetail.Add("$ProductId", Guid.NewGuid());
                            res = _dbConnection.Execute("Proc_InsertProduct", param: dynamicParamDetail, transaction: transaction, commandType: CommandType.StoredProcedure);
                            rowEffects++;
                            count++;
                        }
                        else if (flagMode == 1)
                        {
                            res = _dbConnection.Execute("Proc_UpdateProduct", param: dynamicParamDetail, transaction: transaction, commandType: CommandType.StoredProcedure);
                            rowEffects++;
                            count++;
                        }
                        else
                        {
                            res = _dbConnection.Execute("Proc_DeleteProduct", param: dynamicParamDetail, transaction: transaction, commandType: CommandType.StoredProcedure);
                            rowEffects++;
                            count++;
                        }

                    }
                }

                if (rowEffects == count + 1)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }          
                
            }
            catch (Exception ex)
            {
                rowEffects = 0;
                Console.WriteLine(ex);
            }
            finally
            {
                transaction?.Dispose();
                _dbConnection?.Dispose();
            }
            return rowEffects;
        }

        public int Delete(Guid productId)
        {
            int rowEffects = 0;
            try
            {
                _dbConnection = new MySqlConnection(_connectionString);
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$ProductId", productId);
                rowEffects = _dbConnection.Execute("Proc_DeleteProduct", param: dynamicParam, commandType: CommandType.StoredProcedure);              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                rowEffects = 0;
            }
            finally
            {
                _dbConnection?.Dispose();
            }

            return rowEffects;
        }

        public List<Product> GetAll()
        {
            List<Product> products = null;
            try
            {
                _dbConnection = new MySqlConnection(_connectionString);
                products = (List<Product>)_dbConnection.Query<Product>("Proc_GetAllProduct", commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                products = null;
            }
            finally
            {
                _dbConnection?.Dispose();
            }

            return products;
        }

        public List<Product> GetById(Guid productId)
        {
            List<Product> products = null;
            try
            {
                _dbConnection = new MySqlConnection(_connectionString);
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$ProductId", productId);
                products = (List<Product>)_dbConnection.Query<Product>("Proc_GetProductById", param: dynamicParam, commandType: CommandType.StoredProcedure);
                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                products = null;
            }
            finally
            {
                _dbConnection?.Dispose();
            }

            return products;
        }

        public PagingResult GetPagingResult(List<PagingParam> pagingParams, int pageSize, int pageNumber)
        {
            try
            {
                _dbConnection = new MySqlConnection(_connectionString);
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$pageSize", pageSize);
                dynamicParam.Add("$pageIndex", pageNumber);

                var where = "1=1 AND ParentId IS NULL";
                foreach (var pagingParam in pagingParams)
                {
                    //Lấy giá trị tìm kiếm/lọc
                    var value = pagingParam.Value;
                    int? operatorIndex = pagingParam.OperatorIndex;
                    var field = pagingParam.Field;
                    if (field == "SKUCode" || field == "ProductName" || field == "ProductGroupName" || field == "UnitName")
                    {
                        switch (operatorIndex)
                        {
                            case (int)MISAEnum.MISAStringOperator.Contain:
                                where += $" AND {field} Like '%{value}%'";
                                break;
                            case (int)MISAEnum.MISAStringOperator.Equal:
                                where += $" AND {field} = {value}";
                                break;
                            case (int)MISAEnum.MISAStringOperator.StartWith:
                                //Bắt đầu bằng
                                where += $" AND {field} Like '{value}%'";
                                break;
                            case (int)MISAEnum.MISAStringOperator.EndWith:
                                //Kết thúc bằng
                                where += $" AND {field} Like '%{value}'";
                                break;
                            case (int)MISAEnum.MISAStringOperator.NotContain:
                                //Không chứa
                                where += $" AND {field} Not Like '%{value}%'";
                                break;
                            default: break;
                        }
                    }
                    else if (field == "SellingPrice")
                    {
                        var price = Convert.ToDouble(value);
                        switch (operatorIndex)
                        {
                            case (int)MISAEnum.MISASNumberOperator.Equal:
                                where += $" AND {field} = {price}";
                                break;
                            case (int)MISAEnum.MISASNumberOperator.Lower:
                                where += $" AND {field} < {price}";
                                break;
                            case (int)MISAEnum.MISASNumberOperator.LowerOrEqual:
                                where += $" AND {field} <= {price}";
                                break;
                            case (int)MISAEnum.MISASNumberOperator.Greater:
                                where += $" AND {field} > {price}";
                                break;
                            case (int)MISAEnum.MISASNumberOperator.GreaterOrEqual:
                                where += $" AND {field} >= {price}";
                                break;
                            default: break;
                        }
                    }
                    else if (field == "Display" || field == "StatusBusiness")
                    {
                        var option = Convert.ToInt32(operatorIndex);
                        if (option != 2)
                        {
                            where += $" AND {field} = {option}";
                        }
                    }
                }
                dynamicParam.Add("$where", where);
                dynamicParam.Add("$sort", "CreateDate");
                dynamicParam.Add("$total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var products = _dbConnection.Query<Product>("Proc_PagingFilterSort", param: dynamicParam, commandType: CommandType.StoredProcedure);
                var totalRecord = dynamicParam.Get<int>("$total");
                _pagingResult.TotalRecord = totalRecord;
                _pagingResult.Products = (List<Product>)products;
                var totalPageNumber = Math.Ceiling((decimal)((decimal)totalRecord / (decimal)pageSize));
                _pagingResult.TotalPageNumber = (int)totalPageNumber;            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _pagingResult = null;
            }
            finally
            {
                _dbConnection?.Dispose();
            }

            return _pagingResult;
        }

        public bool CheckCodeExist(Guid productId, string SKUCode)
        {
            try
            {
                _dbConnection = new MySqlConnection(_connectionString);
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$SKUCode", SKUCode);
                Product product = _dbConnection.QueryFirstOrDefault<Product>("Proc_GetProductBySKUCode", param: dynamicParam, commandType: CommandType.StoredProcedure);
                if (product != null)
                {
                    if (productId == Guid.Empty)
                    {
                        return false;
                    }
                    else
                    {
                        if (product.SKUCode != null && product.ProductId != productId)
                        {
                            return false;
                        }
                    }
                }
                _dbConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _dbConnection.Dispose();
                return false;
            }
        }

        public bool ValidateCode(Guid productId, string productCode)
        {
            try
            {
                _dbConnection = new MySqlConnection(_connectionString);
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$ProductCode", productCode);
                Product product = _dbConnection.QueryFirstOrDefault<Product>("Proc_GetProductByProductCode", param: dynamicParam, commandType: CommandType.StoredProcedure);
                if (product != null)
                {
                    if (productId == Guid.Empty)
                    {
                        return false;
                    }
                    else
                    {
                        if (product.ProductCode != null && product.ProductId != productId)
                        {
                            return false;
                        }
                    }
                }
                _dbConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _dbConnection.Dispose();
                return false;
            }
        }

        public string AutoGenSKUCode(string productName)
        {
            try
            {
                _dbConnection = new MySqlConnection(_connectionString);
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$ProductName", productName);
                var skuCode = _dbConnection.QueryFirstOrDefault<string>("Proc_GetSKUCodeByName", param: dynamicParam, commandType: CommandType.StoredProcedure);
                _dbConnection.Dispose();
                return skuCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _dbConnection.Dispose();
                return null;
            }
        }

        public Product GetProductWithDetail(Guid productId)
        {
            try
            {
                _dbConnection = new MySqlConnection(_connectionString);
                Product product = null;
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$ProductId", productId);
                var reader = _dbConnection.ExecuteReader("Proc_GetProductWithDetailById", param: dynamicParam, commandType: CommandType.StoredProcedure);
                var dataset = ConvertDataReaderToDataSet(reader);
                if (dataset?.Tables?.Count > 0)
                {
                    var dtProduct = dataset.Tables[0];
                    var dtProductDetails = dataset.Tables[1];
                    product = ConvertDataTableToList<Product>(dtProduct).SingleOrDefault();
                    var productDetails = ConvertDataTableToList<Product>(dtProductDetails).ToList();
                    if (product != null)
                    {
                        product.ProductDetails = productDetails;
                    }
                }
                _dbConnection.Dispose();
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _dbConnection.Dispose();
                return null;
            }
        }

        /// <summary>
        /// Hàm chuyển dữ liệu table trong dataset về 1 list object   
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        /// Created By: Ngọc 01/10/2021
        private IEnumerable<T> ConvertDataTableToList<T>(DataTable dataTable)
        {
            var result = new List<T>();
            if (dataTable?.Rows?.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    T item = GetItem<T>(row);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Hàm lấy từng thuộc tính của cột trong hàng để đóng gói thành thực thể  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        /// Created By: Ngọc 01/10/2021
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && dr[column.ColumnName] != DBNull.Value)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        /// <summary>
        /// Chuyển dữ liệu về mảng các bảng 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// Created By: Ngọc 01/10/2021
        private DataSet ConvertDataReaderToDataSet(IDataReader data)
        {
            DataSet ds = new DataSet();
            int i = 0;
            while (!data.IsClosed)
            {
                ds.Tables.Add("Table" + (i + 1));
                ds.EnforceConstraints = false;
                ds.Tables[i].Load(data);
                i++;
            }
            return ds;
        }
        #endregion
    }
}
