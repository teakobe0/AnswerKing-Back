using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DAL.Tools
{
    /// <summary>
    ///     数据表转换类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DtObject<T> where T : new()
    {
        /// <summary>
        ///     将DataTable转换为实体列表
        /// </summary>
        /// <param name="dt">待转换的DataTable</param>
        /// <returns></returns>
        public List<T> ConvertToList(DataTable dt)
        {
            // 定义集合  
            var list = new List<T>();

            if (0 == dt.Rows.Count)
            {
                return list;
            }

            // 获得此模型的可写公共属性  
            IEnumerable<PropertyInfo> propertys = new T().GetType().GetProperties().Where(u => u.CanWrite);
            list = ConvertToEntity(dt, propertys);


            return list;
        }

        /// <summary>
        ///     将DataTable的首行转换为实体
        /// </summary>
        /// <param name="dt">待转换的DataTable</param>
        /// <returns></returns>
        public T ConvertToEntity(DataTable dt)
        {
            DataTable dtTable = dt.Clone();
            dtTable.Rows.Add(dt.Rows[0].ItemArray);
            return ConvertToList(dtTable)[0];
        }


        private List<T> ConvertToEntity(DataTable dt, IEnumerable<PropertyInfo> propertys)
        {
            var list = new List<T>();
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                var entity = new T();

                //遍历该对象的所有属性  
                foreach (PropertyInfo p in propertys)
                {
                    //将属性名称赋值给临时变量
                    string tmpName = p.Name;

                    //检查DataTable是否包含此列（列名==对象的属性名）    
                    if (!dt.Columns.Contains(tmpName)) continue;
                    //取值  
                    object value = Convert.ChangeType(dr[tmpName], p.PropertyType);
                    //如果非空，则赋给对象的属性  
                    if (value != DBNull.Value)
                    {
                        p.SetValue(entity, value, null);
                    }
                }
                //对象添加到泛型集合中  
                list.Add(entity);
            }
            return list;
        }
        public  DataTable ToDataTable(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            var result = CreateTable();
            FillData(result, entities);
            return result;
        }

        private static DataTable CreateTable()
        {
            var result = new DataTable();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var propertyType = property.PropertyType;
                if ((propertyType.IsGenericType) && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    propertyType = propertyType.GetGenericArguments()[0];
                result.Columns.Add(property.Name, propertyType);
            }
            return result;
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        private static void FillData(DataTable dt, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                dt.Rows.Add(CreateRow(dt, entity));
            }
        }

        /// <summary>
        /// 创建行
        /// </summary>
        private static DataRow CreateRow(DataTable dt, T entity)
        {
            DataRow row = dt.NewRow();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                row[property.Name] = property.GetValue(entity) ?? DBNull.Value;
            }
            return row;
        }
    }
}