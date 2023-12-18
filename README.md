<h1 align="center"><strong>Solar-Power-Plant</strong></h1>
  <p align="center">
    <br />
    <strong>REST API for monitoring and managing solar power plant, JWT auth, code-first DB, NReco logging</strong>
    <br />
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Instructions for using the REST APIs in Swagger</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a></li>
	<li><a href="#built-with">Built With</a></li>
	<li><a href="#prerequisites">Prerequisites</a></li>
	<li><a href="#usage">Usage</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project


Main goal of this task is to develop a REST API for monitoring and managing solar power plant. The API should allow users to:

- Register and authenticate 

- Create, read, update and delete solar power plant with following attributes: 

	- Solar power plant name (optional) 

	- Installed power (mandatory) 

	- Date of installation (mandatory) 

	- Location longitude and latitude (mandatory) 

- Obtain actual or forecasted production data from a solar power plant for a specific period of time and at a preferred level of data granularity. 

Each solar power plant has actual production and production forecast records with 15 minutes granularity. Based on the user API request, service should return timeseries based on following attributes: 

- Timeseries type – real production or forecasted production 

- Timeseries granularity – 15 minutes or 1 hour, where 1 hour granularity is equal to sum of four 15 min records within respective hour

- Timeseries timespan – span for which user would like to obtain timeseries 

Requirements: 

- Use .NET to build the API.

- Use a SQL database to store data and Entity Framework to interact with the database. Use Code First approach.

- Use JWT for authentication and authorization. Users should be able to create an account, log in, and receive a token that can be used to make authorized API calls.

- The application should write logs to text files, the location of the logs needs to be configurable.

- Create Seed data function that will generate historical production data for power plants in the database allowing API testing -> solar power plant data and associated timeseries can be randomly generated.

- To forecast production, use the installed power attribute of the power plant, and weather data fetched from a free online weather API -> the prediction doesn’t need to make sense, just demonstrate that data from different sources is used in making it.

### Built With

* Visual Studio Community 2022

* .NET 8.0

<!-- GETTING STARTED -->
## Getting Started

To get started clone the repository. When you have the solution running and packages acquiered, you need to adjust the connection string if needed and run the project. This will create a database for the APIs to run up against locally on your SQL server. After that you are ready to test the APIs in the SwaggerUI.

### Prerequisites

List things you need to use i.e. NuGet Packages.

<p><strong>TARGET FRAMEWORK: .NET 8.0</strong></p>

"Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0"
"Microsoft.AspNetCore.Authentication.OpenIdConnect" Version="8.0.0" NoWarn="NU1605"
"Microsoft.AspNetCore.Components.QuickGrid.EntityFrameworkAdapter" Version="8.0.0"
"Microsoft.EntityFrameworkCore" Version="8.0.0"
"Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0"
"Microsoft.EntityFrameworkCore.Tools" Version="8.0.0"
"Microsoft.Extensions.Logging" Version="8.0.0"
"Microsoft.Identity.Web" Version="2.15.2"
"Microsoft.Identity.Web.DownstreamApi" Version="2.15.2"
"Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.0"
"NReco.Logging.File" Version="1.1.7"
"Swashbuckle.AspNetCore" Version="6.4.0"
"System.IdentityModel.Tokens.Jwt" Version="7.0.3"

<!-- USAGE EXAMPLES -->
## Usage

- <strong>Register, Login > Copy the "JWT Bearer KEY" from the response to the top section -> Authorize</strong>. Now you will be able to use the other APIs that need AUTH headers with ease.

- Use the <strong>"SolarPowerPlant" CRUD API</strong> to create, read, update and delete Solar Power Plants from the database.

- You can use the built-in <strong>"WeatherForecast"</strong> API to get a one day forecast. It was edited for the needs of this project and moved to rhe "api/controller" url

- The <strong>"Production" API</strong> can send requests that require forecasting in the future with the "isForecasted" value set to true, if false it will fetch actual plant production power. The "isGranularityHourly" value set to true will return timespan within 4 units of granularity since one unit is 15 minutes, which is the setting if the value is false. "timeSpanStart" sets the timeline starting point, where 0 is now, negative values are in the past, while a value of let's say 8 would set the starting point to 2 hours from now. "futureLenght" is used to set the time span of the response in 15 minute units (even if the granularity is hourly) after the "timeSpanStart" start point. "plantId" selects the solar power plant, by id, that we wish to extract data from.

- The in-built <strong>"Seed" function</strong> inside the "DbInit" class is used to populate the historical data to the database upon running the application. It is configurable as it has PLANT count and the associated PLANT DATA count. 

- <strong>Logging config</strong> is inside the "Program.cs" file, where you can set the path and the filename of the .log file that will be written during the application usage. Every API is logged and the application lifecycle logging is logged by default.

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<!-- LICENSE -->
## License

Distributed under the MIT License.

<!-- CONTACT -->
## Contact

Lovre Šimunović - lowwwre@gmail.com

Project Link: [https://github.com/x10ss/Solar-Power-Plant](https://github.com/x10ss/Solar-Power-Plant)