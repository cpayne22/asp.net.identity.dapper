asp.net Identity v2.1 + Dapper
==========

I needed to get a better understanding of the [asp.net Identity](http://www.asp.net/identity) so I wrote a version of it for [Dapper](https://github.com/StackExchange/dapper-dot-net)

Based on the [Asp.Net.Identity.MySQL](https://aspnet.codeplex.com/SourceControl/latest#Samples/Identity/AspNet.Identity.MySQL/) and https://github.com/rustd/AspnetIdentitySample, this is an implementation using Dapper.

Unit tests inspired from https://github.com/tugberkugurlu/AspNet.Identity.RavenDB

Includes Dapper, Dapper Extensions & Miniprofiler


Help me out!  If something doesn't look right, let me know.


Quick Start
-----------

- Download & build
- Run database.sql to generate the tables
- Update the connection string in web.config


Todo
-----------
- Because there is no lazy loading with Dapper, the framework does a LOT of database hits!  It needs caching or something to sort that out...

Enjoy!
@christian_pyn


[![Build status](https://ci.appveyor.com/api/projects/status/f4jr87cd41owdjr5/branch/master?svg=true)](https://ci.appveyor.com/project/cpayne22/asp-net-identity-dapper/branch/master)
