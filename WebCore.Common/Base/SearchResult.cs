using System;
using System.Data;
using System.Linq;
using System.Reflection;
using WebCore.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace WebCore.Base
{
    public class SearchResult : IDisposable
    {
        private DataTable m_TableCachedResult;
        private OracleDataReader m_AdapDataReader;
        private OracleDataReader m_AdapDataReader2;
        public string SessionKey { get; set; }
        public string SearchKey { get; set; }
        public DateTime TimeSearch { get; set; }
        public DataTable CachedResult
        {
            get
            {
                return m_TableCachedResult;
            }
            set
            {
                m_TableCachedResult = value;
                if (m_TableCachedResult != null) IsBufferMode = true;
            }
        }
        public OracleDataReader DataReader
        {
            get
            {
                return m_AdapDataReader;
            }
            set
            {
                m_AdapDataReader = value;
                var field = typeof(OracleDataReader).GetField("m_rowSize", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    var rowSize = (long)field.GetValue(value);
                    if(rowSize > 0)
                    {
                        value.FetchSize = rowSize * CONSTANTS.MAX_ROWS_IN_BUFFER;
                    }
                }

                CachedResult = new DataTable("RESULT");
                var tblSchema = m_AdapDataReader.GetSchemaTable();
                if (tblSchema != null)
                {
                    foreach (DataRow rowSchema in tblSchema.Rows)
                    {
                        CachedResult.Columns.Add((string)rowSchema["ColumnName"], (Type)rowSchema["DataType"]);
                    }
                    if (m_AdapDataReader != null) IsBufferMode = false;                    
                }
            }
        }
        public OracleDataReader DataReader2
        {
            get
            {
                return m_AdapDataReader2;
            }
            set
            {
                m_AdapDataReader2 = value;

                var field = typeof(OracleDataReader).GetField("m_rowSize", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    var rowSize = (long)field.GetValue(value);
                    if (rowSize > 0)
                    {
                        value.FetchSize = rowSize * CONSTANTS.MAX_ROWS_IN_BUFFER;
                    }
                }

                var tblSchema = m_AdapDataReader2.GetSchemaTable();
                if (tblSchema != null)
                {
                    foreach (DataRow rowSchema in tblSchema.Rows)
                    {
                        if (!CachedResult.Columns.Contains((string)rowSchema["ColumnName"]))
                            CachedResult.Columns.Add((string)rowSchema["ColumnName"], (Type)rowSchema["DataType"]);
                    }
                    if (m_AdapDataReader2 != null) IsBufferMode = false;                    
                }
            }
        }
        public IDbConnection DBConnection { get; set; }
        public bool IsBufferMode { get; set; }

        public void BufferData(int highestRow)
        {
            if (!IsBufferMode)
            {
                var isStop = false;
                if (DataReader2 == null)
                {
                    while (CachedResult.Rows.Count < highestRow && !isStop)
                    {
                        isStop = true;
                        DataRow row = null;
                        if (DataReader != null && DataReader.Read())
                        {
                            row = CachedResult.NewRow();
                            CachedResult.Rows.Add(row);

                            var count = DataReader.FieldCount;
                            for (var i = 0; i < count; i++)
                            {
                                row[DataReader.GetName(i)] = DataReader.GetValue(i);
                            }
                            isStop = false;
                        }

                        if (DataReader2 != null && DataReader2.Read())
                        {
                            if (row == null)
                            {
                                row = CachedResult.NewRow();
                                CachedResult.Rows.Add(row);
                            }

                            var count = DataReader2.FieldCount;
                            for (var i = 0; i < count; i++)
                            {
                                row[DataReader2.GetName(i)] = DataReader2.GetValue(i);
                            }

                            isStop = false;
                        }
                    }
                }
                else
                { 
                    string primaryKey = null;
                    for (var i = 0; i < DataReader.FieldCount; i++)
                        {
                            for (var j = 0; j < DataReader2.FieldCount; j++)
                            {
                                if (DataReader.GetName(i) == DataReader2.GetName(j))
                                {
                                    primaryKey = DataReader.GetName(i);
                                }
                            }
                        }
                
                    while (CachedResult.Rows.Count < highestRow && !isStop)
                    {
                        isStop = true;
                        DataRow row = null;
                        if (DataReader != null && DataReader.Read())
                        {
                            row = CachedResult.NewRow();
                            CachedResult.Rows.Add(row);

                            var count = DataReader.FieldCount;
                            for (var i = 0; i < count; i++)
                            {
                                row[DataReader.GetName(i)] = DataReader.GetValue(i);
                            }
                            isStop = false;
                        }
                    }
                    DataTable dt1 = CachedResult.Copy();
                    CachedResult.Clear();
                    isStop = false;
                    while (CachedResult.Rows.Count < highestRow && !isStop)
                    {
                        isStop = true;
                        DataRow row = null;
                        if (DataReader2 != null && DataReader2.Read())
                        {
                            if (row == null)
                            {
                                row = CachedResult.NewRow();
                                CachedResult.Rows.Add(row);
                            }

                            var count = DataReader2.FieldCount;
                            for (var i = 0; i < count; i++)
                            {
                                row[DataReader2.GetName(i)] = DataReader2.GetValue(i);
                            }

                            isStop = false;
                        }
                    }
                    DataTable dt2 = CachedResult.Copy();
                    CachedResult.Clear();
                    var tables = new DataTable[] { dt1, dt2 };
                    CachedResult = MergeAll(tables.ToList(), primaryKey);
                }
            }
        }
        public static DataTable MergeAll(List<DataTable> tables, String primaryKeyColumn)
        {
            if (!tables.Any())
                throw new ArgumentException("Tables must not be empty", "tables");
            if (primaryKeyColumn != null)
                foreach (DataTable t in tables)
                    if (!t.Columns.Contains(primaryKeyColumn))
                        throw new ArgumentException("All tables must have the specified primarykey column " + primaryKeyColumn, "primaryKeyColumn");

            if (tables.Count == 1)
                return tables[0];

            DataTable table = new DataTable("TblUnion");
            table.BeginLoadData(); // Turns off notifications, index maintenance, and constraints while loading data
            foreach (DataTable t in tables)
            {
                table.Merge(t); // same as table.Merge(t, false, MissingSchemaAction.Add);
            }
            table.EndLoadData();

            if (primaryKeyColumn != null)
            {
                // since we might have no real primary keys defined, the rows now might have repeating fields
                // so now we're going to "join" these rows ...
                //var pkGroups = table.AsEnumerable()
                //    .GroupBy(r => r[primaryKeyColumn]);
                //var dupGroups = pkGroups.Where(g => g.Count() > 1);
                //foreach (var grpDup in dupGroups)
                //{
                //    // use first row and modify it
                //    DataRow firstRow = grpDup.First();
                //    foreach (DataColumn c in table.Columns)
                //    {
                //        if (firstRow.IsNull(c))
                //        {
                //            DataRow firstNotNullRow = grpDup.Skip(1).FirstOrDefault(r => !r.IsNull(c));
                //            if (firstNotNullRow != null)
                //                firstRow[c] = firstNotNullRow[c];
                //        }
                //    }
                //    // remove all but first row
                //    var rowsToRemove = grpDup.Skip(1);
                //    foreach (DataRow rowToRemove in rowsToRemove)
                //        table.Rows.Remove(rowToRemove);
                //}
            }

            return table;
        }
        public DataTable GetSearchResult(int startRow, int rowCount)
        {
            //return CachedResult.Rows.OfType<DataRow>()
            //    .Skip(startRow)
            //    .Take(rowCount)                
            //    .CopyToDataTable();
            return null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            SearchKey = null;
            if (IsBufferMode)
                CachedResult.Dispose();
            else
            {
                DBConnection.Dispose();
                DataReader.Dispose();
                if (DataReader2 != null) DataReader2.Dispose();
            }
        }

        #endregion
    }
}
