using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCore.Helper
{
    public static class SQLiteHelper
    {
        public static SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source='D:\\Others\\Vue\\xpad_core-develop\\xPad_Core\\Helper\\stores.db" + "';Version=3;");

        public static void SaveLoginInfo(string id, string login)
        {
            DataSet ds = ExecuteDataset("SELECT UserLoginID FROM Users u where u.UserLoginID = '" + id + "'");

            if (ds.Tables[0].Rows.Count == 0)
            {
                ExecuteNonQuery(
                    @"insert into [Users] (UserLoginID, UserName, CreatedTM, UpdatedTM) 
                                values( '" + id + "', '" + login + "', datetime('now', 'localtime'), datetime('now', 'localtime'))");
            }
            else
            {
                //ExecuteNonQuery(@"UPDATE [Users] SET UserName='" + login + "',UpdatedTM=datetime('now', 'localtime') WHERE UserLoginID='" + id + "'");
            }
        }
        public static DataSet GetHotStoreByUser()
        {
            DataSet ds = ExecuteDataset(@"select B.UserLoginID, B.UserName, B.NearestStoreTitle, B.NearestStoreAddress, B.StoreSreachedCount from (
	                                        select 	                                        (row_number() over (partition by A.UserLoginID order by A.StoreSreachedCount desc)) as rn,
	                                        A.UserLoginID, A.UserName, A.NearestStoreTitle,A.NearestStoreAddress, A.StoreSreachedCount
	                                        from (
		                                        select u.UserLoginID, u.UserName, r.NearestStoreTitle, r.NearestStoreAddress, count(*) as StoreSreachedCount
		                                        from SearchResult r, Users u
		                                        where r.UserLoginID=u.UserLoginID
		                                        GROUP BY u.UserLoginID, u.UserName, r.NearestStoreTitle, r.NearestStoreAddress
	                                        ) as A 
                                        ) B where B.rn=1");
            return ds;
        }
        public static void SaveSearchResule(Store store)
        {
            if (store.user == "") return;

            ExecuteNonQuery(
                    @"INSERT INTO SearchResult(UserLoginID,NearestStoreAddress,NearestStorelat,NearestStorelng,NearestStoreTitle, CreatedTM) 
			VALUES('" + store.user + "','" + store.address + "','" + store.lat + "','" + store.lng + "','" + store.title + "', datetime('now','localtime'))");
        }
        public static DataSet ExecuteDataset(string sql)
        {
            DataSet ds = new DataSet();

            using (SQLiteCommand cmd = m_dbConnection.CreateCommand())
            {
                cmd.CommandText = sql;

                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();

                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(ds);
                da.Dispose();
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return ds;
        }
        public static void ExecuteNonQuery(string sql)
        {
            using (SQLiteCommand cmd = m_dbConnection.CreateCommand())
            {
                cmd.CommandText = sql;

                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();

                cmd.ExecuteNonQuery();

                cmd.Connection.Close();
                cmd.Dispose();
            }

        }
    }
}
