using System;
using System.Linq;
using System.Text;
using SoftUni.Data;
using SoftUni.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main()
        {
            var softUniContext = new SoftUniContext();
            var currResult = GetLatestProjects(softUniContext);
            Console.WriteLine(currResult);
        }

        // 03. Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var result = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .ToList();

            foreach (var employee in employees)
            {
                result.AppendLine(
                    $"{employee.FirstName} " +
                    $"{employee.LastName} " +
                    $"{employee.MiddleName} " +
                    $"{employee.JobTitle} " +
                    $"{employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        // 04. Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var result = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Salary > 50_000)
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        // 05. Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var result = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary,
                    DepartmnetName = e.Department.Name,
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} " +
                    $"{employee.LastName} " +
                    $"from {employee.DepartmnetName} " +
                    $"- ${employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        // 06. Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var result = new StringBuilder();

            // create new address
            var newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAddress);
            context.SaveChanges();

            // take employee by lastname
            var currEmployee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            // set employee addressId to new address
            currEmployee.AddressId = newAddress.AddressId;
            context.SaveChanges();

            // get all empl adresses !
            var addresses = context.Employees
                .Select(a => new
                {
                    a.Address.AddressText,
                    a.Address.AddressId,
                })
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .ToList();

            foreach (var currAddress in addresses)
            {
                result.AppendLine(currAddress.AddressText);
            }

            return result.ToString().TrimEnd();
        }

        // 07. Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var result = new StringBuilder();

            var employees = context.Employees
                .Include(e => e.EmployeesProjects)
                .ThenInclude(e => e.Project)
                .Where(e => e.EmployeesProjects
                             .Any(p => p.Project.StartDate.Year >= 2001 &&
                                       p.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    EmployeeFirstName = e.FirstName,
                    EmployeeLastName = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        ProjectStartDate = p.Project.StartDate,
                        ProjectEndDate = p.Project.EndDate
                    })
                })
                .Take(10)
                .ToList();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.EmployeeFirstName} {employee.EmployeeLastName} - " +
                    $"Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    var endDate = project.ProjectEndDate.HasValue
                        ? project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                        : "not finished";

                    result.AppendLine($"--{project.ProjectName} - {project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDate}");
                }
            }

            return result.ToString().TrimEnd();
        }

        // 08. Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var result = new StringBuilder();

            var addresses = context.Addresses
                .Include(e => e.Employees)
                .ThenInclude(e => e.Address.Town)
                .Select(a => new
                {
                    AdressName = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmployeesCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AdressName)
                .Take(10)
                .ToList();

            foreach (var adress in addresses)
            {
                result.AppendLine($"{adress.AdressName}, {adress.TownName} - {adress.EmployeesCount} employees");
            }

            return result.ToString().TrimEnd();
        }

        // 09. Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            var result = new StringBuilder();

            var employee = context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    EmployeeProjectName = e.EmployeesProjects
                    .Select(p => new
                    {
                        ProjectName = p.Project.Name
                    })
                    .OrderBy(p => p.ProjectName)
                    .ToList()
                })
                .Where(e => e.EmployeeId == 147)
                .ToList();

            foreach (var emp in employee)
            {
                result.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");

                foreach (var project in emp.EmployeeProjectName)
                {
                    result.AppendLine(project.ProjectName);
                }
            }

            return result.ToString().TrimEnd();
        }

        // 10. Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var result = new StringBuilder();

            var employees = context.Departments
                .Select(e => new
                {
                    DepartmnetName = e.Name,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Employees = e.Employees
                    .Select(e => new
                    {
                        e.JobTitle,
                        EmployeeFirstName = e.FirstName,
                        EmployeeLastName = e.LastName
                    })
                })
                .Where(e => e.Employees.Count() > 5)
                .OrderBy(e => e.Employees.Count())
                .ThenBy(e => e.DepartmnetName)
                .ToList();

            foreach (var manager in employees)
            {
                result.AppendLine($"{manager.DepartmnetName} - {manager.ManagerFirstName} {manager.ManagerLastName}");

                foreach (var empl in manager.Employees)
                {
                    result.AppendLine($"{empl.EmployeeFirstName} {empl.EmployeeLastName} - {empl.JobTitle}");
                }
            }

            return result.ToString().TrimEnd();
        }

        // 11. Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            var result = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .ToList();

            foreach (var project in projects)
            {
                result.AppendLine(project.Name);
                result.AppendLine(project.Description);
                result.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }

            return result.ToString().Trim();
        }
    }
}

