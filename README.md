# Competence Administrator

A website that enables an admin to control the database. The user can add edit and delete elements from the database tables.

#### Built with:
* .NET Core 2.0
* Entity Framework Core 2.0
* Bootstrap 4

#### Setup:
Go into the appsettings.json file and change the CompanyConnection connection string to point towards your server. Give your database a name in the connection string and create it locally. Change the IdentityConnection connection string to point towards your server and give it's database a name. Ths is used to store login information.

You can also change the admin login details and the user login details via the appsettings.json file. Once you have done that, run the scripts in the given order to set up your company database. The Identity database will be set up automatically when you first run the application. Set the UrlSettings ApiBaseUrl to point towards the api address with the correct port number.

Set the email configuration to point towrds you email provider, server, port, username and password if applicable.

Once logged in you will be able to choose which industry you would like to start entering information into the databse for. At the present time only the IT sector is available

From here you will have 7 options:
1. Create a new company
   * Enter the companies details (Name, Contact Person, Email, Website, Phone, Bio)
   * Select which skills and programming languages align with this company (Note: if you havent entered any skills or languages into the database then this section will be blank)
   * Choose a company focus; Product Company is a company which produces products that they then sell on to other companies, Consultancy Company is a company which offers consultancy services, Internal Systems is a company which develops and makes its own internal systems (this can align with a product company which produces products for others while also using those same products in-house)

2. Edit or delete existing companies
   * View all the companies saved in the database. Basic information is shown in a table format
   * Search through all the companies via a search box
   * Click through to edit or delete a company
   * When editing a company you will be presented with the same view as the create company but the information will already be filled in, you can make changes to all the data fields and hit save

3. View, edit or delete the skills and details in the database
   * The same format as the view all companies section where you can view all the saved skillsets and details in the database
   * Click through to edit or delete a skill or detail
   * Search through all the skills and details via a search box

4. Create the Treeview
   * Create nodes to represent the data structure on the main app. The root node "Roles" is added when you run the scripts. From here you can create parent nodes and any related child nodes. For example: Programming would be a parent node, whode parent is Roles. C# would be a child node of Programming, so select Programming as the parent node. You can view the current state of the treeview at any time.

5. Add a focus to the database
   * Choose a focus name and hit save

6. Send Templates to potential companies
   * Enter the companies email address and its name and hit send, the company will recieve an email with a link to a unique template. They can view and edit this as many times as they want. When they choose to submit the form you will see that they have returned it here with the time and date. From here on teh template is locked to the comapany, if they get in contact and wish to alter it you can unlock the template and their link will be active again. You can view the template under the edit button. Look it over and make any changes if you like, otherwise hit apporve and the companies details will be sent the the database ready to be included in any search results.

7. Edit the main apps home page
   * Choose a tagline for the main apps home page.



