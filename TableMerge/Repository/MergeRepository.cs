using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TableMerge.Repository
{
    public class MergeRepository:IMergeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MergeRepository> _logger;

        public MergeRepository(IConfiguration configuration, ILogger<MergeRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> MergeStudentData(int id, string name)
        {
            string connectionString = _configuration.GetConnectionString("MyDB");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MergeStudentDataTable", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Name", name);

                    try
                    {
                        await connection.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return "Merge operation completed successfully.";
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError(ex, "An error occurred while executing the merge operation.");
                        return $"Internal server error: {ex.Message}";
                    }
                }
            }
        }
    }
}
