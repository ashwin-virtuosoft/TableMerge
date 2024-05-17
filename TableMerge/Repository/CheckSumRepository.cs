using System.Data.SqlClient;
using TableMerge.Models;
using TableMerge.Services;

namespace TableMerge.Repository
{
    public class CheckSumRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CheckSumRepository> _logger;
        public CheckSumRepository(IConfiguration configuration, ILogger<CheckSumRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }
        public void InsertPerson(Person person)
        {
            string connectionString = _configuration.GetConnectionString("MyDB");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                 person.RowCheckSum = RowCheckSum.GenerateChecksum(person.FirstName + person.LastName + person.BirthDate);
                 string query = "INSERT INTO Person (FirstName, LastName, BirthDate, RowCheckSum) VALUES (@FirstName, @LastName, @BirthDate, @RowCheckSum)";

                 using(SqlCommand command=new SqlCommand(query, connection))
                 {
                    command.Parameters.AddWithValue("@FirstName", person.FirstName);
                    command.Parameters.AddWithValue("@LastName", person.LastName);
                    command.Parameters.AddWithValue("@BirthDate", person.BirthDate);
                    command.Parameters.AddWithValue("@RowCheckSum", person.RowCheckSum);

                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool CompareChecksum(int personId, string firstName, string lastName, string birthDate)
        {
            string connectionString = _configuration.GetConnectionString("MyDB");
            string calculatedChecksum = RowCheckSum.GenerateChecksum(firstName + lastName + birthDate);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT RowCheckSum FROM Person WHERE PersonID = @PersonID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

                    connection.Open();
                    string storedChecksum = (string)command.ExecuteScalar();

                    _logger.LogInformation("Calculated Checksum: {CalculatedChecksum}", calculatedChecksum);
                    _logger.LogInformation("Stored Checksum: {StoredChecksum}", storedChecksum);

                    return calculatedChecksum == storedChecksum;
                }
            }
        }
    }
}
