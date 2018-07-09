using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CompanyFinderAdmin.Email;
using CompanyFinderAdmin.Infrastructure;
using CompanyFinderAdmin.Models;
using CompanyFinderAdmin.ViewModels;
using DatabaseLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;

namespace CompanyFinderAdmin.Controllers
{
    /// <summary>
    /// Template controller for the company templates
    /// </summary>
    [Authorize]
    public class TemplateController : Controller
    {
        private ICompanyRepository _repo;
        private IEmailService _email;
        private readonly CompanyDbContext dbContext;
        private UserManager<IdentityUser> _userMgr;
        private SignInManager<IdentityUser> _signInMgr;
        private readonly IViewRenderService _viewRender;

        private IConfiguration _config;

        /// <summary>
        /// Constructor DI
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userMgr"></param>
        /// <param name="signInMgr"></param>
        /// <param name="repo"></param>
        /// <param name="emailService"></param>
        /// <param name="viewRenderService"></param>
        /// <param name="Configuration"></param>
        public TemplateController(CompanyDbContext context, UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr,
            ICompanyRepository repo, IEmailService emailService, IViewRenderService viewRenderService, IConfiguration Configuration)
        {
            _repo = repo;
            _email = emailService;
            _userMgr = userMgr;
            _signInMgr = signInMgr;
            dbContext = context;
            _viewRender = viewRenderService;
            _config = Configuration;
        }

        // Get the api base url from the appsetting.json
        private string Api_BaseUrl
        {
            get
            {
                return _config["Data:UrlSettings:ApiBaseUrl"];
            }
        }

        /// <summary>
        /// GET for the tamplate page login
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult TemplateLogin()
        {
            return View(new TemplateLoginViewModel());
        }

        /// <summary>
        /// POST to check the login, if successful then access is gained to the company template 
        /// </summary>
        /// <param name="templateLoginViewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> TemplateLogin(TemplateLoginViewModel templateLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userMgr.FindByNameAsync(templateLoginViewModel.CompanyAccessName);

                if (user != null)
                {
                    await _signInMgr.SignOutAsync();
                    if ((await _signInMgr.PasswordSignInAsync(user, templateLoginViewModel.CompanyAccessPassword, false, false)).Succeeded)
                    {
                        return Redirect("/Template/CompanyTemplate");
                    }

                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(templateLoginViewModel);


        }

        /// <summary>
        /// GET show the companies template ready for editing or approval
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewEditTemplate(int id)
        {

            var companyToEdit = new CreateTemplateData();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/template/gettemplate/" + id).Result;

                var responseString = await response.Content.ReadAsStringAsync();
                companyToEdit = JsonConvert.DeserializeObject<CreateTemplateData>(responseString, Converter.Settings);

            }

            return View(companyToEdit);
        }

        /// <summary>
        /// POST once template is approved the changes are pushed to the real db and the temp data deleted from the temp tables
        /// </summary>
        /// <param name="templateViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ViewEditTemplate([FromBody]CreateTemplateData templateViewModel)
        {

            var dataToSend = new StringContent(JsonConvert.SerializeObject(templateViewModel), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.PostAsync("api/template/posttemplate", dataToSend).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    ViewData["PushCompany"] = $"{templateViewModel.CompanyName} has been added to the database.";
                }
                else
                {
                    ViewData["PushCompany"] = $"Error saving {templateViewModel.CompanyName}. Please check your fields.";
                }
            }

            return View(templateViewModel);
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
                client.BaseAddress = new Uri(Api_BaseUrl);

                var responseCompanyNodes = client.GetAsync("api/template/edittemplatetreenodes/" + id).Result;

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
        public async Task<JsonResult> GetTemplateFocusNodesForEdit([FromBody]int id)
        {
            List<FocusNodesWithStateViewModel> focusNodes;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var responseCompanyNodes = client.GetAsync("api/template/edittemplatefocusnodes/" + id).Result;

                var responseStringCompanyNodes = await responseCompanyNodes.Content.ReadAsStringAsync();
                focusNodes = JsonConvert.DeserializeObject<List<FocusNodesWithStateViewModel>>(responseStringCompanyNodes, Converter.Settings);
            }

