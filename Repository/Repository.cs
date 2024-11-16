using MongoDB.Driver;
using WebApplication1.Model;

namespace WebApplication1.Repository
{
    public class Repo
    {
        private readonly IMongoCollection<Employee> _employeeCollection;
        public Repo(IMongoDatabase database)
        {
            _employeeCollection = database.GetCollection<Employee>("MyNewCollection");
        }
        // Create (Check existence before insertion)
        public async Task CreateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _employeeCollection.Find(e => e.Id == employee.Id).FirstOrDefaultAsync();
            if (existingEmployee != null)
            {
                throw new ArgumentNullException($"An employee with ID {employee.Id} already exists.");
            }
            await _employeeCollection.InsertOneAsync(employee);
        }

        // Read (Get by ID with existence check)
        public async Task<Employee> GetEmployeeByIdAsync(long id)
        {
            var employee = await _employeeCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
            if (employee == null)
            {
                throw new ArgumentNullException($"No employee found with ID {id}.");
            }
            return employee;
        }

        // Read (Get All with existence check)
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var employees = await _employeeCollection.Find(_ => true).ToListAsync();
            if (employees == null || employees.Count == 0)
            {
                throw new ArgumentNullException("No employees found in the database.");
            }
            return employees;
        }

        // Update (Check existence before updating)
        public async Task UpdateEmployeeAsync(long id, Employee updatedEmployee)
        {
            var existingEmployee = await _employeeCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
            if (existingEmployee == null)
            {
                throw new ArgumentNullException($"No employee found with ID {id}.");
            }
            await _employeeCollection.ReplaceOneAsync(e => e.Id == id, updatedEmployee);
        }

        // Delete (Check existence before deletion)
        public async Task DeleteEmployeeAsync(long id)
        {
            var existingEmployee = await _employeeCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
            if (existingEmployee == null)
            {
                throw new ArgumentNullException($"No employee found with ID {id}.");
            }
            await _employeeCollection.DeleteOneAsync(e => e.Id == id);
        }

    }
}
