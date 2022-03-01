# buycoins_engineering_test

Deployed Url: http://buycoinstest.azurewebsites.net/graphql

Postman Documentation: https://documenter.getpostman.com/view/11187475/UVksKtb9 

## What's a good reason why the pure Levenshtein Distance algorithm might be a more effective solution than the broader Damerau–Levenshtein Distance algorithm in this specific scenario.

The Levenshtein Distance (LD) and the Damerau-Levenshtein Distance (DLD) algorithms are both from the family of edit distance algorithms and work on the basis of insertion, substitution and deletion. But with DLD it includes transposition, which means it does not preserve positions. This might be fine for general text in which semantics could be use to compensate for positional change, but with names where there are no semantics positions need to be preserved. 



## Assumptions: 
1.	Each Request to the endpoint is that of a new user, hence there are no checks in the database to see if the user has been verified before. All requests means a trip to the Paystack API every time.

## Project Structure and Setup: 

buycoins_test: Houses the who code built on asp.net Core 6.0 and can be ran locally 

buycoins_test.Tests: houses the integration test build using the xunit test and can also be ran locally. 

