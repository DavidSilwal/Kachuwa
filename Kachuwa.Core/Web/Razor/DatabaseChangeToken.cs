using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Primitives;

namespace Kachuwa.Web.Razor
{
    public class DatabaseChangeToken : IChangeToken
    {
        private string _connection;
        private string _viewPath;

        public DatabaseChangeToken(string connection, string viewPath)
        {
            _connection = connection;
            _viewPath = viewPath;
        }

        public bool ActiveChangeCallbacks => false;

        public bool HasChanged
        {
            get
            {

                var query = "SELECT LastRequested, LastModified FROM ViewContent WHERE Location = @Path;";
                try
                {
                    using (var conn = new SqlConnection(_connection))
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Path", _viewPath);
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                if (reader["LastRequested"] == DBNull.Value)
                                {
                                    return false;
                                }
                                else
                                {
                                    return Convert.ToDateTime(reader["LastModified"]) > Convert.ToDateTime(reader["LastRequested"]);
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => EmptyDisposable.Instance;
    }
}