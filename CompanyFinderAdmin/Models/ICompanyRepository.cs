using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Models
{
    /// <summary>
    /// Interface for the company repository
    /// </summary>
   public interface ICompanyRepository
    {
        /// <summary>
        /// Method to get all the companies from the db
        /// </summary>
        /// <returns></returns>
        IEnumerable<Companies> GetAllCompanies();
        /// <summary>
        /// Method to get all the companies from the temp table in the db
        /// </summary>
        /// <returns></returns>
        IEnumerable<TemporaryCompanyTemplate> GetAllCompaniesFromTempTable();
        /// <summary>
        /// Saves the company data to the temporary company table
        /// </summary>
        /// <param name="temporaryCompany"></param>
        /// <returns></returns>
        Task SaveCompanyDataToTemporaryTable(TemporaryCompanyTemplate temporaryCompany);
        /// <summary>
        /// Get all the companies form the db via id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Companies GetCompanyById(int? id);
        /// <summary>
        /// Get all the skills from the db via id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SkillSet GetSkillById(int id);
        /// <summary>
        /// Get all the details from the db via id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SkillDetail GetDetailById(int id);
        /// <summary>
        /// Get the home page info from the db
        /// </summary>
        /// <returns></returns>
        HomePage GetHomePageInfo();
        /// <summary>
        /// Get all the Focuses from the db via id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Focus GetFocusById(int id);
        /// <summary>
        /// Get all the skills from the db
        /// </summary>
        /// <returns></returns>
        IEnumerable<SkillSet> GetAllSkills();
        /// <summary>
        /// Get all the deatils from the db
        /// </summary>
        /// <returns></returns>
        IEnumerable<SkillDetail> GetAllSkillDetails();
        /// <summary>
        /// Get all the focuses from the db
        /// </summary>
        /// <returns></returns>
        IEnumerable<Focus> GetAllFocuses();
        /// <summary>
        /// Method to create a company
        /// </summary>
        /// <param name="compToCreate"></param>
        /// <returns></returns>
        Task CreateCompany(Companies compToCreate);
        /// <summary>
        /// Method to edit a company
        /// </summary>
        /// <param name="compToEdit"></param>
        /// <returns></returns>
        Task EditCompany(Companies compToEdit);
        /// <summary>
        /// Method to delete a company
        /// </summary>
        /// <param name="selectedCompany"></param>
        /// <returns></returns>
        Task DeleteCompanyFromDb(Companies selectedCompany);
        /// <summary>
        /// Method to delete a skill
        /// </summary>
        /// <param name="selectedSkill"></param>
        /// <returns></returns>
        Task DeleteSkillFromDb(SkillSet selectedSkill);
        /// <summary>
        /// Method to delete a focus
        /// </summary>
        /// <param name="selectedFocus"></param>
        /// <returns></returns>
        Task DeleteFocusFromDb(Focus selectedFocus);
        /// <summary>
        /// Method to delete a detail 
        /// </summary>
        /// <param name="selectedDetail"></param>
        /// <returns></returns>
        Task DeleteDetailFromDb(SkillDetail selectedDetail);
    }
}