            return Json(focusNodes);
        }

        /// <summary>
        /// View to show after the template has been submitted, if the company tries to view the form again they are shown a thank you for submitting page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult ThankYou()
        {
            return View();
        }

        /// <summary>
        /// GET unlock the template for the company to edit
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        public async Task<IActionResult> UnlockTemplate(int id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/template/unlocktemplate/" + id).Result;

                var responseString = await response.Content.ReadAsStringAsync();

            }
            return RedirectToAction("TemplateStatus");
        }
        /// <summary>
        /// Remove the company from the temp company table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RemoveCompanyFromTempTable(int id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/template/removecompanyfromtemptable/" + id).Result;

                var responseString = await response.Content.ReadAsStringAsync();

            }
            return RedirectToAction("TemplateStatus");
        }

        /// <summary>
        /// GET for the company template to be filled out by companies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CompanyTemplate(Guid id)
        {
            var companyToEdit = new CreateTemplateData();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/template/companytemplate/" + id).Result;

                var responseString = await response.Content.ReadAsStringAsync();
                companyToEdit = JsonConvert.DeserializeObject<CreateTemplateData>(responseString, Converter.Settings);


                if (companyToEdit.Locked)
                {
                    return RedirectToAction("ThankYou");
                }
                else
                {
                    return View(companyToEdit);
                }
            }
        }
        /// <summary>
        /// POST to recieve the template form the company and save progress to db 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SubmittedCompanyTemplate([FromBody]CreateTemplateData templateData)
        {
            var dataToSend = new StringContent(JsonConvert.SerializeObject(templateData), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.PostAsync("api/template/submittedtemplate", dataToSend).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    //ViewData["SubmitTemplate"] = $"Success sending {templateData.CompanyName}!";
                    SendTemplateSummary(templateData);
                    //return View("CompanyTemplate");
                    return RedirectToAction("ThankYou");
                }
                else
                {
                    ViewData["SubmitTemplate"] = $"Error sending {templateData.CompanyName}. Please check your fields.";
                    return View("CompanyTemplate", templateData);
                }
            }

        }

        /// <summary>
        /// POST to recieve the template form the company and save progress as a draft to db 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> DraftCompanyTemplate([FromBody]CreateTemplateData templateData)
        {
            var dataToSend = new StringContent(JsonConvert.SerializeObject(templateData), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.PostAsync("api/template/drafttemplate", dataToSend).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["templateMessage"] = $"Your information has been saved as a draft! You can continue to edit before submitting.";
                    return View("CompanyTemplate", templateData);
                }
                else
                {
                    TempData["templateMessage"] = $"Error saving {templateData.CompanyName}. Please check your fields.";
                    return View("CompanyTemplate", templateData);
                }
            }
        }

        private int FindCompanyIdFromTempCompanyTable()
        {
            int newId = 0;

            if (!_repo.GetAllCompaniesFromTempTable().Any())
            {
                newId = 1;
            }
            else
            {
                // Get the last antered Id from the db table and +1 ready to assign to new companyToEdit
                var lastId = _repo.GetAllCompaniesFromTempTable().OrderByDescending(i => i.CompanyId).First();
                newId = lastId.CompanyId;
                newId++;
            }

            return newId;
        }

        /// <summary>
        /// GET's the companies from the temp company template table, loads the view which admin can send templates from
        /// </summary>

        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> TemplateStatus()
        {
            var template = new SendTemplateViewModel();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.GetAsync("api/template/gettemplatestatus/").Result;

                var responseString = await response.Content.ReadAsStringAsync();
                template = JsonConvert.DeserializeObject<SendTemplateViewModel>(responseString, Converter.Settings);

                if (response.IsSuccessStatusCode)
                {
                    return View(template);
                }
                else
                {
                    var templateNoData = new SendTemplateViewModel
                    {
                        CompanyList = new List<TemporaryCompanyTemplate>()
                    };
                    return View(templateNoData);
                    //TempData["message"] = $"Error fetching data. Please check your server.";
                    //return RedirectToAction("Dashboard", "Admin");
                }
            }

        }

        /// <summary>
        /// POST emails the template to the company and updates the temp comapny template table
        /// </summary>
        /// <param name="sendTemplate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> TemplateStatus(SendTemplateViewModel sendTemplate)
        {
            //Unique guid to identify the company
            var guid = Guid.NewGuid();

            // Link for the company to open with unique guid
            var link = @Url.Action("CompanyTemplate", "Template", new { id = guid }, "https");

            sendTemplate.Link = link;
            sendTemplate.CompanyGuid = guid;


            var dataToSend = new StringContent(JsonConvert.SerializeObject(sendTemplate), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.PostAsync("api/template/savetemplatestatus", dataToSend).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = $"Template has been sent to {sendTemplate.CompanyName}";
                    var httpResponse = new HttpResponseMessage();

                    httpResponse = client.GetAsync("api/template/gettemplatestatus/").Result;

                    var httpResponseString = await httpResponse.Content.ReadAsStringAsync();
                    sendTemplate = JsonConvert.DeserializeObject<SendTemplateViewModel>(httpResponseString, Converter.Settings);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return View(sendTemplate);
                    }
                    else
                    {
                        var templateNoData = new SendTemplateViewModel
                        {
                            CompanyList = new List<TemporaryCompanyTemplate>()
                        };
                        return View(templateNoData);
                    }
                }
                else

                {
                    TempData["errorMessage"] = $"Error sending template. " + response.ReasonPhrase + " Please check your email settings.";
                    return View(sendTemplate);
                }
            }
        }
        /// <summary>
        /// A Summary of the companies submitted information, to be sent as an email after submition of form.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async void SendTemplateSummary(CreateTemplateData template)
        {
            var dataToSend = new StringContent(JsonConvert.SerializeObject(template), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Api_BaseUrl);

                var response = new HttpResponseMessage();

                response = client.PostAsync("api/template/sendtemplatesummary", dataToSend).Result;

                var responseString = await response.Content.ReadAsStringAsync();

            }
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
                client.BaseAddress = new Uri(Api_BaseUrl);

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
                client.BaseAddress = new Uri(Api_BaseUrl);

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
    }
}