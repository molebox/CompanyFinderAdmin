using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompanyFinderAdmin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CompanyFinderAdmin.Models;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Localization;
using CompanyFinderAdmin.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace CompanyFinderAdmin.Controllers
{
    /// <summary>
    /// Admin controller used to responed to login requests made from the home page
    /// </summary>
    [Authorize]
    public class AdminController : Controller
    {
        private UserManager<IdentityUser> _userManger;
        private SignInManager<IdentityUser> _signInManager;
        private ICompanyRepository _repository;
        private CompanyDbContext _context;
        private IConfiguration _configuration;

        /// <summary>
        /// Holds the company to edits id
        /// </summary>
        public int EditCompId { get; set; }

        /// <summary>
        /// Constructor for Admin Controller. Gets the context from the repo, also gets the context directly for use in certain methods
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="signInMgr"></param>
        /// <param name="repo"></param>
        /// <param name="context"></param>
        /// <param name="Configuration"></param>
        public AdminController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr, ICompanyRepository repo, CompanyDbContext context, IConfiguration Configuration)
        {
            _userManger = userMgr;
            _signInManager = signInMgr;
            _repository = repo;
            _context = context;
            _configuration = Configuration;
        }

        // Get the api base url from the appsetting.json
        private string ApiBaseUrl
        {
            get
            {
                return _configuration["Data:UrlSettings:ApiBaseUrl"];
            }
        }

        private async Task<List<Companies>> GetAllCompaniesFromApiAsync()
        {
            List<Companies> compList = new List<Companies>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/company/getallcompanies").Result;

                var responseString = await response.Content.ReadAsStringAsync();
                compList = JsonConvert.DeserializeObject<List<Companies>>(responseString, Converter.Settings);
            }

            return compList;
        }

        /// <summary>
        /// GETs all the companies from the db and shows them in the view. The page size determines how many will be shown on each page.
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ViewResult> CompanyIndex(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            //var companies = await GetAllCompaniesFromApiAsync();

            var companies = from s in _context.Companies
                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                companies = companies.Where(s => s.CompanyName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    companies = companies.OrderByDescending(s => s.CompanyName);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Companies>.CreateAsync(companies.AsNoTracking(), page ?? 1, pageSize));


        }

        #region LOGIN
        /// <summary>
        /// GET for the admin login view. Check the db to see if any admins exist, if not then set the usernam to default "admin" and prompt to change username and password.
        /// If admin exists already then they can login as usual
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new AdminViewModel());
        }

        /// <summary>
        /// Post method that checks the login for access to the search page. If login is good then the user is redirected to the search oage, if the login
        /// is bad an error message is displayed
        /// </summary>
        /// <param name="adminViewModel"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminViewModel adminViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser admin = await _userManger.FindByNameAsync(adminViewModel.AdminName);

                if (admin != null)
                {
                    await _signInManager.SignOutAsync();
                    if ((await _signInManager.PasswordSignInAsync(admin, adminViewModel.AdminPassword, false, false)).Succeeded)
                    {
                        return Redirect("/Admin/IndustryFilter");
                    }

                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(adminViewModel);

        }

        /// <summary>
        /// Logout method to close the session
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
        #endregion

        /// <summary>
        /// GET for the Admin filter view
        /// </summary>
        /// <returns></returns>
        public IActionResult IndustryFilter()
        {
            return View();
        }

        /// <summary>
        /// GET Admin dashboard where they can choose what they want to do
        /// </summary>
        /// <returns></returns>
        public IActionResult Dashboard()
        {
            return View();
        }

        /// <summary>
        /// GET to load the tagline from the db for the main apps home page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult MainAppHomePage()
        {
            var tagline = _repository.GetHomePageInfo();
            var tagLineViewModel = new TagLineViewModel();

            if (tagline != null)
            {
                tagLineViewModel.Tagline = tagline.TagLine;
                return View(tagLineViewModel);
            }
            else
            {
                return View(new TagLineViewModel());
            }
        }

        /// <summary>
        /// Post to add the tagline to the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tagLineViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MainApphomePage(int id, TagLineViewModel tagLineViewModel)
        {
            if (ModelState.IsValid)
            {
                var tag = _context.HomePage.FirstOrDefault();
                if (tag != null)
                {
                    _context.HomePage.Remove(tag);
                }

                // Create new home page object and add the new tagline
                var homePageInfo = new HomePage
                {
                    TagLine = tagLineViewModel.Tagline
                };

                // Save the new tagline to the database and return to the view
                _context.HomePage.Add(homePageInfo);
                _context.SaveChanges();
                TempData["homepageMessage"] = $"{tagLineViewModel.Tagline} has been added as the new tagline.";
                return View(tagLineViewModel);
            }
            return View(tagLineViewModel);
        }

        /// <summary>
        /// View to create the treview structure. The nodes are loaded from the api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateTreeNodes() => View();

        /// <summary>
        /// The Post back of the new treeview state, here the new nodes are added to the db via the api
        /// </summary>
        /// <param name="saveTreeViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTreeNodes([FromBody]NodesToSend saveTreeViewModel)
        {
            //var returnedData = new NodesToSend();

            if (ModelState.IsValid)
            {
                // Tree nodes from db
                //List<TreeNodes> treeNodes;

                var dataToSend = new StringContent(JsonConvert.SerializeObject(saveTreeViewModel), Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new Uri(ApiBaseUrl);

                    var response = new HttpResponseMessage();

                    response = client.PostAsync("api/company/savetree", dataToSend).Result;

                    var responseString = await response.Content.ReadAsStringAsync();
                    //returnedData = JsonConvert.DeserializeObject<NodesToSend>(responseString, Converter.Settings);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["addNodesMessage"] = $"Tree has been saved to the database";
                        //return View();
                    }
                    else
                    {
                        TempData["addNodesMessage"] = $"Error saving tree. Please try again.";
                        //return RedirectToAction("Dashboard", "Admin");
                    }
                }
            }
            return Json(saveTreeViewModel);

        }


        /// <summary>
        /// Get the tree nodes from the api
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetTreeNodes(string query)
        {
            // Tree nodes from db
            List<TreeNodes> treeNodes;
            // Tree nodes view model
            List<TreeNodesViewModel> treeNodesViewModel;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/company/gettreenodes").Result;

                var responseString = await response.Content.ReadAsStringAsync();
                treeNodes = JsonConvert.DeserializeObject<List<TreeNodes>>(responseString, Converter.Settings);
            }


            if (!string.IsNullOrWhiteSpace(query))
            {
                treeNodes = treeNodes.Where(q => q.Name.Contains(query)).ToList();
            }

            treeNodesViewModel = treeNodes.Where(l => l.ParentId == null)
                    .Select(l => new TreeNodesViewModel
                    {
                        Text = l.Name,
                        Id = l.Id,
                        ParentId = l.ParentId,
                        OrderNumber = l.OrderNumber,
                        Children = GetRolesChildren(treeNodes, l.Id)
                    }).ToList();

            return Json(treeNodesViewModel);
        }

        /// <summary>
        /// Gets all the company focuses from the db via the repo and returns them to the view.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetFocusNodesAsync(string query)
        {
            List<FocusNodes> focusNodes;
            List<FocusNodeViewModel> focusNodesViewModel;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/company/getfocusnodes").Result;

                var responseString = await response.Content.ReadAsStringAsync();
                focusNodes = JsonConvert.DeserializeObject<List<FocusNodes>>(responseString, Converter.Settings);
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                focusNodes = focusNodes.Where(q => q.Name.Contains(query)).ToList();
            }

            focusNodesViewModel = focusNodes.Where(l => l.ParentId == null)
                    .Select(l => new FocusNodeViewModel
                    {
                        Text = l.Name,
                        Id = l.Id,
                        ParentId = l.ParentId,
                        Children = GetFocusChildren(focusNodes, l.Id)
                    }).ToList();

            return Json(focusNodesViewModel);
        }

        private List<TreeNodesViewModel> GetRolesChildren(List<TreeNodes> nodes, int parentId)
        {
            return nodes.Where(l => l.ParentId == parentId).OrderBy(l => l.OrderNumber)
                .Select(l => new TreeNodesViewModel
                {
                    Text = l.Name,
                    Id = l.Id,
                    ParentId = l.ParentId,
                    OrderNumber = l.OrderNumber,
                    Children = GetRolesChildren(nodes, l.Id)
                }).ToList();
        }
        private List<TreeNodesWithStateViewModel> GetRolesChildrenState(List<TreeNodesWithStateViewModel> nodes, int parentId)
        {
            return nodes.Where(l => l.ParentId == parentId).OrderBy(l => l.OrderNumber)
                .Select(l => new TreeNodesWithStateViewModel
                {
                    Text = l.Text,
                    Id = l.Id,
                    ParentId = l.ParentId,
                    OrderNumber = l.OrderNumber,
                    Children = GetRolesChildrenState(nodes, l.Id),
                    State = l.State
                }).ToList();
        }

        private List<FocusNodeViewModel> GetFocusChildren(List<FocusNodes> nodes, int parentId)
        {
            return nodes.Where(l => l.ParentId == parentId).OrderBy(l => l.OrderNumber)
                .Select(l => new FocusNodeViewModel
                {
                    Text = l.Name,
                    Id = l.Id,
                    ParentId = l.ParentId,
                    Children = GetFocusChildren(nodes, l.Id)
                }).ToList();
        }
        /// <summary>
        /// Posts the treeview to the api where it is added to the database
        /// </summary>
        /// <param name="saveTreeViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveTree([FromBody]NodesToSend saveTreeViewModel)
        {
            // Tree nodes from db
            List<TreeNodes> treeNodes;

            var dataToSend = new StringContent(JsonConvert.SerializeObject(saveTreeViewModel), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();

                response = client.PostAsync("api/company/savetree", dataToSend).Result;

                var responseString = await response.Content.ReadAsStringAsync();
                treeNodes = JsonConvert.DeserializeObject<List<TreeNodes>>(responseString, Converter.Settings);

                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = $"Tree has been saved to the database";
                    return Json(treeNodes);
                }
                else
                {
                    TempData["message"] = $"Error saving tree. Please try again.";
                    return Json(saveTreeViewModel);
                }
            }
        }


        #region CREATE COMPANY  
        /// <summary>
        /// GET to create a new company. The Skills and Details are loaded from the api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateCompany()
        {
            var companyEditor = new CompanyEditorViewModel();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/company/getnewcompanyinfo").Result;

                var responseString = await response.Content.ReadAsStringAsync();
                companyEditor = JsonConvert.DeserializeObject<CompanyEditorViewModel>(responseString, Converter.Settings);
            }

            return View(companyEditor);
        }

        /// <summary>
        /// POST to create a new company. An id check if first performed to see if the db is empty and/or to assign a new id to the new company.
        /// 3 tables are edited to create the new company = Companies-CompanySkills-CompanyDetails
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody]CreateCompanyData company)
        {

            var dataToSend = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();

                response = client.PostAsync("api/company/createcompany", dataToSend).Result;

                var responseString = await response.Content.ReadAsStringAsync();
                //companyEditor = JsonConvert.DeserializeObject<CompanyEditorViewModel>(responseString, Converter.Settings);

                if (response.IsSuccessStatusCode)
                {
                    TempData["createCompanyMessage"] = $"{company.CompanyName} has been saved";
                }
                else
                {
                    TempData["createCompanyMessage"] = $"Error saving {company.CompanyName}. Please check your fields.";
                }
            }

            return Json(company);
        }
        #endregion

        #region EDIT COMPANY

        /// <summary>
        /// Get the tree nodes from the api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetTreeNodesForEdit([FromBody]int id)
        {
            List<TreeNodesWithStateViewModel> treeNodes;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var responseCompanyNodes = new HttpResponseMessage();

                responseCompanyNodes = client.GetAsync("api/company/editcompanytreenodes/" + id).Result;

                var responseStringCompanyNodes = await responseCompanyNodes.Content.ReadAsStringAsync();
                treeNodes = JsonConvert.DeserializeObject<List<TreeNodesWithStateViewModel>>(responseStringCompanyNodes, Converter.Settings);
            }

            return Json(treeNodes);
        }
        /// <summary>
        /// Get the tree nodes from the api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetTemplateTreeNodesForEdit([FromBody]int id)
        {
            List<TreeNodesWithStateViewModel> treeNodes;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var responseCompanyNodes = new HttpResponseMessage();

                responseCompanyNodes = client.GetAsync("api/company/editcompanytreenodes/" + id).Result;

                var responseStringCompanyNodes = await responseCompanyNodes.Content.ReadAsStringAsync();
                treeNodes = JsonConvert.DeserializeObject<List<TreeNodesWithStateViewModel>>(responseStringCompanyNodes, Converter.Settings);
            }

            return Json(treeNodes);
        }
        /// <summary>
        /// Get the tree nodes from the api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetFocusNodesForEdit([FromBody]int id)
        {
            List<FocusNodesWithStateViewModel> focusNodes;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var responseCompanyNodes = new HttpResponseMessage();

                responseCompanyNodes = client.GetAsync("api/company/editcompanyfocusnodes/" + id).Result;

                var responseStringCompanyNodes = await responseCompanyNodes.Content.ReadAsStringAsync();
                focusNodes = JsonConvert.DeserializeObject<List<FocusNodesWithStateViewModel>>(responseStringCompanyNodes, Converter.Settings);
            }

            return Json(focusNodes);
        }
        /// <summary>
        /// Get the focus tree nodes from the api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetTemplateFocusNodesForEdit([FromBody]int id)
        {
            List<FocusNodesWithStateViewModel> focusNodes;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var responseCompanyNodes = new HttpResponseMessage();

                responseCompanyNodes = client.GetAsync("api/company/edittemplatefocusnodes/" + id).Result;

                var responseStringCompanyNodes = await responseCompanyNodes.Content.ReadAsStringAsync();
                focusNodes = JsonConvert.DeserializeObject<List<FocusNodesWithStateViewModel>>(responseStringCompanyNodes, Converter.Settings);
            }

            return Json(focusNodes);
        }

        /// <summary>
        /// GET for the admin company edit view. The company is found in api via its id, the corrasponding company is then found in the CompanySkills and CompanyDetails tables.
        /// The values are passed to a view model and then sent to the view for editing.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditCompany(int id)
        {
            var companyEditor = new CompanyEditorViewModel();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/company/editcompany/" + id).Result;

                var responseString = await response.Content.ReadAsStringAsync();
                companyEditor = JsonConvert.DeserializeObject<CompanyEditorViewModel>(responseString, Converter.Settings);
            }

            return View(companyEditor);
        }

        /// <summary>
        /// POST for the admin company edit view. The company data is sent to the api where the database is updated.
        /// </summary>
        /// <param name="companyToEdit"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCompany([FromBody]CreateCompanyData companyToEdit)
        {
            var dataToSend = new StringContent(JsonConvert.SerializeObject(companyToEdit), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();

                response = client.PostAsync("api/company/editcompany", dataToSend).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["editCompanyMessage"] = $"{companyToEdit.CompanyName} has been saved";
                }
                else
                {
                    TempData["editCompanyMessage"] = $"Error saving {companyToEdit.CompanyName}. Please check your fields.";
                }
            }

            return Json(companyToEdit);

        }

        private bool CompanyExists(int id)
        {
            return _repository.GetAllCompanies().Any(e => e.CompanyId == id);
        }
        #endregion


        #region ADD FOCUS
        /// <summary>
        /// GET for the add skills admin view. 
        /// </summary>
        /// <returns></returns>
        public IActionResult AddFocus()
        {
            return View(new AddFocusViewModel());
        }

        /// <summary>
        /// POST to add new skills to the db. An id check if first performed to see if the db is empty and/or to assign a new id to the new skill.
        /// </summary>
        /// <param name="addFocusViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFocus([Bind("FocusId, FocusType")] AddFocusViewModel addFocusViewModel)
        {
            var focus = new AddFocusViewModel();

            var dataToSend = new StringContent(JsonConvert.SerializeObject(addFocusViewModel), Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var responseAddNode = new HttpResponseMessage();


                responseAddNode = client.PostAsync("api/company/addfocus", dataToSend).Result;

                var responseString = await responseAddNode.Content.ReadAsStringAsync();
                focus = JsonConvert.DeserializeObject<AddFocusViewModel>(responseString, Converter.Settings);

                if (responseAddNode.IsSuccessStatusCode)
                {
                    TempData["focusMessage"] = $"{focus.FocusType} has been saved";
                }
                else
                {
                    TempData["focusMessage"] = $"Error saving {focus.FocusType}. Please try again.";
                }
            }

            return View(focus);
        }

        #endregion

        #region INDEX SKILLS DETAILS AND FOCUS
        /// <summary>
        /// GET for the add skills and details admin view. The skills and details are loaded via the repo from the db and shown in a table format.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> IndexSkillsDetailsFocus()
        {
            var sd = new SkillsDetailsFocusListViewModel();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();
                //var responseFocus = new HttpResponseMessage();

                response = client.GetAsync("api/company/indexskillsdetailsfocus").Result;
                //responseFocus = client.GetAsync("api/company/getfocusnodes").Result;

                var responseString = await response.Content.ReadAsStringAsync();
                sd = JsonConvert.DeserializeObject<SkillsDetailsFocusListViewModel>(responseString, Converter.Settings);


            }

            return View(sd);
        }
        #endregion

        #region EDIT & DELETE SKILL, DETAILS & FOCUS
        /// <summary>
        /// GET for the edit skill admin view. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditFocus(int id)
        {
            var findFocus = _repository.GetFocusById(id);

            var focus = new AddFocusViewModel
            {
                FocusId = findFocus.FocusId,
                FocusType = findFocus.FocusType
            };



            return View(focus);
        }

        /// <summary>
        /// POST for the edit skill admin view. The skills id is found in the db and its new values assigned.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addFocusViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditFocus(int id, [Bind("FocusId, FocusType")] AddFocusViewModel addFocusViewModel)
        {

            var findFocus = _repository.GetFocusById(id);
            findFocus.FocusType = addFocusViewModel.FocusType;


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(findFocus);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (findFocus == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["editFocusMessage"] = $"{findFocus.FocusType} has been saved";
                return View(addFocusViewModel);
            }
            return View(addFocusViewModel);
        }


        /// <summary>
        /// GET for the delete details admin view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteFocus(int id)
        {
            var focus = _repository.GetFocusById(id);

            if (focus == null)
            {
                return NotFound();
            }

            return View(focus);
        }

        /// <summary>
        /// POST for the confirmation to delete detail admin view. The details id is sent to the repo and located in the db, its then sent back to the repo to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteFocus")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFocusConfirmed(int id)
        {
            var focus = _repository.GetFocusById(id);

            await _repository.DeleteFocusFromDb(focus);
            TempData["deleteFocusMessage"] = $"{focus.FocusType} has been deleted";
            return RedirectToAction(nameof(IndexSkillsDetailsFocus));
        }

        /// <summary>
        /// GET for the admin delete company view. The companys id is sent to the repo and foudn in the db.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteCompany(int? id)
        {
            var companyEditor = new CompanyEditorViewModel();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/company/getcompanybyid/" + id).Result;

                var responseString = await response.Content.ReadAsStringAsync();
                companyEditor = JsonConvert.DeserializeObject<CompanyEditorViewModel>(responseString, Converter.Settings);
            }

            return View(companyEditor);
        }


        /// <summary>
        /// POST for the confirmation to delete company admin view. The companies id is sent to the repo and located in the db, its then sent back to the repo to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteCompany")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCompanyConfirmed(int id)
        {
            //var compToDelete = new CompanyEditorViewModel();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(ApiBaseUrl);

                var response = new HttpResponseMessage();
                response = client.DeleteAsync("api/company/deletecompany/" + id).Result;

                var responseString = await response.Content.ReadAsStringAsync();
                //compToDelete = JsonConvert.DeserializeObject<CompanyEditorViewModel>(responseString, Converter.Settings);

                if (response.IsSuccessStatusCode)
                {
                    TempData["deleteCompanyMessage"] = $"Company has been deleted";
                    return View();
                    //return RedirectToAction("CompanyIndex");
                }
                else
                {
                    TempData["deleteCompanyMessage"] = $"Error deleting company. Please try again.";
                    return View();
                    //return RedirectToAction("CompanyIndex");
                }
            }
        }
        #endregion

        /// <summary>
        /// Sets the languge
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                //"Preferences",
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

    }
}