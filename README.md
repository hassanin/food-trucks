# Food Truck Locator
This project is a web API that has a dataset of all food trucks in the SF bay area. It allows users to query the data and return the closest trucks to their location. It also allows the users to search for certain items such as "Noodles" or Sushi, and so on and return the results sorted by how far the truck is.

# Running the Solution
You need visual studio to debug / develop the solution. You open the solution file `food-trucks.sln` and you can run the test project from there.

## APIs exposed
/api/food-trucks/all     -> Returns all food trucks
/api/food-trucks/location {location} {maxItems}   returns the closest food trucks to said location
/api/food-trucks/search  {location} {searchTerm} {maxITems} returns the closest food truck that match the search Terms. The search here is not exact word match, but semantic search and this step will be expalined in detail below.

There is also a swagger openAPI that ships with the project that you can try out those different APIs. I did not have time to create a CLI unforunaetly.
# Project Focus
There are so many moving pieces and extra features that can be added to the project. I have focused on scafolding the backend service and designing it such that it is easily extensible to other data backing services/ Web APIs. The design makes heavy use of Dependency Injection and loose coupling of components.

- All the backing services were in memory based databsaes. No extrnal database has been used in the project. As mentioned earlier in the description it should be fairly simple to implemennt a service for external databases such as CosmosDB and ElasticSearch or Redis. 

- Scaffolding and choosing the appropriate interfaces, APIs, etc, took the most of the 3 hours allocated to the project. It is fairly simple to implement most of the services in the project usning real databases.

# Architecure
1. The backend service is an HTTP server that accepts the users query.
2. The restaurant data is loaded into a NOSQL database (for example CosmosDB) which will serve as the source of truth.
3. The restaurant data is indexed into ElasticSearch database to make semantic searches faster.
4. Api calls are cached (location,searchTerms) -> foodTrucks up to a certain interval (typically 7 days). The cache that will be used is Redis.
5. the backend service will be placed behing a Loabd balaner and will be completley stateless. All data is persited in external services (CosmosDB, ElasticSearch and Redis).
6. Scaling the solution would require no down time , since we can increase the number of service backend instances, increase te allocate Read/Write Throughput for the SaaS databases or add nodes to the hosted service which will behing the scene perform repartitioning / resharing of the data.

# Highlights
1. When using CosmosDB or a similar services, there is a special GeoLocation data type that can be used and the location data will be indexed. This allows queries such as , GET me the closest 5 location to this location, really fast. We do not have to do a scan on the DB to compare the difference.
2. Right now the API only accepts a GeoLocation (longitude,latitude). It is very easy to accept an Address, but for that we need to convert the address to a GeoLocation using something like Google GeoLocation APIs.
3. Currently the food trucks data is read from a CSV file, I did not have time to read it dynamically from the SF city website and update the local database. A small microservice can be responsible for that

# Performance improvment
1. We can construct a grid for the different locations. For exzmple we can partiion the city of SF into square grids of 0.25 * 0.25 mile area. And we treat all locations within the same grid as the same. This allows very efficent caching for search results within that grid.

# Discussion
