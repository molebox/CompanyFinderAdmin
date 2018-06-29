using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyFinderAdmin.Models
{
    /// <summary>
    /// Repository for the company database
    /// </summary>
    public class CompanyRepository : ICompanyRepository
    {

        private CompanyDbContext _context;

        /// <summary>
        /// Company repo constructor which injects the db context for use in the repo
        /// </summary>
        /// <param name="context"></param>
        public CompanyRepository(CompanyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all the companies from the db
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TemporaryCompanyTemplate> GetAllCompaniesFromTempTable() => _context.TemporaryCompanyTemplate;

        /// <summary>
        /// Saves the company data to the temporary company table
        /// </summary>
        /// <param name="temporaryCompany"></param>
        /// <returns></returns>
        public async Task SaveCompanyDataToTemporaryTable(TemporaryCompanyTemplate temporaryCompany)
        {
            try
            {
                _context.Add(temporaryCompany);
                           

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);            
            }
        }

        /// <summary>
        /// Creates a new company in the companies table in the db
        /// </summary>
        /// <param name="compToCreate"></param>
        /// <returns></returns>
        public async Task CreateCompany(Companies compToCreate)
        {
            try
            {
                _context.Add(compToCreate);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);             
            }

        }

        /// <summary>
        /// Delete the company from the db. The companies child elemets are first deleted from the CompanySkills and CompanyDetails tables.
        /// Then the company is deleted from the companies table.
        /// </summary>
        /// <param name="selectedCompany"></param>
        /// <returns></returns>
        public async Task DeleteCompanyFromDb(Companies selectedCompany)
        {
            try
            {
                // Delete the child entities first
                var tempCompSkills = _context.CompanySkills.Where(c => c.CompanyId == selectedCompany.CompanyId).ToList();
                var tempCompDetails = _context.CompanyDetails.Where(c => c.CompanyId == selectedCompany.CompanyId).ToList();

                foreach (var skill in tempCompSkills)
                {
                    _context.CompanySkills.RemoveRange(skill);
                }
                foreach (var detail in tempCompDetails)
                {
                    _context.CompanyDetails.RemoveRange(detail);
                }

                // Then remove the company from the companies table
                _context.Companies.Remove(selectedCompany);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        /// <summary>
        /// Delete the detail from the db. The details are found via their ids and deleted from the ComapnyDetails table. They are then delete from the SkillDetail table
        /// </summary>
        /// <param name="selectedDetail"></param>
        /// <returns></returns>
        public async Task DeleteDetailFromDb(SkillDetail selectedDetail)
        {
            try
            {
                var tempDetail = _context.CompanyDetails.Where(d => d.SkillDetailId == selectedDetail.SkillDetailId).ToList();

                foreach (var detail in tempDetail)
                {
                    _context.CompanyDetails.RemoveRange(detail);
                }

                _context.SkillDetail.Remove(selectedDetail);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete the skill from the db. The skills are found via their ids and deleted from the ComapnySkills table. They are then delete from the Skillset table
        /// </summary>
        /// <param name="selectedFocus"></param>
        /// <returns></returns>
        public async Task DeleteFocusFromDb(Focus selectedFocus)
        {
            try
            {
                var tempFocuses = _context.CompanyFocus.Where(s => s.FocusId == selectedFocus.FocusId).ToList();

                foreach (var focus in tempFocuses)
                {
                    _context.CompanyFocus.RemoveRange(focus);
                }

                _context.Focus.Remove(selectedFocus);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete the skill from the db. The skills are found via their ids and deleted from the ComapnySkills table. They are then delete from the Skillset table
        /// </summary>
        /// <param name="selectedSkill"></param>
        /// <returns></returns>
        public async Task DeleteSkillFromDb(SkillSet selectedSkill)
        {
            try
            {
                var tempSkills = _context.CompanySkills.Where(s => s.SkillId == selectedSkill.SkillId).ToList();

                foreach (var skill in tempSkills)
                {
                    _context.CompanySkills.RemoveRange(skill);
                }

                _context.SkillSet.Remove(selectedSkill);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the Companies table with the changes made.
        /// </summary>
        /// <param name="compToEdit"></param>
        /// <returns></returns>
        public async Task EditCompany(Companies compToEdit)
        {
            try
            {
                _context.Update(compToEdit);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        /// <summary>
        /// Gets all the companies from the db
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Companies> GetAllCompanies() => _context.Companies;

        /// <summary>
        /// Get all the focuses from the db
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Focus> GetAllFocuses() => _context.Focus;

        /// <summary>
        /// Get all the skill details from db
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SkillDetail> GetAllSkillDetails() => _context.SkillDetail;

        /// <summary>
        /// Get all the skills from the db
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SkillSet> GetAllSkills() => _context.SkillSet;

        /// <summary>
        /// Get a specific company by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Companies GetCompanyById(int? id) => GetAllCompanies().FirstOrDefault(comp => comp.CompanyId == id);

        /// <summary>
        /// Get a specific detail by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillDetail GetDetailById(int id) => GetAllSkillDetails().FirstOrDefault(detail => detail.SkillDetailId == id);

        /// <summary>
        /// Get a specific focus by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Focus GetFocusById(int id) => GetAllFocuses().FirstOrDefault(focus => focus.FocusId == id);

        /// <summary>
        /// Gets the home page info from the db
        /// </summary>
        /// <returns></returns>
        public HomePage GetHomePageInfo() => _context.HomePage.FirstOrDefault();

        /// <summary>
        /// Get a specific skill by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillSet GetSkillById(int id) => GetAllSkills().FirstOrDefault(skill => skill.SkillId == id);
    }
}
