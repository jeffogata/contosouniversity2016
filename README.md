# Contoso University 2016

The Microsoft Contoso University sample application, rebuilt using the ASP.NET 5 stack.  

Also uses AutoMapper, MediatR, RoundhousE, and Sake.

## Assumptions/Restrictions
* Assumes you have a SQL Server default instance accessible as `localhost`.
  * To create a `localhost` alias for a named intance, see this [blog post](https://blog.mariusschulz.com/2014/02/09/how-to-configure-a-sql-server-alias-for-a-named-instance-on-a-development-machine).
  * To use a different data source, you will need to modify connection strings in the build script and source.  I plan to move this information to configuration or environment settings soon.
* No cross-platform support; the third party projects used are currently Windows only.

## Initial Build - Command Line

You do not need Visual Studio 2015 installed to build and run from the command line.  Run the following commands:

```
> build.cmd
> run.cmd
```

`build.cmd` will install dnx if needed, build the database, and run tests.
`run.cmd` will start the Kestrel server and launch a browser window to the site.

To see more build targets, run `> build.cmd help`.  For more on Sake, see the [sakeproject](https://github.com/sakeproject/sake) and also this work in progress [Sake documentation](http://sake-docs.readthedocs.org/en/latest/).

## Running in Visual Studio 2105

Install the [ASP.NET 5 tooling for VS2015](https://docs.asp.net/en/latest/getting-started/installing-on-windows.html#install-asp-net-5-with-visual-studio).

Before running the project, build the databases using the build script, or by running the initial schema scripts in SQL Server Management Studio.

To build the databases using the build script:

`> build.cmd rdb`

To build using the sql scripts, create 2 databases named `ContosoUniversity2016` and `ContosoUniversity2016.Tests`.  Run the initial schema script found in `src\DatabaseMigration\up` on both databases.

## Current Project State

The project is functionally complete, but still has much room for improvement.  Areas I hope to explore include custom tag helpers, view components, and better handling of information like connection strings.  Additionally, the build needs work to clean up last-minute workarounds (had problems with the path variable not updating in the same session that dnx was installed).

Larger features like authentication/authorization and localization will be tackled as part of a more ambitious future project.
