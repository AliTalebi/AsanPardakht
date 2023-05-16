# Runtime Elements

## Address Manegement
  In this element you register base information includes (Province, City and Person)
  
## Console App Scheduler
  this element automatically retrieves data from above element by using two options: processing outbox events and calling rest apis 
  
# Architecture and Patterns

Onion, DDD, CQRS and Outbox pattern

# How to Run?

For address management build solution then apply migrations, set Api project as the stratup project, now you can run the app or publish it and move to anywhere you want to deploy, don't forget to install .NET 6 runtime in destination

for Console App Scheduler what you need is only build and start the app (using visual studio or double click on .exe file), database will be created

# Authorization

basic authorization and default user and pass for calling apis: ali.talebi:123

# Tests
under development
