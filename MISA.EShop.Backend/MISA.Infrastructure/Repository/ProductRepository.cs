using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Entities;
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
            _dbConnection = new MySqlConnection(_connectionString);
            _pagingResult = new PagingResult();
        }
        #endregion

        #region Methods
        public IEnumerable<Product> Add(Product product)
        {
            try
            {
                int rowEffects = 0;
                int count = 0;
                IEnumerable<Product> res = null;
                Guid productId = Guid.NewGuid();
                _dbConnection.Open();

                var transaction = _dbConnection.BeginTransaction();
                var dynamicParam = new DynamicParameters();

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
                res = _dbConnection.Query<Product>("Proc_InsertProduct", param: dynamicParam, transaction: transaction, commandType: CommandType.StoredProcedure);
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
                        res = _dbConnection.Query<Product>("Proc_InsertProduct", param: dynamicParam, transaction: transaction, commandType: CommandType.StoredProcedure);
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
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public IEnumerable<Product> Edit(Product product, Guid productId)
        {
            try
            {
                int rowEffects = 0;
                int count = 0;
                IEnumerable<Product> res = null;

                _dbConnection.Open();
                var transaction = _dbConnection.BeginTransaction();
                var dynamicParam = new DynamicParameters();
                var dynamicParamDetail = new DynamicParameters();
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
                res = _dbConnection.Query<Product>("Proc_UpdateProduct", param: dynamicParam, transaction: transaction, commandType: CommandType.StoredProcedure);
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
                            dynamicParamDetail.Add("ProductId", Guid.NewGuid());                          
                            res = _dbConnection.Query<Product>("Proc_InsertProduct", param: dynamicParamDetail, transaction: transaction, commandType: CommandType.StoredProcedure);
                            rowEffects++;
                            count++;
                        }
                        else if (flagMode == 1)
                        {
                            res = _dbConnection.Query<Product>("Proc_UpdateProduct", param: dynamicParamDetail, transaction: transaction, commandType: CommandType.StoredProcedure);
                            rowEffects++;
                            count++;
                        }
                        else
                        {
                            res = _dbConnection.Query<Product>("Proc_DeleteProduct", param: dynamicParamDetail, transaction: transaction, commandType: CommandType.StoredProcedure);
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
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public IEnumerable<Product> Delete(Guid productId)
        {
            try
            {
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$ProductId", productId);
                var rowEffects = _dbConnection.Query<Product>("Proc_DeleteProduct", param: dynamicParam, commandType: CommandType.StoredProcedure);
                return rowEffects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }



        public List<Product> GetAll()
        {
            try
            {
                var products = _dbConnection.Query<Product>("Proc_GetAllProduct", commandType: CommandType.StoredProcedure);
                return (List<Product>)products;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public List<Product> GetById(Guid productId)
        {
            try
            {
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$ProductId", productId);
                var products = _dbConnection.Query<Product>("Proc_GetProductById", param: dynamicParam, commandType: CommandType.StoredProcedure);
                return (List<Product>)products;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public PagingResult GetPagingResult(List<PagingParam> pagingParams, int pageSize, int pageNumber)
        {
            try
            {
                var dynamicParam = new DynamicParameters();
                dynamicParam.Add("$pageSize", pageSize);
                dynamicParam.Add("$pageIndex", pageNumber);

                var where = "1=1";
                foreach (var pagingParam in pagingParams)
                {
                    //Lấy giá trị tìm kiếm/lọc
                    var value = pagingParam.Value;
                    int operatorIndex = pagingParam.OperatorIndex;
                    var field = pagingParam.Field;
                    if (field == "SKUCode" || field == "ProductName" || field == "ProductGroupName" || field == "UnitName")
                    {
                        switch (operatorIndex)
                        {
                            case 0:
                                where += $" AND {field} Like '%{value}%'";
                                break;
                            case 1:
                                where += $" AND {field} = {value}";
                                break;
                            case 2:
                                where += $" AND {field} Like '%{value}'";
                                break;
                            case 3:
                                where += $" AND {field} Like '{value}%'";
                                break;
                            case 4:
                                where += $" AND {field} Not Like '%{value}%'";
                                break;
                        }
                    }
                    else if (field == "SellingPrice")
                    {
                        var price = Convert.ToDouble(value);
                        switch (operatorIndex)
                        {
                            case 0:
                                where += $" AND {field} = {price}";
                                break;
                            case 1:
                                where += $" AND {field} < {price}";
                                break;
                            case 2:
                                where += $" AND {field} <= {price}";
                                break;
                            case 3:
                                where += $" AND {field} > {price}";
                                break;
                            case 4:
                                where += $" AND {field} >= {price}";
                                break;
                        }
                    }
                    else
                    {
                        var option = Convert.ToInt32(value);
                        where += $" AND {field} = {option}";
                    }
                }
                dynamicParam.Add("$where", where);
                dynamicParam.Add("$total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var products = _dbConnection.Query<Product>("Proc_PagingFilterSort", param: dynamicParam, commandType: CommandType.StoredProcedure);
                var totalRecord = dynamicParam.Get<int>("$total");
                _pagingResult.TotalRecord = totalRecord;
                _pagingResult.Products = (List<Product>)products;
                var totalPageNumber = Math.Ceiling((decimal)((decimal)totalRecord / (decimal)pageSize));
                _pagingResult.TotalPageNumber = (int)totalPageNumber;
                return _pagingResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public Product GetProductWithDetail(Guid productId)
        {
            try
            {
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
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

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
